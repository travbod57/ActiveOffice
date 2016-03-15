using Model.CompetitionOwnership;
using Model.Tournaments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Knockouts
{
    [Table("Knockout")]
    public class Knockout : DBEntity
    {
        public Knockout()
        {
            KnockoutAdmins = new List<KnockoutAdmin>();
        }

        public int NumberOfRounds { get; set; }

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<KnockoutAdmin> KnockoutAdmins { get; set; }
    }
}
