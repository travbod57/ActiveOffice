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
                winner.CurrentPositionNumber = loserPosition;
                loser.CurrentPositionNumber = loserPosition + 1;

                int upperBound = winner.CurrentPositionNumber + 2;
                int lowerBound = winnerPosition;

                foreach (var competitor in _challengeLeague.LeagueCompetitors)
	            {
                    if (competitor.CurrentPositionNumber <= lowerBound && competitor.CurrentPositionNumber >= upperBound)
                        competitor.CurrentPositionNumber--;
	            }
            }
        }

        public override List<LeagueTableRowDto> GetLeagueStandings()
        {
            List<LeagueTableRowDto> standings = base.GetLeagueStandings();

            return standings.OrderByDescending(s => s.ColumnValues.Single(x => x.Item1 == "Position").Item2).ToList();
        }
    }
}
