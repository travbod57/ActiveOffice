﻿using Model.CompetitionOwnership;
using Model.Competitors;
using Model.ReferenceData;
using Model.Schedule;
using Model.Scheduling;
using Model.Sports;
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
            SportColumns = new List<SportColumn>();
        }

        #region Config
        public int NumberOfPositions { get; set; }
        public int NumberOfMatchUps { get; set; }
        public bool HasScheduledMatches { get; set; }
        public bool HasAdhocMatches { get; set; } 
        #endregion

        #region Properties
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } 
        #endregion

        public bool IsActive { get; set; }
        public bool IsCreated { get; set; }

        public int NumberOfMatches
        {
            get { return LeagueMatches.Count; }
        }

        public virtual Tournament Tournament { get; set; }
        public virtual SportType SportType { get; set; }
        public virtual Season Season { get; set; }
        public virtual CompetitionType CompetitionType { get; set; }

        public virtual ICollection<LeagueMatch> LeagueMatches { get; set; }
        public virtual ICollection<LeagueCompetitor> LeagueCompetitors { get; set; }
        public virtual ICollection<LeagueAdmin> LeagueAdmins { get; set; }
        public virtual ICollection<SportColumn> SportColumns { get; set; }
    }
}
