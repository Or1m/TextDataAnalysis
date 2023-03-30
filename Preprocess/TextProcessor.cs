using Poseidon.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Preprocess
{
    public class TextProcessor
    {
        private readonly string[] stopWords = new string[]
        {
            "a", "an", "and", "are", "as", "at", "be", "by", "for", "from",
            "has", "he", "in", "is", "it", "its", "of", "on", "that", "the",
            "to", "was", "were", "will", "with"
        };

        private readonly string inputPath;
        private readonly string outputPath;

        private readonly string[] inputFileNames;

        public TextProcessor(string inputPath, string outputPath)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;

            var inputFiles = Directory.GetFiles(inputPath);
            inputFileNames = inputFiles.Select(x => Path.GetFileName(x)).ToArray();
        }

        public void Process(bool verbose = false)
        {
            var length = inputFileNames.Length;
            for (int i = 0; i < length; i++)
            {
                ProcessFile(inputFileNames[i], verbose);
            }

            Console.WriteLine("Done");
        }

        private void ProcessFile(string fileName, bool verbose)
        {
            var fileBody = File.ReadAllText(inputPath + fileName);
            var words = GetLowerAlphanumWords(fileBody);

            RemoveStopWords(words, out var filteredWords);
            ApplyPorterStemmer(filteredWords, verbose);

            var result = string.Join(" ", filteredWords);
            if (verbose)
            {
                Console.WriteLine(result);
                Console.ReadLine();
            }

            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            File.WriteAllText(outputPath + fileName, result);
            Console.WriteLine($"{fileName} processed");
        }

        private List<string> GetLowerAlphanumWords(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9 ']", " ")
                .ToLower()
                .Split(' ')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
        }
        private void RemoveStopWords(List<string> words, out List<string> filteredWords)
        {
            var length = words.Count;
            filteredWords = new List<string>(3/4 * length);

            for (int i = 0; i < length; i++)
            {
                var word = words[i];

                if (stopWords.Contains(word))
                    continue;

                //Optional
                var apostropheIdx = word.IndexOf('\'');
                if (apostropheIdx >= 0)
                    word = word.Remove(apostropheIdx, 1);

                filteredWords.Add(word);
            }
        }
        private void ApplyPorterStemmer(List<string> filteredWords, bool verbose = false)
        {
            PorterStemmer stemmer = new PorterStemmer();
            var length = filteredWords.Count;

            for (int i = 0; i < length; i++)
            {
                var word = filteredWords[i];
                filteredWords[i] = stemmer.StemWord(word);

                if (verbose)
                    Console.WriteLine($"{word} -> {filteredWords[i]}");
            }
        }
    }
}
