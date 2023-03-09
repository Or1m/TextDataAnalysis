using System.Collections.Generic;
using System;

namespace ExactPatternMatching
{
    public static class Utils
    {
        private const int MinPatternLength = 5;
        private const int MaxPatternLength = 10;

        public static List<string> GetRandomPatterns(string text, int number)
        {
            Random random = new Random();
            List<string> picks = new List<string>(number);
            int capacity = picks.Capacity;
            int textLength = text.Length;

            while (picks.Count < capacity)
            {
                int length = random.Next(MinPatternLength, MaxPatternLength + 1);
                int idx = random.Next(0, textLength - length);

                picks.Add(text.Substring(idx, length));
            }

            return picks;
        }
        public static List<string> GetRandomWords(string text, int number)
        {
            Random random = new Random();

            List<string> picks = new List<string>(number);
            int capacity = picks.Capacity;

            var words = text.Split(' ');
            var wordsLength = words.Length;

            while (picks.Count < capacity)
            {
                var pick = words[random.Next(wordsLength)];

                if (!picks.Contains(pick))
                    picks.Add(pick);
            }

            return picks;
        }
    }
}
