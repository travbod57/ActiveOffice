using BusinessServices.Builders.LeagueCompetition;
using Model.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Extensions
{
    public static class ModelExtensions
    {
        public static LeagueConfig ExtractLeagueConfig(this League pointsLeague)
        {
            LeagueConfig leagueConfig = new LeagueConfig()
            {
                Name = pointsLeague.Name,
                NumberOfMatchUps = pointsLeague.NumberOfMatchUps,
                NumberOfPositions = pointsLeague.NumberOfPositions,
                SportColumns = pointsLeague.SportColumns.ToList(),
                IsPartOfTournament = pointsLeague.Tournament != null
            };

            return leagueConfig;
        }
    }
}
