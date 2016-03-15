using Model.Record;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Actors
{
    [Table("Team")]
    public class Team : Side
    {
        public Team()
        {
            this.Players = new List<Player>();
        }

        public virtual TeamRecord TeamRecord { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}
