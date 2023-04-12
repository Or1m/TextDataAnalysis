#define Similarity

using System.IO;
using System;

namespace VectorModel
{
    public class Program
    {
        private const string inputFolderPath = "\\gutenberg_out\\";

        static void Main()
        {
            var MATDPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            var inputPath = MATDPath + inputFolderPath;

            var model = new Model(inputPath);
            model.Create();

#if Similarity
            //model.FindSimilarDocument(0, out string documentName, out var results_tf, out var results_idf);

            //Console.WriteLine($"Top 10 documents similar to {documentName} according to TF:\n");
            //foreach (var (fileName, score) in results_tf)
            //    Console.WriteLine($"{fileName,-12} ({score:0.00})");

            //Console.WriteLine(new string('-', 20));
            //Console.WriteLine();

            //Console.WriteLine($"Top 10 documents similar to {documentName} according to TF_IDF:\n");
            //foreach (var (fileName, score) in results_idf)
            //    Console.WriteLine($"{fileName,-12} ({score:0.00})");

            //Console.WriteLine(new string('-', 20));
            //Console.WriteLine();

            var results = model.FindSimilarWords();
            foreach (var (first, second, similarity) in results)
                Console.WriteLine($"{first} -> {second} ({similarity})");
#else
            var results = model.Search("god"); 
            //var results = model.Search("Names of the Persons voting for and against the Bill shall be entered on the Journal of each House respectively");

            Console.WriteLine("Top 10 documents which contains all provided words:\n");

            foreach (var (fileName, score) in results)
                Console.WriteLine($"{fileName, -12} ({score:00.000})");
#endif
            Console.ReadLine();
        }
    }
}
