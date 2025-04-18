using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackjackGame.Engine
{
    public class Deck
    {
        public List<(string, int)> Stack { get; }

        public Deck()
        {
            Stack = new List<(string, int)>{()};
        }
    }
}