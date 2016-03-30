using BusinessServices.Interfaces;
using Model;
using Model.Competitors;
using Model.Knockouts;
using Model.Record;
using Model.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Managers.KnockoutCompetition
{
    public class KnockoutManager
    {
        private Knockout _knockout;
        private ISportManager _sportManager;

        public KnockoutManager(Knockout knockout, ISportManager sportManager)
        {
            _knockout = knockout;
            _sportManager = sportManager;
        }

        public void AwardWin(KnockoutMatch knockoutMatch, KnockoutCompetitor winner, KnockoutCompetitor loser, int winnerScore, int loserScore)
        {
            knockoutMatch.Winner = winner;
            knockoutMatch.MatchState = EnumMatchState.Played;
            knockoutMatch.Loser = loser;

            if (knockoutMatch.CompetitorA == winner)
            {
                knockoutMatch.CompetitorAScore = winnerScore;
                knockoutMatch.CompetitorBScore = loserScore;
            }
            else
            {
                knockoutMatch.CompetitorAScore = loserScore;
                knockoutMatch.CompetitorBScore = winnerScore;
            }

            _sportManager.CompetitorRecord = winner.CompetitorRecord;
            _sportManager.AwardWin(winnerScore, loserScore);
            _sportManager.WriteCompetitorHistoryRecord();

            _sportManager.CompetitorRecord = loser.CompetitorRecord;
            _sportManager.AwardLoss(winnerScore, loserScore);
            _sportManager.WriteCompetitorHistoryRecord();

            // move onto next stage of knockout

            if (knockoutMatch.NextRoundMatch.CompetitorA == null)
                knockoutMatch.NextRoundMatch.CompetitorA = winner;
            else
                knockoutMatch.NextRoundMatch.CompetitorB = winner;

            // move loser to third place playoff
            if (knockoutMatch.AlternativeNextRoundMatch != null)
            {
                if (knockoutMatch.AlternativeNextRoundMatch.CompetitorA == null)
                    knockoutMatch.AlternativeNextRoundMatch.CompetitorA = loser;
                else
                    knockoutMatch.AlternativeNextRoundMatch.CompetitorB = loser;
            }
        }
    }
}
