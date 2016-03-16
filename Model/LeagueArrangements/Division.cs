using Model.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.LeagueArrangements
{
    public class Division : DBEntity
    {
        public Division()
        {
            Leagues = new List<League>();
        }

        public string Name { get; set; }
        public virtual Cluster Cluster { get; set; }

        public ICollection<League> Leagues { get; set; }
    }
}
