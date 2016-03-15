using Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Extensions
{
    public static class ModelExtensions
    {
        public static int GetWinningScore(this Match match)
        {
            return match.CompetitorAScore > match.CompetitorBScore ? match.CompetitorAScore : match.CompetitorBScore;
        }

        public static int GetLosingScore(this Match match)
        {
            return match.CompetitorAScore < match.CompetitorBScore ? match.CompetitorAScore : match.CompetitorBScore;
        }

        public static int GetDrawScore(this Match match)
        {
            return match.CompetitorAScore == match.CompetitorBScore ? match.CompetitorAScore : -1;
        }
    }
}
