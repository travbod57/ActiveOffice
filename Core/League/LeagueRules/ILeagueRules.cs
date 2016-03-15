using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.LeagueRules
{
    public interface ILeagueRules
    {
        void RewardWin(LeagueCompetitor competitor);
        void RewardDraw(LeagueCompetitor competitorA, LeagueCompetitor competitorB);
        void RewardLoss(LeagueCompetitor competitor);
    }
}
