using Model.Record;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Actors
{
    [Table("Side")]
    public abstract class Side : DBEntity
    {
        public string Name { get; set; }
    }
}
