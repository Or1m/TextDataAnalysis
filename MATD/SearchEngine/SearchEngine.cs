using System;
using System.Collections.Generic;

namespace ExactPatternMatching
{
    public abstract partial class SearchEngine
    {
        protected const string placeholder = "PLACEHOLDER";

        protected string text;
        protected List<string> patterns;

        protected int textLength;
        protected int patternsLength;

        protected Dictionary<string, List<int>> results;


        public SearchEngine(string text, List<string> patterns)
        {
            this.text = text;
            this.patterns = patterns;

            textLength = text.Length;
            patternsLength = patterns.Count;

            results = new Dictionary<string, List<int>>();
        }

        public void DoSomeMagic(EPrintType printType = EPrintType.First)
        {
            DoSearchAlgorithm(text, placeholder); // Filter 1. call of function which takes longer time
            DoSearch(printType);
        }

        protected void PrintResults(EPrintType printType)
        {
            if (printType == EPrintType.None)
                return;

            foreach (var result in results)
            {
                Console.WriteLine($"Pattern: \"{result.Key}\":");

                if (printType == EPrintType.CountOnly)
                {
                    Console.WriteLine($"Number of occurrences: {result.Value.Count}");
                    continue;
                }

                foreach (var idx in result.Value)
                {
                    Console.Write($"{idx}, ");

                    if (printType == EPrintType.First)
                        break;
                }
            }

            Console.WriteLine(new string('-', 32));
            Console.WriteLine();
        }

        
        protected abstract void DoSearch(EPrintType printType);
        protected abstract List<int> DoSearchAlgorithm(string text, string pattern);
    }
}
