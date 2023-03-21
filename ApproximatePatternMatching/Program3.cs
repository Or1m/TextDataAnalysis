//using System;
//using System.Collections.Generic;

//class NFAMatch
//{
//    private readonly int[,] transitionTable;
//    private readonly int patternLength;

//    public NFAMatch(string pattern, int k)
//    {
//        patternLength = pattern.Length;
//        var stateCount = patternLength + 1;

//        // Initialize the transition table
//        transitionTable = new int[stateCount, 256];
//        for (int i = 0; i < stateCount; i++)
//        {
//            for (int j = 0; j < 256; j++)
//            {
//                transitionTable[i, j] = -1;
//            }
//        }

//        // Fill in the transition table
//        for (int i = 0; i < patternLength; i++)
//        {
//            transitionTable[i, pattern[i]] = i + 1;
//        }

//        // Fill in the k-error transitions
//        for (int i = 0; i < patternLength; i++)
//        {
//            for (int j = 0; j < 256; j++)
//            {
//                if (transitionTable[i, j] != -1)
//                {
//                    for (int e = 1; e <= k; e++)
//                    {
//                        if (i + e < patternLength)
//                        {
//                            transitionTable[i, j] = i + e + 1;
//                        }
//                    }
//                }
//            }
//        }
//    }

//    public bool IsMatch(string text)
//    {
//        var currentStates = new HashSet<int>();
//        currentStates.Add(0);

//        foreach (char c in text)
//        {
//            var nextStates = new HashSet<int>();
//            foreach (int state in currentStates)
//            {
//                if (transitionTable[state, c] != -1)
//                {
//                    nextStates.Add(transitionTable[state, c]);
//                }
//            }

//            for (int e = 1; e <= k; e++)
//            {
//                var errorStates = new HashSet<int>();
//                foreach (int state in currentStates)
//                {
//                    for (int i = 0; i < 256; i++)
//                    {
//                        if (transitionTable[state, i] != -1)
//                        {
//                            errorStates.Add(transitionTable[state, i]);
//                        }
//                    }
//                }
//                currentStates.UnionWith(errorStates);
//            }

//            currentStates = nextStates;
//        }

//        return currentStates.Contains(patternLength);
//    }
//}