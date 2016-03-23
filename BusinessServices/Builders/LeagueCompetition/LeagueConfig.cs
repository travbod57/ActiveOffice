using BusinessServices.Interfaces;
using Model.Actors;
using Model.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Builders.LeagueCompetition
{
    public class LeagueConfig
    {
        public string Name { get; set; } 
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }  
        public int NumberOfPositions { get; set; }  
        public int NumberOfMatchUps { get; set; }  
        public IList<Side> Sides { get; set; }  
        public IAuditLogger AuditLogger { get; set; }  
        public IList<SportColumn> SportColumns { get; set; }
        public bool IsPartOfTournament { get; set; }
        public bool IsRenewedLeague { get; set; }
    }
}
