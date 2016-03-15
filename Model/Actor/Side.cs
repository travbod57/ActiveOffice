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
        public Side()
        {
            SportSideRecords = new List<SportSideRecord>();
        }

        public string Name { get; set; }
        public ICollection<SportSideRecord> SportSideRecords { get; set; }
    }
}
