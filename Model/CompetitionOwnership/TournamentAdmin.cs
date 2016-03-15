using Model.Tournaments;
using Model.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.CompetitionOwnership
{
    public class TournamentAdmin : DBEntity
    {
        public Tournament Tournament { get; set; }
        public User User { get; set; }
    }
}
