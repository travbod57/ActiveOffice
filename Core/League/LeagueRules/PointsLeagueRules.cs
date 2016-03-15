using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.LeagueRules
{
    class PointsLeagueRules : ILeagueRules
    {
        private readonly int _pointsForWin;
        private readonly int _pointsForDraw;
        private readonly int _pointsForLoss;

        public PointsLeagueRules(int pointsForWin, int pointsForDraw, int pointsForLoss)
        {
            _pointsForWin = pointsForWin;
            _pointsForDraw = pointsForDraw;
            _pointsForLoss = pointsForLoss;
        }

        public void RewardWin(LeagueCompetitor competitor)
        {
            AddPoints(competitor, _pointsForWin);
        }

        public void RewardDraw(LeagueCompetitor competitorA, LeagueCompetitor competitorB)
        {
            //AddPoints(side, _pointsForDraw);
        }

        public void RewardLoss(LeagueCompetitor competitor)
        {
            AddPoints(competitor, _pointsForLoss);
        }

        private void AddPoints(LeagueCompetitor competitor, int points)
        {
            competitor.Points += points;
        }
    }
}
