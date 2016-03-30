using Model.Record;
using System.Collections.Generic;

namespace BusinessServices.Interfaces
{
    public interface ISportManager
    {
        CompetitorRecord CompetitorRecord { get; set; }
        void AwardWin(int winnerScore, int loserScore);
        void AwardDraw(int winnerScore, int loserScore);
        void AwardLoss(int winnerScore, int loserScore);
        void WriteCompetitorHistoryRecord();
    }
}
