using Model.Competitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Dtos
{
    public class LeagueTableRowDto
    {
        public LeagueTableRowDto()
        {
            ColumnValues = new List<Tuple<string, int>>();
        }

        public string SideName { get; set; }
        public List<Tuple<string, int>> ColumnValues { get; set; }
    }
}
