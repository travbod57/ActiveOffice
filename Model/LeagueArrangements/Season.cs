using Model.Knockouts;
using Model.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.LeagueArrangements
{
    public class Season : DBEntity
    {
        public Season()
        {
            Leagues = new List<League>();
            Knockouts = new List<Knockout>();
        }

        public string Name { get; set; }

        public ICollection<League> Leagues { get; set; }
        public ICollection<Knockout> Knockouts { get; set; }
    }
}
