using Model.Leagues;
using Model.ReferenceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Profile.Dtos
{
    public class PlayerOverviewProfileDto
    {
        public int NumberOfActiveLeagues { get; set; }
        public League HighestRankedInLeague { get; set; }

        public int NumberOfLeaguesWon { get; set; }
        public int NumberOfTournamentsWon { get; set; }
        public int NumberOfKnockoutsWon { get; set; }

        public IEnumerable<SportType> SportsParticipatingIn { get; set; }
        public SportType BestPerformingSport { get; set; }
    }

    public class PlayerSportProfileDto
    {
        public SportType SportType { get; set; }
        public int Played { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }

        public decimal WinPercentage { get; set; }
    }
}
