using Model.Knockouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces
{
    public interface IKnockoutSorter
    {
        void Reset(Knockout knockout);
    }
}
