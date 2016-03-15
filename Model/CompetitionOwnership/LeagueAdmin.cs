using Model.Leagues;
using Model.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.CompetitionOwnership
{
    public class LeagueAdmin : DBEntity
    {
        public User User { get; set; }
        public League League { get; set; }
    }
}
