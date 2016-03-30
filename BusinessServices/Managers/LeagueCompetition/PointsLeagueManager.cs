using BusinessServices.Builders.LeagueCompetition;
using BusinessServices.Dtos;
using BusinessServices.Interfaces;
using Model.Competitors;
using Model.Leagues;
using Model.Record;
using Model.Schedule;
using Model.Extensions;
using System.Collections.Generic;
using System.Linq;
using BusinessServices.Extensions;
using BusinessServices.Schedulers;
using BusinessServices.Dtos.League;
using Model.Actors;
using Core.Extensions;

namespace BusinessServices.Managers.LeagueCompetition
{
    public class PointsLeagueManager : LeagueManagerBase, ILeagueManager
    {
        private PointsLeague _pointsLeague;

        public PointsLeagueManager(PointsLeague pointsLeague, ISportManager sportManager) : base(pointsLeague, sportManager)
        {
            _pointsLeague = pointsLeague;
        }

        public override void AwardWin(LeagueMatch leagueMatch, LeagueCompetitor winner, LeagueCompetitor loser)
        {
            base.AwardWin(leagueMatch, winner, loser);
            UpdateStandings();
        }

        public override void AwardDraw(LeagueMatch leagueMatch, LeagueCompetitor competitorA, LeagueCompetitor competitorB)
        {
            base.AwardDraw(leagueMatch, competitorA, competitorB);
            UpdateStandings();
        }

        private void UpdateStandings()
        {
            // could reorder by same way as standings and then assign new position. Only send update sql for those that have changed.
        }

        public override List<LeagueTableRowDto> GetLeagueStandings()
        {
            List<LeagueTableRowDto> standings = _pointsLeague.LeagueCompetitors.Select(lc => new LeagueTableRowDto()
            {
                SideName = lc.Side.Name,
                CompetitorRecord = lc.CompetitorRecord
            }).OrderByDescending(s => s.CompetitorRecord.Points)
                .ThenByDescending(s => s.CompetitorRecord.Difference)
                .ThenBy(s => s.CompetitorRecord.For)
                .ToList();

            return standings;
        }

        public override void RenewLeague()
        {
            League divisionAbove = _pointsLeague.DivisionAbove;
            League divisionBelow = _pointsLeague.DivisionBelow;

            List<Side> relegatedSides = divisionAbove.LeagueCompetitors.Where(lc => lc.IsRelegated).Select(lc => lc.Side).ToList();
            List<Side> promotedSides = divisionBelow.LeagueCompetitors.Where(lc => lc.IsPromoted).Select(lc => lc.Side).ToList();

            LeagueConfig leagueConfig = _pointsLeague.ExtractLeagueConfig();
            leagueConfig.IsRenewedLeague = true;
            leagueConfig.Sides.AddRange(promotedSides);
            leagueConfig.Sides.AddRange(relegatedSides);

            LeagueCreatorDto leagueCreatorDto = new LeagueCreatorDto();

            LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>(leagueConfig);
            
            PointsLeague pointsLeague = new PointsLeague();
            IMatchScheduler matchScheduler = new RandomLeagueMatchScheduler(pointsLeague, leagueCreatorDto);
            LeagueBuilder<PointsLeague> builder = new LeagueBuilder<PointsLeague>(pointsLeague, matchScheduler);

            director.Construct(builder);
        }
    }
}