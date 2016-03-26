using Model.Knockouts;
using Model.Scheduling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Schedule
{
    [Table("KnockoutMatch")]
    public class KnockoutMatch : Match
    {
        public Knockout Knockout { get; set; }
        public KnockoutMatch NextRoundMatch { get; set; }
        public KnockoutMatch AlternativeNextRoundMatch { get; set; }
        public int MatchNumberForRound { get; set; }
        public EnumRound Round { get; set; }
        public EnumKnockoutSide KnockoutSide { get; set; }
    }
}
