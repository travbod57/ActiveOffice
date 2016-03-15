using Model.Competitors;
using Model.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Record
{
    public class CompetitorHistoryRecord : DBEntity
    {
        public virtual Competitor Competitor { get; set; }
        public virtual SportColumn SportColumn { get; set; }
        public int Value { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
