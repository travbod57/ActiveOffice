using Model.Actors;
using Model.Record;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Competitors
{
    [Table("Competitor")]
    public abstract class Competitor : DBEntity
    {
        public Competitor()
        {
            CompetitorHistoryRecords = new List<CompetitorHistoryRecord>();
        }

        public virtual Side Side { get; set; }
        public virtual CompetitorRecord CompetitorRecord { get; set; }
        public virtual ICollection<CompetitorHistoryRecord> CompetitorHistoryRecords { get; set; }
    }
}
