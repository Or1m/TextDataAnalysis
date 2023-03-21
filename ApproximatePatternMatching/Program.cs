//using System;
//using System.Collections.Generic;
//using System.Linq;

//class NFA
//{
//    private readonly int[,] transitions;
//    private readonly int startState;
//    private readonly int[] finalStates;
//    private readonly int alphabetSize;

//    public NFA(int[,] transitions, int startState, int[] finalStates, int alphabetSize)
//    {
//        this.transitions = transitions;
//        this.startState = startState;
//        this.finalStates = finalStates;
//        this.alphabetSize = alphabetSize;
//    }

//    public IEnumerable<int> GetMatches(string input, int maxErrors)
//    {
//        HashSet<int> currentStates = new HashSet<int>();
//        HashSet<int> nextStates = new HashSet<int>();

//        currentStates.Add(startState);

//        for (int i = 0; i < input.Length; i++)
//        {
//            foreach (int state in currentStates)
//            {
//                for (int j = 0; j < alphabetSize; j++)
//                {
//                    if (transitions[state, j] != -1)
//                    {
//                        nextStates.Add(transitions[state, j]);
//                    }
//                }
//            }

//            currentStates = nextStates;
//            nextStates = new HashSet<int>();

//            if (currentStates.Count == 0)
//            {
//                break;
//            }
//        }

//        for (int i = 0; i <= maxErrors; i++)
//        {
//            foreach (int state in currentStates)
//            {
//                if (finalStates.Contains(state))
//                {
//                    yield return i;
//                }
//            }

//            HashSet<int> newCurrentStates = new HashSet<int>();

//            foreach (int state in currentStates)
//            {
//                for (int j = 0; j < alphabetSize; j++)
//                {
//                    if (transitions[state, j] != -1)
//                    {
//                        newCurrentStates.Add(transitions[state, j]);
//                    }
//                }
//            }

//            currentStates = newCurrentStates;
//        }
//    }
//}

//class Program
//{
//    static void Main()
//    {
//        int[,] transitions = new int[,] { { 1, -1, -1 }, { 2, 1, -1 }, { 3, -1, 1 }, { 3, 3, 3 } };
//        int startState = 0;
//        int[] finalStates = new int[] { 3 };
//        int alphabetSize = 3;

//        NFA nfa = new NFA(transitions, startState, finalStates, alphabetSize);

//        string input = "survey";
//        int maxErrors = 2;

//        IEnumerable<int> matches = nfa.GetMatches(input, maxErrors);

//        foreach (int match in matches)
//        {
//            Console.WriteLine("Match found with {0} errors.", match);
//        }

//        Console.ReadLine();
//    }
//}