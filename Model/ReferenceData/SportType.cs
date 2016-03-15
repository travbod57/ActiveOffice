using Model.Leagues;
using Model.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ReferenceData
{
    public class SportType : DBEntity
    {
        public SportType()
        {
            Accounts = new List<Account>();
            Leagues = new List<League>();
        }

        public string Name { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public virtual ICollection<League> Leagues { get; set; }
    }
}
