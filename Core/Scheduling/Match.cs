using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Match
    {
        public Match(Side sideA, Side sideB)
        {
            this.SideA = sideA;
            this.SideB = sideB;
        }

        public Side SideA { get; set; }
        public Side SideB { get; set; }

        public int ScoreToWin { get; set; }
        public int BestOf { get; set; }

        public Result Result { get; set; }
    }
}
