using Model.Packages;
using Model.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ReferenceData
{
    public class CompetitionType : DBEntity
    {
        public CompetitionType()
        {
            Packages = new List<Package>();
            Accounts = new List<Account>();
        }

        public string Name { get; set; }

        public virtual ICollection<Package> Packages { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
