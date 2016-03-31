using Model.Actors;
using Model.Interfaces;
using Model.Record;
using Model.ReferenceData;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Competitors
{
    [Table("Competitor")]
    public abstract class Competitor : DBEntity, IFootballRecord
    {
        public Competitor()
        {
            CompetitorHistoryRecords = new List<CompetitorHistoryRecord>();
        }

        public virtual Side Side { get; set; }

        public SportType SportType { get; set; }

        public int Played { get; set; }
        public int Points { get; set; }
        public int BonusPoints { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int For { get; set; }
        public int Against { get; set; }
        public int Difference { get; set; }
        public int Laps { get; set; }
        public int Races { get; set; }
        public int Sets { get; set; }
        public int Games { get; set; }

        public virtual ICollection<CompetitorHistoryRecord> CompetitorHistoryRecords { get; set; }
    }
}
