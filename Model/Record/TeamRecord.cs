using Model.Actors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Record

{
    public class TeamRecord : DBEntity
    {
        [Key, ForeignKey("Team")]
        public int TeamId { get; set; }

        public virtual Team Team { get; set; }
    }
}
