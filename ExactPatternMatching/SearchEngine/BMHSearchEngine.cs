using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ExactPatternMatching
{
    public class BMHSearchEngine : SearchEngine
    {
        private const int alphabetLength = 128;

        readonly List<int[]> badCharTables;

        int[] currentBadCharTable;

        public BMHSearchEngine(string text, List<string> patterns) : base(text, patterns) 
        {
            badCharTables = new List<int[]>(patternsLength);

            for (int i = 0; i < patternsLength; i++)
                badCharTables.Add(CreateBadCharTable(patterns[i]));

            currentBadCharTable = CreateBadCharTable(placeholder);
        }

        protected override void DoSearch(EPrintType printType)
        {
            var watch = Stopwatch.StartNew();

            for (int i = 0; i < patternsLength; i++)
            {
                var pattern = patterns[i];
                currentBadCharTable = badCharTables[i];

                results.Add(pattern, DoSearchAlgorithm(text, pattern));
            }

            watch.Stop();

            Console.WriteLine($"[BMH] Elapsed {watch.ElapsedMilliseconds} ms.\n");
            PrintResults(printType);
        }

        protected override List<int> DoSearchAlgorithm(string text, string pattern)
        {
            var resultIndices = new List<int>();
            int patternLength = pattern.Length;

            //currentBadCharTable = CreateBadCharTable(pattern); // Done in preprocessing phase (constructor)

            int i = 0;
            while (i <= textLength - patternLength)
            {
                int j = patternLength - 1;

                while (j >= 0 && text[i + j] == pattern[j])
                    j--;

                if (j >= 0)
                {
                    char c = text[i + j];

                    i += (c < alphabetLength) ? Math.Max(1, j - currentBadCharTable[c]) : 1;
                    continue;
                }

                resultIndices.Add(i);

                int nextPos = i + patternLength;
                char nextChar = text[nextPos];

                if (nextPos < textLength && nextChar < alphabetLength)
                {
                    i += patternLength - currentBadCharTable[nextChar];
                    continue;
                }

                i++;
            }

            return resultIndices;
        }

        private int[] CreateBadCharTable(string pattern)
        {
            var badCharTable = new int[alphabetLength];
            int patternLength = pattern.Length;

            for (int i = 0; i < alphabetLength; i++)
                badCharTable[i] = -1;

            for (int i = 0; i < patternLength; i++)
                badCharTable[pattern[i]] = i;

            return badCharTable;
        }
    }
}
