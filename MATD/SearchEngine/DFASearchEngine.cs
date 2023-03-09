#define ASCIIOnly

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ExactPatternMatching
{
    public class DFASearchEngine : SearchEngine
    {
#if !ASCIIOnly
        private readonly char[] alphabet;
        private readonly Dictionary<char, int> alphaDict;
#endif

        private readonly List<int[,]> DFAs;

        private readonly int alphabetLength = 128;

        private int[,] currentDFA;


        public DFASearchEngine(string text, List<string> patterns) : base(text, patterns) 
        {
#if !ASCIIOnly
            alphabet = text.Distinct().ToArray();
            alphaDict = new Dictionary<char, int>();

            alphabetLength = alphabet.Length;
            for (int i = 0; i < alphabetLength; i++)
                alphaDict[alphabet[i]] = i;
#endif

            DFAs = new List<int[,]>();

            for (int i = 0; i < patternsLength; i++)
                DFAs.Add(CreateDFA(patterns[i]));

            currentDFA = CreateDFA(placeholder);
        }

        protected override void DoSearch(EPrintType printType)
        {
            var watch = Stopwatch.StartNew();

            for (int i = 0; i < patternsLength; i++)
            {
                currentDFA = DFAs[i];
                var pattern = patterns[i];

                results.Add(pattern, DoSearchAlgorithm(text, pattern));
            }

            watch.Stop();

            Console.WriteLine($"[DFA] Elapsed {watch.ElapsedMilliseconds} ms\n");
            PrintResults(printType);
        }
        protected override List<int> DoSearchAlgorithm(string text, string pattern)
        {
            List<int> resultIndices = new List<int>();

            int patternLength = pattern.Length;
            int state = 0;

            for (int i = 0; i < textLength; i++)
            {
#if ASCIIOnly
                char c = text[i];
                if (c >= alphabetLength) // Not in ASCII
                {
                    state = 0;
                    continue;
                }

                state = currentDFA[state, c];
#else
                state = currentDFA[state, alphaDict[text[i]]];
#endif

                if (state == patternLength)
                    resultIndices.Add(i - patternLength + 1);
            }

            return resultIndices;
        }

        private int[,] CreateDFA(string pattern)
        {
            int numOfStates = pattern.Length + 1;
            var DFA = new int[numOfStates, alphabetLength];

            for (int i = 0; i < numOfStates; i++)
            {
                for (char j = (char)0; j < alphabetLength; j++)
                {
#if ASCIIOnly
                    DFA[i, j] = GetNextState(pattern, i, j);
#else
                    DFA[i, j] = GetNextState(pattern, i, alphabet[j]);
#endif
                }
            }

            return DFA;
        }
        private int GetNextState(string pattern, int state, char c)
        {
            if (state < pattern.Length && c == pattern[state])
                return state + 1;

            // i -> longest prefix (next state)
            for (int i = state; i > 0; i--)
            {
                int next = i - 1;

                if (pattern[next] != c)
                    continue;

                int j;
                for (j = 0; j < next; j++)
                {
                    if (pattern[j] != pattern[state - i + 1 + j])
                        break;
                }

                if (j == next)
                    return i;
            }

            return 0;
        }
    }
}
