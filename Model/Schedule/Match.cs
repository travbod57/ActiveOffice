using Model.Actors;
using Model.Competitors;
using Model.Leagues;
using Model.ReferenceData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Scheduling
{
    [Table("Match")]
    public class Match : DBEntity
    {
        // might be challenger or home side
        public Competitor CompetitorA { get; set; }
        public int CompetitorAScore { get; set; }
        // might be challengee or away side
        public Competitor CompetitorB { get; set; }
        public int CompetitorBScore { get; set; }

        public Competitor Winner { get; set; }
        public Competitor Loser { get; set; }

        public bool IsDraw { get; set; }

        public DateTime? DateTimeOfPlay { get; set; }

        public EnumMatchState MatchState { get; set; }
    }
}
