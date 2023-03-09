using System;
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
            string text = File.ReadAllText(fileName);
            var patterns = Utils.GetRandomPatterns(text, 10);

            SearchEngine engine;

            engine = new BruteForceSearchEngine(text, patterns);
            engine.DoSomeMagic(EPrintType.None);

            engine = new DFASearchEngine(text, patterns);
            engine.DoSomeMagic(EPrintType.None);

            //engine = new KMPSearchEngine(text, patterns);
            //engine.DoSomeMagic(EPrintType.None);

            //engine = new BMHSearchEngine(text, patterns);
            //engine.DoSomeMagic(EPrintType.None);

            Console.ReadLine();
        }
    }
}
