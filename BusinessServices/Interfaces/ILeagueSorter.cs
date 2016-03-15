using Model.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces
{
    public interface ILeagueSorter
    {
        void UpdatePosition();
        void Reset(League league);
    }
}
