using Model.Record;
using System.Collections.Generic;

namespace BusinessServices.Interfaces
{
    public interface ISportManager
    {
        Dictionary<string, CompetitorRecord> CompetitorRecords { get; set; }
        void AwardWin(int winnerScore, int loserScore);
        void AwardDraw(int winnerScore, int loserScore);
        void AwardLoss(int winnerScore, int loserScore);
    }
}
