using Model.CompetitionOwnership;
using Model.Competitors;
using Model.LeagueArrangements;
using Model.ReferenceData;
using Model.Schedule;
using Model.Scheduling;
using Model.Tournaments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Leagues
{
    [Table("League")]
    public abstract class League : DBEntity
    {
        public League()
        {
            LeagueCompetitors = new List<LeagueCompetitor>();
            LeagueMatches = new List<LeagueMatch>();
            LeagueAdmins = new List<LeagueAdmin>();
            TournamentCompetitors = new List<TournamentCompetitor>();
        }

        #region Config
        public int NumberOfCompetitors { get; set; }
        public int NumberOfMatchUps { get; set; }
        public bool HasScheduledMatches { get; set; }
        public bool HasAdhocMatches { get; set; } 
        #endregion

        #region Properties
        public string Name { get; set; }
        public string Sponsor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DivisionNumber { get; set; }
        public virtual League DivisionAbove { get; set; }
        public virtual League DivisionBelow { get; set; }
        public virtual Tournament Tournament { get; set; }
        public virtual Cluster Cluster { get; set; }
        public virtual SportType SportType { get; set; }
        public virtual Season Season { get; set; }
        public virtual CompetitionType CompetitionType { get; set; }
        public bool IsFinalised { get; set; }
        public bool IsRenewed { get; set; }
        public bool IsActive { get; set; }
        public bool IsCreated { get; set; }
        public int NumberOfMatches
        {
            get { return LeagueMatches.Count; }
        }
        public int NumberOfPositions
        {
            get { return LeagueCompetitors.Count; }
        }
        public int NumberOfRelegationPositions { get; set; }
        public int NumberOfPromotionPositions { get; set; }
        #endregion

        public virtual ICollection<LeagueMatch> LeagueMatches { get; set; }
        public virtual ICollection<LeagueCompetitor> LeagueCompetitors { get; set; }
        public virtual ICollection<LeagueAdmin> LeagueAdmins { get; set; }
        public virtual ICollection<TournamentCompetitor> TournamentCompetitors { get; set; }
        
    }
}
