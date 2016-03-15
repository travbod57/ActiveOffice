using BusinessServices.Interfaces;
using Model.Competitors;
using Model.Schedule;

namespace BusinessServices.Updaters
{
    public class LeagueManager
    {
        private ILeagueManager _leagueManager { get; set; }

        public LeagueManager(ILeagueManager leagueManager)
        {
            _leagueManager = leagueManager;
        }

        public void AddMatch(Competitor competitorA, Competitor competitorB)
        {
            _leagueManager.AddMatch(competitorA, competitorB);
        }

        public void AwardWin(LeagueMatch leagueMatch, Competitor winner, Competitor loser)
        {
            _leagueManager.AwardWin(leagueMatch, winner, loser);
        }

        public void AwardDraw(LeagueMatch leagueMatch, Competitor competitorA, Competitor competitorB)
        {
            _leagueManager.AwardDraw(leagueMatch, competitorA, competitorB);
        }
    }
}
