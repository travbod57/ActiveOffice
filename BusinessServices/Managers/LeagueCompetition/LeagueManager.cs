using BusinessServices.Interfaces;
using Model.Competitors;
using Model.Schedule;

namespace BusinessServices.Managers.LeagueCompetition
{
    public class LeagueManager
    {
        private ILeagueManager _leagueManager { get; set; }

        public LeagueManager(ILeagueManager leagueManager)
        {
            _leagueManager = leagueManager;
        }

        public void AddMatch(LeagueCompetitor competitorA, LeagueCompetitor competitorB)
        {
            _leagueManager.AddMatch(competitorA, competitorB);
        }

        public void AwardWin(LeagueMatch leagueMatch, LeagueCompetitor winner, LeagueCompetitor loser)
        {
            _leagueManager.AwardWin(leagueMatch, winner, loser);
        }

        public void AwardDraw(LeagueMatch leagueMatch, LeagueCompetitor competitorA, LeagueCompetitor competitorB)
        {
            _leagueManager.AwardDraw(leagueMatch, competitorA, competitorB);
        }
    }
}
