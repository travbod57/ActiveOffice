using Model.ReferenceData;
using Model.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Packages
{
    public class Package : DBEntity
    {
        public Package()
        {
            Accounts = new List<Account>();
            CompetitionTypes = new List<CompetitionType>();
        }

        public string Name { get; set; } // e.g. summer time deal!
        public int NumberOfSportTypes { get; set; }
        public int NumberOfCompetitionTypes { get; set; }
        public int NumberOfCompetitions { get; set; }
        public int NumberOfSidesPerCompetition { get; set; }
        public bool HasLeagueDivisions { get; set; }
        public bool CanGenerateFixtures { get; set; }
        public bool CanViewStats { get; set; }
        public bool CanAccessApp { get; set; }

        public decimal AdvertisedCostPerMonth { get; set; }
        public decimal AdvertisedCostPerYear { get; set; }

        public bool IsDiscountActive { get; set; }
        public decimal? AdvertisedDiscountedCostPerMonth { get; set; }
        public decimal? AdvertisedDiscountedCostPerYear { get; set; }

        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }

        public bool IsActive { get; set; }

        public virtual PackageType PackageType { get; set; }
        public virtual PackageDiscount PackageDiscount { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<CompetitionType> CompetitionTypes { get; set; }
    }
}
