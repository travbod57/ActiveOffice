using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Dtos
{
    public class LeagueTableDto
    {
        public List<LeagueTableRowDto> LeagueTableRowDtos { get; set; }
        public List<string> ColumnHeadings { get; set; }
    }
}
