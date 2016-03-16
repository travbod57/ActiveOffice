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

        public override void AwardWin(LeagueMatch leagueMatch, LeagueCompetitor winner, LeagueCompetitor loser)
        {
            base.AwardWin(leagueMatch, winner, loser);
            UpdateStandings(winner, loser);
        }

        public override void AwardDraw(LeagueMatch leagueMatch, LeagueCompetitor competitorA, LeagueCompetitor competitorB)
        {
            if (_challengeLeague.CanDraw)
            {
                base.AwardDraw(leagueMatch, competitorA, competitorB);
            }
        }

        private void UpdateStandings(LeagueCompetitor winner, LeagueCompetitor loser)
        {
            int winnerPosition = winner.CurrentPositionNumber;
            int loserPosition = loser.CurrentPositionNumber;

            bool challengerWins = winnerPosition > loserPosition;

            if (challengerWins)
            {
                winner.CurrentPositionNumber = loserPosition; // CompetitorRecordHelpers.WriteCompetitorHistoryRecords(loser, LoserRecords); to track posotion , need position column in congiguration file
                loser.CurrentPositionNumber = loserPosition + 1; // CompetitorRecordHelpers.WriteCompetitorHistoryRecords(loser, LoserRecords);

                int upperBound = winner.CurrentPositionNumber + 1;
                int lowerBound = winnerPosition;

                foreach (var competitor in _challengeLeague.LeagueCompetitors.Where( lc => !lc.Equals(winner) && !lc.Equals(loser)))
	            {
                    if (competitor.CurrentPositionNumber <= lowerBound && competitor.CurrentPositionNumber >= upperBound)
                        competitor.CurrentPositionNumber++; // CompetitorRecordHelpers.WriteCompetitorHistoryRecords(loser, LoserRecords);
	            }
            }
        }

        public override List<LeagueTableRowDto> GetLeagueStandings()
        {



            List<LeagueTableRowDto> standings = base.GetLeagueStandings();

            return standings.OrderBy(s => s.ColumnValues.Single(x => x.Item1 == "Position").Item2).ToList();
        }
    }
}
