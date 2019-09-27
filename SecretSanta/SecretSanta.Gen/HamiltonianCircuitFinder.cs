using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Gen
{
    public class HamiltonianCircuitFinder
    {
        public const int MaxWeight = 500;
        public const int PreferredPath = -10;

        public static GiftMatrix DoTravelingSalesman(GiftMatrix thawedMatrix)
        {
            int[,] bestSolutionSoFar = null;
            int bestCostSoFar = MaxWeight - 1;
            var partialSolutions = new Stack<int[,]>();

            var empty = new int[thawedMatrix.NumberOfParticipants, thawedMatrix.NumberOfParticipants];

            for (int i = 1; i < thawedMatrix.NumberOfParticipants; i++)
            {
                var starter = Clone(empty);
                starter[0, i] = 1;
                partialSolutions.Push(starter);
            }

            while (partialSolutions.Count > 0)
            {
                var next = partialSolutions.Pop();
                var cost = Cost(next, thawedMatrix);
                if (cost < bestCostSoFar)
                {
                    if (IsTotalSolution(next))
                    {
                        bestCostSoFar = cost;
                        bestSolutionSoFar = next;
                    }
                    else
                    {
                        GenerateNextPartialSolutions(next, partialSolutions);
                    }
                }
            }

            return new GiftMatrix(thawedMatrix.Participants, bestSolutionSoFar);
        }

        private static void GenerateNextPartialSolutions(int[,] next, Stack<int[,]> partialSolutions)
        {
            // Look for a reciever that's not also a giver
            int newGiver = -1;
            for (int i = 0; newGiver == -1 && i < next.GetLength(0); i++)
            {
                for (int j = 0; newGiver == -1 && j < next.GetLength(0); j++)
                {
                    if (next[i, j] != 0)
                    {
                        bool doTheyGive = false;

                        // Found a reciever, j, now look to see if they are in turn a giver
                        for (int k = 0; k < next.GetLength(0); k++)
                        {
                            if (next[j, k] != 0)
                            {
                                doTheyGive = true;
                            }
                        }

                        if (!doTheyGive)
                        {
                            newGiver = j;
                        }
                    }
                }
            }

            if (newGiver == -1)
            {
                return;
            }

            // Ok, our new giver is foundOn

            // For each possible next receiver (column)
            for (int i = 0; i < next.GetLength(0); i++)
            {
                // Don't go from yourself to yourself
                if (i == newGiver)
                {
                    continue;
                }

                // Look above the current i to see if it's already in the path
                bool alreadyHit = false;
                for (int j = 0; j < next.GetLength(0); j++)
                {
                    if (next[j, i] != 0)
                    {
                        alreadyHit = true;
                    }
                }

                if (alreadyHit)
                {
                    continue;
                }

                // If you're not the last giver, also look at the receiver to make sure they're not already giving, which makes not a full cycle
                int numGivers = 0;
                for (int j = 0; j < next.GetLength(0); j++)
                {
                    for (int k = 0; k < next.GetLength(0); k++)
                    {
                        numGivers += next[j, k];
                    }
                }

                if (numGivers != next.GetLength(0) - 1)
                {
                    for (int j = 0; j < next.GetLength(0); j++)
                    {
                        if (next[i, j] != 0)
                        {
                            alreadyHit = true;
                        }
                    }

                    if (alreadyHit)
                    {
                        continue;
                    }
                }

                int[,] clone = Clone(next);

                clone[newGiver, i] = 1;
                partialSolutions.Push(clone);
            }
        }

        private static int[,] Clone(int[,] next)
        {
            // Ok, we have a new potential solution at [foundOn, i], clone the existing one and add the value
            var clone = new int[next.GetLength(0), next.GetLength(0)];
            for (int row = 0; row < next.GetLength(0); row++)
            {
                for (int col = 0; col < next.GetLength(0); col++)
                {
                    clone[row, col] = next[row, col];
                }
            }

            return clone;
        }

        private static bool IsTotalSolution(int[,] next)
        {
            for (int i = 0; i < next.GetLength(0); i++)
            {
                bool found = false;
                for (int j = 0; j < next.GetLength(0); j++)
                {
                    if (next[i, j] != 0)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }

            return true;
        }

        private static int Cost(int[,] next, GiftMatrix thawedMatrix)
        {
            int partialCost = 0;
            for (int i = 0; i < thawedMatrix.NumberOfParticipants; i++)
            {
                for (int j = 0; j < thawedMatrix.NumberOfParticipants; j++)
                {
                    partialCost += next[i, j] * thawedMatrix.GetWeight(i, j);
                }
            }

            return partialCost;
        }
    }
}
