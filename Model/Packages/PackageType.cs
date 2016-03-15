using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Packages
{
    public class PackageType : DBEntity
    {
        public PackageType()
        {
            Packages = new List<Package>();
        }

        public string Name { get; set; }
        public virtual ICollection<Package> Packages { get; set; }
    }
}
