using Model.Competitors;
using Model.Interfaces;
using Model.Record;
using System.Collections.Generic;

namespace BusinessServices.Interfaces
{
    public interface ISportManager
    {
        Competitor Competitor { get; set; }
        void AwardWin(int winnerScore, int loserScore);
        void AwardDraw(int winnerScore, int loserScore);
        void AwardLoss(int winnerScore, int loserScore);
        void WriteCompetitorHistoryRecord();
        List<string> GetColumnHeadings();
        List<int> GetColumnData(IFootballRecord competitor);
    }
}
