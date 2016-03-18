using Model.Knockouts;
using Model.Leagues;
using Model.Scheduling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Competitors
{
    [Table("KnockoutCompetitor")]
    public class KnockoutCompetitor : Competitor
    {
        public KnockoutCompetitor()
        {

        }

        public int? InitialSeeding { get; set; }
        
        public virtual Knockout Knockout { get; set; }
    }
}
