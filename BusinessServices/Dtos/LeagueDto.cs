using Model.Record;
using System.Collections.Generic;

namespace BusinessServices.Dto
{
    public class LeagueDto
    {
        public LeagueDto()
        {
            CompetitorStandings = new List<CompetitorRecord>();
        }

        public string SideName { get; set; }
        public List<CompetitorRecord> CompetitorStandings { get; set; }
    }
}
