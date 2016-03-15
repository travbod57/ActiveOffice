using BusinessServices.Dtos;
using BusinessServices.Helpers;
using BusinessServices.Interfaces;
using Model.Competitors;
using Model.Extensions;
using Model.Leagues;
using Model.Record;
using Model.Schedule;
using System.Collections.Generic;
using System.Linq;

namespace BusinessServices.Updaters
{
    public class ChallengeLeagueManager : LeagueManagerBase, ILeagueManager
    {
        private ChallengeLeague _challengeLeague;

        public ChallengeLeagueManager(ChallengeLeague challengeLeague, ISportManager sportManager) : base(challengeLeague, sportManager)
        {
            _challengeLeague = challengeLeague;
        }

        public override void AwardDraw(LeagueMatch leagueMatch, Competitor competitorA, Competitor competitorB)
        {
            if (_challengeLeague.CanDraw)
            {
                base.AwardDraw(leagueMatch, competitorA, competitorB);
            }
        }

        private void UpdateStandings(Competitor winner, Competitor loser)
        {
            int winnerPosition = WinnerRecords["Position"].Value;
            int loserPosition = LoserRecords["Position"].Value;

            bool challengerWins = winnerPosition < loserPosition;

            if (challengerWins)
            {



            }
        }

        public override List<LeagueTableRowDto> GetLeagueStandings()
        {
            List<LeagueTableRowDto> standings = base.GetLeagueStandings();

            return standings.OrderByDescending(s => s.ColumnValues.Single(x => x.Item1 == "Position").Item2).ToList();
        }
    }
}
