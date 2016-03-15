using Model.Competitors;
using Model.Leagues;
using Model.Scheduling;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Schedule
{
    [Table("LeagueMatch")]
    public class LeagueMatch : Match
    {
        public League League { get; set; }
    }
}
