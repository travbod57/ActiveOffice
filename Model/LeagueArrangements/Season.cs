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
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual Cluster Cluster { get; set; }

        public virtual League League { get; set; }
        public virtual Knockout Knockout { get; set; }
    }
}
