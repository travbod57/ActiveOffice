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
            // get them in order
           var standings = _pointsLeague.LeagueCompetitors.OrderByDescending(s => s.Points)
                            .ThenByDescending(s => s.Difference)
                            .ThenBy(s => s.For)
                            .ToList();

            // update the record if their position has changed
            for (int i = 0; i < standings.Count; i++)
		    {
                LeagueCompetitor competitor = standings[i];
                if (competitor.CurrentPositionNumber != i + 1)
                    competitor.CurrentPositionNumber = i + 1;
		    }
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