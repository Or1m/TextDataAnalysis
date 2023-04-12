using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ExactPatternMatching
{
    public class KMPSearchEngine : SearchEngine
    {
        readonly List<int[]> LPSs;
        
        int[] currentLPS;

        public KMPSearchEngine(string text, List<string> patterns) : base(text, patterns) 
        {
            LPSs = new List<int[]>(patternsLength);

            for (int i = 0; i < patternsLength; i++)
                LPSs.Add(CreateLPS(patterns[i]));

            currentLPS = CreateLPS(placeholder);
        }

        protected override void DoSearch(EPrintType printType)
        {
            var watch = Stopwatch.StartNew();

            for (int i = 0; i < patternsLength; i++)
            {
                currentLPS = LPSs[i];
                var pattern = patterns[i];

                results.Add(pattern, DoSearchAlgorithm(text, pattern));
            }

            watch.Stop();

            Console.WriteLine($"[KMP] Elapsed {watch.ElapsedMilliseconds} ms.\n");
            PrintResults(printType);
        }

        protected override List<int> DoSearchAlgorithm(string text, string pattern)
        {
            var patternLength = pattern.Length;
            var resultIndices = new List<int>();

            int i = 0, j = 0;
            while (textLength - i >= patternLength - j)
            {
                if (text[i] == pattern[j])
                {
                    j++;
                    i++;
                }

                if (j == patternLength)
                {
                    resultIndices.Add(i - j);
                    j = currentLPS[j - 1];
                    continue;
                }

                if (i >= textLength || text[i] == pattern[j])
                    continue;

                if (j > 0)
                    j = currentLPS[j - 1];
                else
                    i++;
            }

            return resultIndices;
        }

        private int[] CreateLPS(string pattern)
        {
            var patternLength = pattern.Length;
            var resultLPS = new int[patternLength];

            int l = 0;

            // lps[i] = the longest proper prefix of pat[0..i] which is also a suffix of pat[0..i]. 
            for (int i = 1; i < patternLength; i++)
            {
                if (pattern[l] == pattern[i])
                {
                    resultLPS[i] = ++l;
                    continue;
                }

                if (l > 0)
                {
                    l = resultLPS[l - 1];
                    i--;
                }
            }

            return resultLPS;
        }
    }
}
