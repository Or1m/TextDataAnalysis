using System.IO;
using System;

namespace InvertedIndex
{
    public class Program
    {
        private const string inputFolderPath = "\\gutenberg_out\\";

        static void Main()
        {

            var MATDPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

            var inputPath = MATDPath + inputFolderPath;

            var index = new Index(inputPath);
            index.Create();

            var results = index.Search("Names of the Persons voting for and against the Bill shall be entered on the Journal of each House respectively");
            if (results == null)
                Console.WriteLine("Match not found in any file");
            else
            {
                Console.WriteLine($"All your words are contained in theese {results.Length} files:\n");

                foreach (var result in results)
                    Console.WriteLine(result);
            }

            Console.ReadLine();
        }
    }
}
