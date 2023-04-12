using Poseidon.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InvertedIndex
{
    public class Index
    {
        private readonly string[] stopWords = new string[]
        {
            "a", "an", "and", "are", "as", "at", "be", "by", "for", "from",
            "has", "he", "in", "is", "it", "its", "of", "on", "that", "the",
            "to", "was", "were", "will", "with"
        };

        private readonly string inputPath;
        private readonly string[] inputFileNames;

        private readonly Dictionary<string, SortedSet<int>> resultIndex = new Dictionary<string, SortedSet<int>>();

        public Index(string inputPath)
        {
            this.inputPath = inputPath;

            inputFileNames = Directory.GetFiles(inputPath)
                .Select(x => Path.GetFileName(x))
                .ToArray();
        }

        public void Create()
        {
            var length = inputFileNames.Length;
            for (int i = 0; i < length; i++)
            {
                CreateFromFile(inputFileNames[i], i);
            }

            Console.WriteLine("Index Created");
            Console.WriteLine(new string('-', 13));
            Console.WriteLine();
        }
        private void CreateFromFile(string inputFileName, int fileIdx)
        {
            var text = File.ReadAllText(inputPath + inputFileName).Split(' ');

            int length = text.Length;
            for (int i = 0; i < length; i++)
            {
                string word = text[i];

                if (!resultIndex.ContainsKey(word))
                    resultIndex[word] = new SortedSet<int>();

                resultIndex[word].Add(fileIdx);
            }
        }

        public string[] Search(string pattern)
        {
            string[] words = GetStemmedWordsFromPattern(pattern);

            if (!resultIndex.TryGetValue(words[0], out var files))
                return null;

            int wordsLength = words.Length;
            for (int i = 1; i < wordsLength; i++)
            {
                if (!resultIndex.TryGetValue(words[i], out var newFiles))
                    return null;

                files.IntersectWith(newFiles);
            }

            return files
                .Select(x => inputFileNames[x])
                .ToArray();
        }
        private string[] GetStemmedWordsFromPattern(string pattern)
        {
            var stemmer = new PorterStemmer();

            return pattern
                .ToLower()
                .Split(' ')
                .Except(stopWords)
                .Select(x => stemmer.StemWord(x))
                .ToArray();
        }
    }
}
