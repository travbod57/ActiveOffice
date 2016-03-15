using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Tournament : Competition
    {
        public int NumberOfPools { get; set; }

        public List<Core.League.League> Leagues { get; set; }

        public int NumberOfRounds { get; set; }
        
    }
}
