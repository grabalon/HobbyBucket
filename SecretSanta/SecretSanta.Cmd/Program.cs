using SecretSanta.Gen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Cmd
{
    class Program
    {
        private static Family _family;
        private static GiftMatrix _lastHamiltonean;

        private enum Command
        {
            GenerateHamiltonean,
            PrintWeights,
            Quit,
            Usage
        }
        static void Main(string[] args)
        {
#if true
            _family = new Family(new string[] {
                "Linnea",
                "Christopher",
                "Corina",
                "James",
                "Sam",
                "Elizabeth",
                "Josh",
                "Daniel",
                "Kanesa",
                "Austin"
            });

            _family.SetExcludedPair("Linnea", "Christopher");
            _family.SetExcludedPair("Corina", "James");
            _family.SetExcludedPair("Sam", "Elizabeth");
            _family.SetExcludedPair("Kanesa", "Austin");

            // 2017
            _family.SetPreviousGiver("James", "Sam");
            _family.SetPreviousGiver("Corina", "Linnea");
            _family.SetPreviousGiver("Linnea", "Corina");
            _family.SetPreviousGiver("Christopher", "Daniel");
            _family.SetPreviousGiver("Sam", "Kanesa");
            _family.SetPreviousGiver("Josh", "James");
            _family.SetPreviousGiver("Daniel", "Josh");
            _family.SetPreviousGiver("Kanesa", "Christopher");

            // 2018
            _family.SetPreviousGiver("Linnea", "Kanesa");
            _family.SetPreviousGiver("Christopher", "James");
            _family.SetPreviousGiver("Corina", "Christopher");
            _family.SetPreviousGiver("James", "Linnea");
            _family.SetPreviousGiver("Sam", "Corina");
            _family.SetPreviousGiver("Elizabeth", "Josh");
            _family.SetPreviousGiver("Josh", "Sam");
            _family.SetPreviousGiver("Daniel", "Elizabeth");
            _family.SetPreviousGiver("Kanesa", "Daniel");
#else
            _family = new Family(new string[]
            {
                "Mandy",
                "Easton",
                "Rhett",
                "Levi",
                "Tucker",
                "Alvin",
                "Glenna",
                "Meridian"
            });

            _family.SetExcludedPair("Mandy", "Easton");
            _family.SetExcludedPair("Rhett", "Levi");
            _family.SetExcludedPair("Rhett", "Tucker");
            _family.SetExcludedPair("Tucker", "Levi");
            _family.SetExcludedPair("Alvin", "Glenna");

            // 2017
            _family.SetPreviousGiver("Alvin", "Levi");
            _family.SetPreviousGiver("Rhett", "Mandy");
            _family.SetPreviousGiver("Levi", "Easton");
            _family.SetPreviousGiver("Mandy", "Alvin");
            _family.SetPreviousGiver("Easton", "Rhett");

            //2018
            _family.SetPreviousGiver("Mandy", "Glenna");
            _family.SetPreviousGiver("Easton", "Levi");
            _family.SetPreviousGiver("Rhett", "Alvin");
            _family.SetPreviousGiver("Levi", "Mandy");
            _family.SetPreviousGiver("Alvin", "Easton");
            _family.SetPreviousGiver("Glenna", "Rhett");
#endif
            Command c;
            while ((c = ReadCommand()) != Command.Quit)
            {
                switch (c)
                {
                    case Command.PrintWeights:
                        PrintWeights(_family.ThawedMatrix);
                        break;
                    case Command.GenerateHamiltonean:
                        GenerateHamiltonean();
                        break;
                    case Command.Usage:
                        PrintUsage();
                        break;
                }
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine(@"USAGE: (p, g, q)
p - Print current weights
g - Generate new hamiltonean circuit
q - quit");
        }

        private static void GenerateHamiltonean()
        {
            _lastHamiltonean = HamiltonianCircuitFinder.DoTravelingSalesman(_family.ThawedMatrix);
            PrintWeights(_lastHamiltonean);
            _family.ThawedMatrix = _family.ThawedMatrix + _lastHamiltonean;
        }

        private static void PrintWeights(GiftMatrix matrix)
        {
            Console.Write("\\/gives  to> ");
            foreach (string participant in matrix.Participants)
            {
                Console.Write("{0, -4}", participant.Substring(0, 3));
            }

            Console.WriteLine();

            foreach (string giver in matrix.Participants)
            {
                Console.Write("{0, -13}", giver);
                int giverIndex = matrix.GetIndex(giver);

                foreach (string recipient in matrix.Participants)
                {
                    int recipientIndex = matrix.GetIndex(recipient);
                    Console.Write("{0, -4}", matrix.GetWeight(giverIndex, recipientIndex));
                }

                Console.WriteLine();
            }
        }

        private static Command ReadCommand()
        {
            Console.Write("> ");
            string typed = Console.ReadLine();

            switch (typed)
            {
                case "p":
                case "P":
                case "print":
                    return Command.PrintWeights;
                case "g":
                    return Command.GenerateHamiltonean;
                case "q":
                    return Command.Quit;
                default:
                    return Command.Usage;
            }
        }
    }
}
