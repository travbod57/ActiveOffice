using BusinessServices.Builders.LeagueCompetition;
using BusinessServices.Dtos;
using BusinessServices.Helpers;
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

        public override List<LeagueTableRowDto> GetLeagueStandings()
        {
            List<LeagueTableRowDto> standings = base.GetLeagueStandings();

            return standings.OrderByDescending(s => s.ColumnValues.Single(x => x.Item1 == "Points").Item2)
                .ThenByDescending(s => s.ColumnValues.Single(x => x.Item1 == "GoalDifference").Item2)
                .ThenBy(s => s.ColumnValues.Single(x => x.Item1 == "GoalsFor").Item2)
                .ToList();
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