using BusinessServices.Helpers;
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
        private Dictionary<string, CompetitorRecord> _winnerRecords;
        private Dictionary<string, CompetitorRecord> _loserRecords;
        private ISportManager _sportManager;

        public KnockoutManager(Knockout knockout, ISportManager sportManager)
        {
            _knockout = knockout;
            _sportManager = sportManager;
        }

        public void AwardWin(KnockoutMatch knockoutMatch, KnockoutCompetitor winner, KnockoutCompetitor loser, int winnerScore, int loserScore)
        {
            _winnerRecords = CompetitorRecordHelpers.GetCompetitorRecords(winner);
            _loserRecords = CompetitorRecordHelpers.GetCompetitorRecords(loser);

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

            _sportManager.CompetitorRecords = _winnerRecords;
            _sportManager.AwardWin(winnerScore, loserScore);

            _sportManager.CompetitorRecords = _loserRecords;
            _sportManager.AwardLoss(winnerScore, loserScore);

            // move onto next stage of knockout

            if (knockoutMatch.NextRoundMatch.CompetitorA == null)
                knockoutMatch.NextRoundMatch.CompetitorA = winner;
            else
                knockoutMatch.NextRoundMatch.CompetitorB = winner;

            // insert history record

            CompetitorRecordHelpers.WriteCompetitorHistoryRecords(winner, _winnerRecords);
            CompetitorRecordHelpers.WriteCompetitorHistoryRecords(loser, _loserRecords);
        }
    }
}
