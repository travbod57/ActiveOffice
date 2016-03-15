using Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ReferenceData
{
    public class MatchState : DBEntity
    {
        public MatchState()
        {
            Matches = new List<Match>();
        }

        public string Name { get; set; }

        public ICollection<Match> Matches { get; set; }
    }
}
