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
    [Table("LeagueCompetitor")]
    public class LeagueCompetitor : Competitor
    {
        public LeagueCompetitor()
        {

        }

        public int InitialPositionNumber { get; set; }
        public int CurrentPositionNumber { get; set; }
        public virtual League League { get; set; }
    }
}
