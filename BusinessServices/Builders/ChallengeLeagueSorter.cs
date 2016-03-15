using BusinessServices.Interfaces;
using Model.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Builders
{
    public class ChallengeLeagueSorter : LeagueSorterBase, ILeagueSorter
    {
       private ChallengeLeague _challengeLeague { get; set; }

       public ChallengeLeagueSorter(ChallengeLeague challengeLeague)
        {
            _challengeLeague = challengeLeague;
        }

        public void UpdatePosition()
        {

        }
    }
}
