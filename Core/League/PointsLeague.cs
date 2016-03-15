using Core.LeagueRules;
using Core.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.League
{
    public class PointsLeague : League
    {
        public List<MatchDay> MatchDays { get; set; }

        public PointsLeague(int pointsForWin, int pointsForDraw, int pointsForLoss, int numberOfPositions) : base(numberOfPositions)
	    {
            leagueRules = new PointsLeagueRules(pointsForWin, pointsForDraw, pointsForLoss);

            if (HasMatchDays)
                MatchDays = new List<MatchDay>();
	    }
    }
}
