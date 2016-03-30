using BusinessServices.Interfaces;
using Model.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Builders.KnockoutCompetition
{
    public class KnockoutConfig
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IList<Side> Sides { get; set; }
        public int NumberOfRounds { get; set; }
        public bool IsSeeded { get; set; }
        public bool IncludeThirdPlacePlayoff { get; set; }
        public IAuditLogger AuditLogger { get; set; }
        public bool IsPartOfTournament { get; set; }
    }
}
