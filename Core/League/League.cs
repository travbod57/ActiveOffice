using Core.LeagueRules;
using Core.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.League
{
    public abstract class League : Competition
    {
        protected ILeagueRules leagueRules;

        public List<LeagueCompetitor> Competitors { get; set; }
        

        public League(int numberOfPostions)
        {
            NumberOfPositions = numberOfPostions;
            Competitors = new List<LeagueCompetitor>();
        }

        public bool HasMatchDays { get; set; }
        
        public int NumberOfCompetitors 
        {
            get { return Competitors.Count; }
        }

        public bool IsLeagueFull 
        {
            get { return NumberOfCompetitors == NumberOfPositions; }
        }


        public int NumberOfPositions { get; set; }

        public void RewardWin(LeagueCompetitor competitor)
        {
            leagueRules.RewardWin(competitor);
        }

        public void RewardDraw(LeagueCompetitor competitorA, LeagueCompetitor competitorB)
        {
            leagueRules.RewardDraw(competitorA, competitorB);
        }

        public void RewardLoss(LeagueCompetitor competitor)
        {
            leagueRules.RewardLoss(competitor);
        }
    }
}
