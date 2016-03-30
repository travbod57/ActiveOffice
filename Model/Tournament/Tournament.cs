using Model.CompetitionOwnership;
using Model.Competitors;
using Model.Interfaces;
using Model.Knockouts;
using Model.Leagues;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tournaments
{
    [Table("Tournament")]
    public class Tournament : DBEntity, IAudit
    {
        public Tournament()
        {
            Leagues = new List<League>();
            TournamentAdmins = new List<TournamentAdmin>();
            TournamentCompetitors = new List<TournamentCompetitor>();
        }

        public int NumberOfPools { get; set; }

        public virtual Knockout Knockout { get; set; }

        public int NumberOfRounds { get; set; }

        public string Name { get; set; }
        public string Sponsor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<League> Leagues { get; set; }
        public virtual ICollection<TournamentAdmin> TournamentAdmins { get; set; }
        public virtual ICollection<TournamentCompetitor> TournamentCompetitors { get; set; }

        #region IAudit
        [NotMapped]
        public EnumSubjectType SubjectType { get { return EnumSubjectType.Tournament; } }
        #endregion
    }
}
