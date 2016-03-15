using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.League
{
    public class Challenge
    {
        public Match Match { get; set; }

        public Challenge(LeagueCompetitor challenger, LeagueCompetitor defender)
        {
            Match = new Match(challenger.Side, defender.Side);
        }
    }
}
