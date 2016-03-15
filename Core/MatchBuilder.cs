using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class MatchBuilder
    {

        public void NewMatch(Competition competition, Side sideA, Side sideB)
        {
            Match match = new Match(sideA, sideB);
            competition.Matches.Add(match);
        }
    }
}
