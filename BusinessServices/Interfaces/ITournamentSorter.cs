using Model.Knockouts;
using Model.Tournaments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces
{
    public interface ITournamentSorter
    {
        void Reset(Tournament tournament);
    }
}
