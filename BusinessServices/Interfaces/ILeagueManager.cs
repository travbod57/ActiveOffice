using BusinessServices.Dtos;
using Model.Competitors;
using Model.Schedule;
using System.Collections.Generic;

namespace BusinessServices.Interfaces
{
    public interface ILeagueManager
    {
        void AwardWin(LeagueMatch leagueMatch,Competitor winner, Competitor loser);
        void AwardDraw(LeagueMatch leagueMatch, Competitor competitorA, Competitor competitorB);
        void AddMatch(Competitor competitorA, Competitor competitorB);
        List<LeagueTableRowDto> GetLeagueStandings();
    }
}
