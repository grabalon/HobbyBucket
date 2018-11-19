using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Gen
{
    public class GiftMatrix
    {
        int _numParticipants;
        private string[] _participants;
        private int[,] _costs;
        private bool _frozen = false;

        public GiftMatrix(string [] participants, int [,] costs)
        {
            _numParticipants = participants.Length;
            if (costs.GetLength(0) != costs.GetLength(1) || costs.GetLength(0) != _numParticipants)
            {
                throw new ArgumentException("Need costs for each giver and each recipient", nameof(costs));
            }
            _participants = participants;
            _costs = costs;
        }

        public int NumberOfParticipants
        {
            get
            {
                return _numParticipants;
            }
        }

        public GiftMatrix Clone()
        {
            var participants = new string[_numParticipants];
            var costs = new int[_numParticipants, _numParticipants];

            for (int i = 0; i < _numParticipants; i++)
            {
                participants[i] = _participants[i];
                for (int j = 0; j < _numParticipants; j++)
                {
                    costs[i, j] = _costs[i, j];
                }
            }

            return new GiftMatrix(participants, costs);
        }

        public static GiftMatrix operator +(GiftMatrix left, GiftMatrix right)
        {
            if (left._numParticipants != right._numParticipants)
            {
                throw new ArgumentException("Adding two differently sized groups together");
            }

            int[,] newCosts = new int[left._numParticipants, left._numParticipants];
            for (int i = 0; i < left._numParticipants; i++)
            {
                for (int j = 0; j < left._numParticipants; j++)
                {
                    newCosts[i, j] = left._costs[i, j] + right._costs[i, j];
                }
            }

            return new GiftMatrix(left._participants, newCosts);
        }

        public string[] Participants
        {
            get
            {
                return _participants;
            }
        }

        public int GetIndex(string participant)
        {
            for (int i = 0; i < _participants.Length; i++)
            {
                if (participant == _participants[i])
                {
                    return i;
                }
            }
            throw new ArgumentOutOfRangeException(nameof(participant), participant, "Participant not found");
        }

        public int GetWeight(int giverIndex, int recipientIndex)
        {
            return _costs[giverIndex, recipientIndex];
        }

        public void SetWeight(int giverIndex, int recipientIndex, int weight)
        {
            if (!_frozen)
            {
                _costs[giverIndex, recipientIndex] = weight;
            }
            else
            {
                throw new InvalidOperationException("Cannot set weight on a frozen matrix");
            }
        }

        public void Freeze()
        {
            _frozen = true;
        }
    }
}
