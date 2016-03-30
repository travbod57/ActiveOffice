using BusinessServices.Builders.LeagueCompetition;
using BusinessServices.Dtos;
using BusinessServices.Dtos.League;
using BusinessServices.Interfaces;
using BusinessServices.Schedulers;
using Model.Actors;
using Model.Competitors;
using Model.Extensions;
using Model.Leagues;
using Model.Record;
using Model.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessServices.Extensions;

namespace BusinessServices.Managers.LeagueCompetition
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
            List<LeagueTableRowDto> standings = _challengeLeague.LeagueCompetitors.Select(lc => new LeagueTableRowDto()
            {
                SideName = lc.Side.Name,
                CompetitorRecord = lc.CompetitorRecord
            }).OrderBy(s => s.CompetitorRecord.Position).ToList();

            return standings;
        }

        public void CreateChallenge(LeagueCompetitor challenger, LeagueCompetitor defender, DateTime dateTimeOfPlay)
        {
            LeagueMatch leagueMatch = new LeagueMatch();
            leagueMatch.CompetitorA = challenger;
            leagueMatch.CompetitorB = defender;
            leagueMatch.DateTimeOfPlay = dateTimeOfPlay;

            _challengeLeague.LeagueMatches.Add(leagueMatch);
        }

        public override void RenewLeague()
        {
            LeagueConfig leagueConfig = _challengeLeague.ExtractLeagueConfig();
            leagueConfig.IsRenewedLeague = true;

            LeagueCreatorDto leagueCreatorDto = new LeagueCreatorDto();

            LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>(leagueConfig);

            PointsLeague pointsLeague = new PointsLeague();
            IMatchScheduler matchScheduler = new NonMatchScheduler();
            LeagueBuilder<PointsLeague> builder = new LeagueBuilder<PointsLeague>(pointsLeague, matchScheduler);

            director.Construct(builder);
        }
    }
}
