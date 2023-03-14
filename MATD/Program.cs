using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ExactPatternMatching
{
    public class Program
    {
        private const string fileName = "english.50MB";
        //private const string fileName = "dna.50MB";
        //private const string fileName = "sample.txt";
        //private const string fileName = "test.txt";

        static void Main()
        {
#if DEBUG
            string text = File.ReadAllText(fileName);
            var patterns = Utils.GetRandomPatterns(text, 10);
#else
            string text = "ABABDABACDABABCABAB";
            var patterns = new List<string>() { "BAB", "AAA", "AAACAAAA", "ABCDE", "AABAACAABAA", "AAACAAAAAC", "AAABAAA"};
#endif

            SearchEngine engine;

            engine = new BruteForceSearchEngine(text, patterns);
            engine.DoSomeMagic(EPrintType.None);

            engine = new DFASearchEngine(text, patterns);
            engine.DoSomeMagic(EPrintType.None);

            engine = new KMPSearchEngine(text, patterns);
            engine.DoSomeMagic(EPrintType.None);

            engine = new BMHSearchEngine(text, patterns);
            engine.DoSomeMagic(EPrintType.None);

            Console.ReadLine();
        }
    }
}
