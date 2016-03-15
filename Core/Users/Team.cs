using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Team : Side
    {
        public Team()
        {
            this.Players = new List<Player>();
        }

        public IList<Player> Players { get; set; }
    }
}
