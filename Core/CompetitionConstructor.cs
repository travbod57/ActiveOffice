using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class CompetitionConstructor
    {
        // Builder uses a complex series of steps
        public void Construct(CompetitionBuilder builder)
        {
            builder.AddMatches();
        }
    }
}
