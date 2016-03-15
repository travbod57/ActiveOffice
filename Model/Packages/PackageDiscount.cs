using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Packages
{
    public class PackageDiscount : DBEntity
    {
        public PackageDiscount()
        {
            Packages = new List<Package>();
        }

        public string Name { get; set; }
        public decimal DiscountPercentage { get; set; }

        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }

        public virtual ICollection<Package> Packages { get; set; }
    }
}
