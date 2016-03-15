using Model.Packages;
using Model.ReferenceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.UserManagement
{
    public class Account : DBEntity
    {
        public Account()
        {
            SportTypes = new List<SportType>();
            CompetitionTypes = new List<CompetitionType>();
            Administrators = new List<User>();
        }

        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyTelephone { get; set; }

        public Package Package { get; set; }

        public bool IsPayingAdvertisedPackageCost { get; set; }
        public bool IsPayingMonthly { get; set; }
        public bool IsPayingAnnually { get; set; }

        public decimal ActualPackageMonthlyPayment { get; set; }
        public decimal ActualPackageAnnualPayment { get; set; }

        public DateTime BillingDate { get; set; }
        public DateTime RenewalDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        // these link to the choice this account ahs made for its sports and competition. Number governed by the package chosen
        public ICollection<SportType> SportTypes { get; set; }
        public ICollection<CompetitionType> CompetitionTypes { get; set; }
        public ICollection<User> Administrators { get; set; }
    }
}
