using Model.CompetitionOwnership;
using Model.Competitors;
using Model.Interfaces;
using Model.ReferenceData;
using Model.Schedule;
using Model.Sports;
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
    public class Knockout : DBEntity, IAudit
    {
        public Knockout()
        {
            KnockoutAdmins = new List<KnockoutAdmin>();
            KnockoutCompetitors = new List<KnockoutCompetitor>();
            SportColumns = new List<SportColumn>();
            KnockoutMatches = new List<KnockoutMatch>();
        }

        public int NumberOfCompetitors { get; set; }
        public int NumberOfRounds { get; set; }
        public bool IsSeeded { get; set; }
        public bool IncludeThirdPlacePlayoff { get; set; }

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
        public bool IsCreated { get; set; }

        public virtual ICollection<KnockoutAdmin> KnockoutAdmins { get; set; }
        public virtual ICollection<KnockoutCompetitor> KnockoutCompetitors { get; set; }
        public virtual ICollection<SportColumn> SportColumns { get; set; }
        public virtual ICollection<KnockoutMatch> KnockoutMatches { get; set; }
        public virtual CompetitionType CompetitionType { get; set; }

        #region IAudit
        [NotMapped]
        public EnumSubjectType SubjectType { get { return EnumSubjectType.Knockout; } }
        #endregion
    }
}
