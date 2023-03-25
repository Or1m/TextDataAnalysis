using System;
using System.Collections.Generic;

namespace ApproximatePatternMatching
{
    public class Program
    {
        public static void Main()
        {
            const int maxErrors = 2;
            
            string[] patterns = new string[] { "survey", "survex", "lurvex", "lurvix" };
            string text = "survey";

            var results = new List<(string pattern, int errors)>();
            
            for (int i = 0; i < patterns.Length; i++)
            {
                var pattern = patterns[i];
                int numOfStates = (pattern.Length + 1) * (maxErrors + 1);
                
                var activeStates = new Queue<(int stateIdx, int patternIdx)>();
                activeStates.Enqueue((1, 0));

                while (activeStates.Count > 0)
                {
                    var activeState = activeStates.Dequeue();

                    int stateNumber = activeState.stateIdx;
                    int patternIdx = activeState.patternIdx;

                    if (stateNumber % (pattern.Length + 1) == 0)
                    {
                        results.Add((pattern, (stateNumber / (pattern.Length + 1)) - 1));
                        break;
                    }

                    if ((patternIdx < pattern.Length) && (text[(stateNumber - 1) % (pattern.Length + 1)] == pattern[patternIdx]))
                        activeStates.Enqueue((stateNumber + 1, patternIdx + 1));

                    var statePlusLength = stateNumber + pattern.Length;

                    if (statePlusLength + 1 <= numOfStates)
                    {
                        activeStates.Enqueue((statePlusLength + 1, patternIdx));
                        activeStates.Enqueue((statePlusLength + 1, patternIdx + 1));
                    }

                    if (statePlusLength + 2 <= numOfStates)
                    {
                        activeStates.Enqueue((statePlusLength + 2, patternIdx));
                        activeStates.Enqueue((statePlusLength + 2, patternIdx + 1));
                    } 
                }
            }

            Console.WriteLine(text);
            Console.WriteLine();
            results.ForEach(x => Console.WriteLine($"{x.pattern} [{x.errors}]"));

            Console.ReadLine();
        }
    }
}