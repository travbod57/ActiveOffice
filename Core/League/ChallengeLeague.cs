using Core.LeagueRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.League
{
    public class ChallengeLeague : League
    {
        public ChallengeLeague(int numberOfPositions, int challengeLimit) : base(numberOfPositions)
	    {
            leagueRules = new ChallengeLeagueRules();
            ChallengeLimit = challengeLimit;
            Challenges = new List<Challenge>();
	    }

        public List<Challenge> Challenges { get; set; }

        public void Challenge(LeagueCompetitor challenger, LeagueCompetitor defender)
        {
            if (challenger.Position.Number >= defender.Position.Number)
                throw new Exception("Invalid Challenge - the challenger is already above the challenger");

            if (defender.Position.Number + ChallengeLimit < challenger.Position.Number)
                throw new Exception("Invalid Challenge - the challenger is not positioned within reach of the defender");

            Challenges.Add(new Challenge(challenger, defender));
        }

        public int ChallengeLimit { get; set; }
    }
}
