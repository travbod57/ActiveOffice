using Model.Competitors;
using Model.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Dtos
{
    public class LeagueTableRowDto
    {
        public string SideName { get; set; }
        public CompetitorRecord CompetitorRecord { get; set; }
    }
}
