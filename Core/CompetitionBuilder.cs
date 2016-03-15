using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{

    public abstract class CompetitionBuilder
    {
        public abstract void AddMatches();
        public abstract Competition GetResult();
    }
}
