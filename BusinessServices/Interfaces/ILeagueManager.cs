using BusinessServices.Dtos;
using Model.Actors;
using Model.Competitors;
using Model.Schedule;
using System.Collections.Generic;

namespace BusinessServices.Interfaces
{
    public interface ILeagueManager
    {
        void AwardWin(LeagueMatch leagueMatch, LeagueCompetitor winner, LeagueCompetitor loser);
        void AwardDraw(LeagueMatch leagueMatch, LeagueCompetitor competitorA, LeagueCompetitor competitorB);
        void AddMatch(LeagueCompetitor competitorA, LeagueCompetitor competitorB);
        List<LeagueTableRowDto> GetLeagueStandings();
        void RenewLeague();
    }
}
