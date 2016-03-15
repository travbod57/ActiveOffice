using Model.Actors;
using Model.CompetitionOwnership;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.UserManagement
{
    public class User : DBEntity
    {
        public User()
        {
            LeagueAdmins = new List<LeagueAdmin>();
            TournamentAdmins = new List<TournamentAdmin>();
            KnockoutAdmins = new List<KnockoutAdmin>();
            Audits = new List<Audit>();
        }

        [Key, ForeignKey("Player")]
        public int PlayerId { get; set; }

        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string GravitarURL { get; set; }

        public bool IsAccountAdministrator 
        {
            get { return Account != null; }
        }

        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

        public virtual Player Player { get; set; }

        public virtual ICollection<LeagueAdmin> LeagueAdmins { get; set; }
        public virtual ICollection<TournamentAdmin> TournamentAdmins { get; set; }
        public virtual ICollection<KnockoutAdmin> KnockoutAdmins { get; set; }
        public virtual ICollection<Audit> Audits { get; set; }
    }
}
