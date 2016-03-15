using Model.Knockouts;
using Model.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.CompetitionOwnership
{
    public class KnockoutAdmin : DBEntity
    {
        public Knockout Knockout { get; set; }
        public User User { get; set; }
    }
}
