using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.LeagueRules
{
    public class ChallengeLeagueRules : ILeagueRules
    {
        public void RewardWin(LeagueCompetitor competitor)
        {
            AssumePosition(competitor);
        }

        public void RewardDraw(LeagueCompetitor competitorA, LeagueCompetitor competitorB)
        {

        }

        public void RewardLoss(LeagueCompetitor competitor)
        {

        }


        private void AssumePosition(LeagueCompetitor competitor)
        {

        }
    }
}
