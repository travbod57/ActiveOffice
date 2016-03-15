using Model.Record;
using System.Collections.Generic;

namespace BusinessServices.Interfaces
{
    public interface ISportManager
    {
        Dictionary<string, CompetitorRecord> CompetitorRecords { get; set; }
        void AwardWin(int goalsFor, int goalsAgainst);
        void AwardDraw(int goalsFor, int goalsAgainst);
        void AwardLoss(int goalsFor, int goalsAgainst);
    }
}
