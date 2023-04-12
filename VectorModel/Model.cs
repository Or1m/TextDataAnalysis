using Poseidon.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace VectorModel
{
    public class Model
    {
        private static readonly string[] stopWords = new string[]
        {
            "a", "an", "and", "are", "as", "at", "be", "by", "for", "from",
            "has", "he", "in", "is", "it", "its", "of", "on", "that", "the",
            "to", "was", "were", "will", "with"
        };

        private readonly string inputPath;
        private readonly string[] inputFileNames;

        private readonly Dictionary<string, Dictionary<int, int>> termFrequency_td = new Dictionary<string, Dictionary<int, int>>();

        public Model(string inputPath)
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
                CreateFromFile(inputFileNames[i], i);

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

                if (!termFrequency_td.ContainsKey(word))
                    termFrequency_td[word] = new Dictionary<int, int>();

                var filesFreq = termFrequency_td[word];

                if (!filesFreq.ContainsKey(fileIdx))
                    filesFreq.Add(fileIdx, 0);
                else
                    filesFreq[fileIdx]++;
            }
        }

        #region Searching by score
        public List<(string fileName, double score)> Search(string pattern)
        {
            var words = GetStemmedWordsFromPattern(pattern);
            var results = new List<(string fileName, double score)>();

            var numOfFiles = inputFileNames.Length;

            for (int i = 0; i < numOfFiles; i++)
                results.Add((inputFileNames[i], CalcScore(words, i)));

            return results
                .OrderByDescending(x => x.score)
                .Take(10)
                .ToList();
        }

        /// <summary>
        /// inverse document frequency idf_t = log(N/df_t),
        /// kde N je počet dokumentů v kolekci a 'df_t' je počet dokumentů obsahujích term 't'.
        /// </summary>
        private double CalcInverseDocumentFrequency_t(string term)
        {
            int df_t = termFrequency_td[term].Count();
            double ratio = inputFileNames.Length / (double)df_t;

            return Math.Log10(ratio);
        }
        /// <summary>
        /// Spočítejte tf-idf váhy: tf-idf_t_d = tf_t_d * idf_t
        /// </summary>
        private double CalcTermFrequency_idf_td(string term, int document)
        {
            if (!termFrequency_td.ContainsKey(term) || !termFrequency_td[term].ContainsKey(document))
                return 0;

            int termFrequencyInDoc = termFrequency_td[term][document];
            double inverseDocFrequency = CalcInverseDocumentFrequency_t(term);

            return termFrequencyInDoc * inverseDocFrequency;
        }
        /// <summary>
        /// Spočítejte skóre pro termy v dotazu q: Score(q,d) = sum_t_in_q tf-idf_t_d
        /// </summary>
        private double CalcScore(string[] terms, int document)
        {
            var termsLength = terms.Length;
            double result = 0;

            for (int i = 0; i < termsLength; i++)
            {
                result += CalcTermFrequency_idf_td(terms[i], document);
            }

            return result;
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
        #endregion

        #region Searching by similarity
        public void FindSimilarDocument(int documentIdx, out string name, 
            out List<(string fileName, double dotProduct)> results_tf,
            out List<(string fileName, double dotProduct)> results_idf)
        {
            name = inputFileNames[documentIdx];

            int rows = termFrequency_td.Count;
            int cols = inputFileNames.Length;

            CreateMatrix_tf(rows, cols, out var matrix_tf);
            NormalizeCols(matrix_tf, rows, cols);
            results_tf = GetTopWithNames(10, GetSimilarities(matrix_tf, rows, cols, documentIdx));

            CreateMatrix_idf(rows, cols, out var matrix_idf);
            NormalizeCols(matrix_idf, rows, cols);
            results_idf = GetTopWithNames(10, GetSimilarities(matrix_idf, rows, cols, documentIdx)); ;
        }
        public List<(string first, string second, int similarity)> FindSimilarWords()
        {
            int rows = termFrequency_td.Count;
            int cols = inputFileNames.Length;

            foreach (var wordRow in termFrequency_td)
            {

                foreach (var fileCount in wordRow.Value)
                {

                }
            }



            return null;
        }
        

        private List<(int document, double dot)> GetSimilarities(double[][] matrix, int rows, int cols, int documentIdx)
        {
            var pivot = GetColumn(matrix, documentIdx, rows);
            var results = new List<(int document, double dot)>();

            for (int j = 0; j < cols; j++)
            {
                if (j == documentIdx)
                    continue;

                results.Add((j, CalcDotPoduct(pivot, GetColumn(matrix, j, rows))));
            }

            return results;
        }

        private void CreateMatrix_tf(int rows, int cols, out double[][] matrix)
        {
            matrix = new double[rows][];
            
            for (int i = 0; i < rows; i++)
                matrix[i] = new double[cols];

            int rowIdx = 0;
            foreach (var word in termFrequency_td)
            {
                int colIdx = 0;
                foreach (var fileCounter in word.Value)
                {
                    while (colIdx < fileCounter.Key)
                        matrix[rowIdx][colIdx++] = 0;

                    matrix[rowIdx][colIdx++] = fileCounter.Value;
                }

                rowIdx++;
            }
        }
        private void CreateMatrix_idf(int rows, int cols, out double[][] matrix)
        {
            matrix = new double[rows][];

            for (int i = 0; i < rows; i++)
                matrix[i] = new double[cols];

            int rowIdx = 0;
            foreach (var word in termFrequency_td)
            {
                int colIdx = 0; //
                foreach (var fileCounter in word.Value)
                {
                    int docIdx = fileCounter.Key;

                    while (colIdx < docIdx)
                        matrix[rowIdx][colIdx++] = 0;

                    matrix[rowIdx][colIdx++] = CalcTermFrequency_idf_td(word.Key, docIdx);
                }

                rowIdx++;
            }
        }

        private void NormalizeCols(double[][] matrix, int rows, int cols)
        {
            for (int j = 0; j < cols; j++)
            {
                int i = 0;
                var length = CalcMagnitude(GetColumn(matrix, j, rows));

                for (; i < rows; i++)
                    matrix[i][j] /= length;
            }
        }
        private void NormalizeRows(double[][] matrix, int rows, int cols)
        {
            for (int i = 0; i < rows; i++)
            {
                double length = CalcMagnitude(matrix[i]);

                for (int j = 0; j < cols; j++)
                    matrix[i][j] /= length;
            }
        }

        public double CalcDotPoduct(double[] a, double[] b)
        {
            var length = a.Length;
            double result = 0;

            for (int i = 0; i < length; i++)
                result += a[i] * b[i];

            return result;
        }
        private double CalcMagnitude(IEnumerable<double> vector)
        {
            return Math.Sqrt(vector.Sum(v => v * v));
        }
        
        private double[] GetColumn(double[][] matrix, int idx, int rows)
        {
            double[] result = new double[rows];

            for (int i = 0; i < rows; i++)
                result[i] = matrix[i][idx];

            return result;
        }
        private List<(string fileName, double dotProduct)> GetTopWithNames(int number, List<(int document, double dot)> list)
        {
            return list
                .OrderByDescending(x => x.dot)
                .Select(x => (inputFileNames[x.document], x.dot))
                .Take(number)
                .ToList();
        }
        #endregion
    }
}
