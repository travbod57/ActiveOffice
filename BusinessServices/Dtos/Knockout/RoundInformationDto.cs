using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Dtos.Knockout
{
    public class RoundInformationDto
    {
        public EnumRound Round { get; set; }
        public int MatchesForRound { get; set; }
        public int MatchesForRoundCount { get; set; }
    }
}
