using System;
using System.Collections.Generic;

namespace ExactPatternMatching
{
    public class KMPSearchEngine : SearchEngine
    {
        public KMPSearchEngine(string text, List<string> patterns) : base(text, patterns) { }

        protected override void DoSearch(EPrintType printType)
        {
            throw new NotImplementedException();
        }

        protected override List<int> DoSearchAlgorithm(string text, string pattern)
        {
            throw new NotImplementedException();
        }
    }
}
