using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Result
    {
        public Side Winner { get; set; }
        public Side Loser { get; set; }
        public bool IsDraw { get; set; }
    }
}
