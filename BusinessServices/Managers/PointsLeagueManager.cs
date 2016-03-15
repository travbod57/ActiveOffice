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
    }
}