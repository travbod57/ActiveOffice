using Model.Competitors;
using Model.Interfaces;
using Model.ReferenceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Record
{
    public class CompetitorHistoryRecord : DBEntity, IFootballRecord
    {
        public CompetitorHistoryRecord()
        {
            TimeStamp = DateTime.Now;
        }

        public virtual Competitor Competitor { get; set; }
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

        public DateTime TimeStamp { get; set; }
    }
}
