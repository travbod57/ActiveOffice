using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.LeagueArrangements
{
    public class Cluster : DBEntity
    {
        public Cluster()
        {
            Divisions = new List<Division>();
        }

        public string Name { get; set; }

        public ICollection<Division> Divisions { get; set; }
    }
}
