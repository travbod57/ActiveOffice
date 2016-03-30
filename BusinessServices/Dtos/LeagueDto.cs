using Model.Record;
using System.Collections.Generic;

namespace BusinessServices.Dto
{
    public class LeagueDto
    {
        public string SideName { get; set; }
        public CompetitorRecord CompetitorStanding { get; set; }
    }
}
