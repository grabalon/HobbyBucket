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
            Quit
        }
        static void Main(string[] args)
        {
#if false
            _family = new Family(new string[] {
                "Linnea",
                "Christopher",
                "Corina",
                "James",
                "Sam",
                "Elizabeth",
                "Josh",
                "Daniel",
                "Kanesa"
            });

            _family.SetExcludedPair("Linnea", "Christopher");
            _family.SetExcludedPair("Corina", "James");
            _family.SetExcludedPair("Sam", "Elizabeth");

            _family.SetPreviousGiver("James", "Sam");
            _family.SetPreviousGiver("Corina", "Linnea");
            _family.SetPreviousGiver("Linnea", "Corina");
            _family.SetPreviousGiver("Christopher", "Daniel");
            _family.SetPreviousGiver("Sam", "Kanesa");
            _family.SetPreviousGiver("Josh", "James");
            _family.SetPreviousGiver("Daniel", "Josh");
            _family.SetPreviousGiver("Kanesa", "Christopher");
#else
            _family = new Family(new string[]
            {
                "Mandy",
                "Easton",
                "Rhett",
                "Levi",
                "Alvin",
                "Glenna"
            });

            _family.SetExcludedPair("Mandy", "Easton");
            _family.SetExcludedPair("Rhett", "Levi");
            _family.SetExcludedPair("Alvin", "Glenna");

            _family.SetPreviousGiver("Alvin", "Levi");
            _family.SetPreviousGiver("Rhett", "Mandy");
            _family.SetPreviousGiver("Levi", "Easton");
            _family.SetPreviousGiver("Mandy", "Alvin");
            _family.SetPreviousGiver("Easton", "Rhett");
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
                }
            }
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
                Console.Write("{0, -12}", participant);
            }

            Console.WriteLine();

            foreach (string giver in matrix.Participants)
            {
                Console.Write("{0, -13}", giver);
                int giverIndex = matrix.GetIndex(giver);

                foreach (string recipient in matrix.Participants)
                {
                    int recipientIndex = matrix.GetIndex(recipient);
                    Console.Write("{0, -12}", matrix.GetWeight(giverIndex, recipientIndex));
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
                default:
                    return Command.Quit;
            }
        }
    }
}
