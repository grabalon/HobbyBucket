using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Gen
{
    public class Family
    {
        private GiftMatrix _thawedMatrix;

        public Family(string[] participants)
        {
            int numParticipants = participants.Length;
            var costs = new int[numParticipants, numParticipants];

            for (int i = 0; i < numParticipants; i++)
            {
                costs[i, i] = HamiltonianCircuitFinder.MaxWeight;
            }

            _thawedMatrix = new GiftMatrix(participants, costs);
        }

        public void SetExcludedPair(string participant1, string participant2)
        {
            int index1 = _thawedMatrix.GetIndex(participant1);
            int index2 = _thawedMatrix.GetIndex(participant2);

            _thawedMatrix.SetWeight(index1, index2, HamiltonianCircuitFinder.MaxWeight);
            _thawedMatrix.SetWeight(index2, index1, HamiltonianCircuitFinder.MaxWeight);
        }

        public void SetPreferredPath(string giver, string recipient)
        {
            int giverIndex = _thawedMatrix.GetIndex(giver);
            int recipientIndex = _thawedMatrix.GetIndex(recipient);
            _thawedMatrix.SetWeight(giverIndex, recipientIndex, HamiltonianCircuitFinder.PreferredPath);
        }

        public void SetPreviousGiver(string giver, string recipient)
        {
            int giverIndex = _thawedMatrix.GetIndex(giver);
            int recipientIndex = _thawedMatrix.GetIndex(recipient);
            int prevWeight = _thawedMatrix.GetWeight(giverIndex, recipientIndex);
            _thawedMatrix.SetWeight(giverIndex, recipientIndex, prevWeight + 1);
        }

        public GiftMatrix ThawedMatrix
        {
            get
            {
                return _thawedMatrix;
            }
            set
            {
                _thawedMatrix = value;
            }
        }
    }
}
