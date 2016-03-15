using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class LeagueCompetitor : Competitor
    {
        public Position Position { get; set; }

        public int Points { get; set; }
        public int Played { get; set; }

        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
    }
}
