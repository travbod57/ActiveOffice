using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Scheduling
{
    public class MatchDay
    {
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public List<Match> Matches { get; set; }
    }
}
