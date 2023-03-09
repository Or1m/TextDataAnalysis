using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ExactPatternMatching
{
    public class BruteForceSearchEngine : SearchEngine
    {
        public BruteForceSearchEngine(string text, List<string> patterns) : base(text, patterns) { }

        protected override void DoSearch(EPrintType printType)
        {
            var watch = Stopwatch.StartNew();

            for (int i = 0; i < patternsLength; i++)
            {
                var pattern = patterns[i];
                results.Add(pattern, DoSearchAlgorithm(text, pattern));
            }

            watch.Stop();

            Console.WriteLine($"[Brute Force] Elapsed {watch.ElapsedMilliseconds} ms\n");
            PrintResults(printType);
        }
        protected override List<int> DoSearchAlgorithm(string text, string pattern)
        {
            var patternLength = pattern.Length;
            var resultIndices = new List<int>();

            for (int i = 0; i <= textLength - patternLength; i++)
            {
                int j;

                for (j = 0; j < patternLength; j++)
                {
                    if (text[i + j] != pattern[j])
                        break;
                }

                if (j == patternLength)
                    resultIndices.Add(i);
            }

            return resultIndices;
        }
    }
}
