using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    [Flags]
    enum Outcome
    {
        Win = 0,
        Lose = 1,
        Draw = 2,
        Home = 4,
        Away = 8
    }
}
