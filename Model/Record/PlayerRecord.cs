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
    public class PlayerRecord : DBEntity
    {
        [Key, ForeignKey("Player")]
        public int PlayerId { get; set; }

        public virtual Player Player { get; set; }
    }
}
