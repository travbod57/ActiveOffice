using BusinessServices.Interfaces;
using Model.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Builders.TournamentCompetition
{
    public class TournamentConfig
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } 
        public int NumberOfRounds { get; set; } 
        public bool IsSeeded { get; set; } 
        public bool IncludeThirdPlacePlayoff { get; set; } 
        public int NumberOfPools { get; set; } 
        public int NumberOfPositionsPerPool { get; set; }
        public IAuditLogger AuditLogger { get; set; }
        public List<Side> Sides { get; set; }
    }
}
