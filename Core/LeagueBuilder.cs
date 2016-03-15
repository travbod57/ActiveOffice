using Core.LeagueRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class LeagueBuilder
    {
        private ILeagueRules _leagueRules;

        public LeagueBuilder(ILeagueRules leagueRules)
        {
            _leagueRules = leagueRules; 
        }


    }
}
