using Model.Competitors;
using Model.Record;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessServices.Helpers
{
    public static class CompetitorRecordHelpers
    {
        public static Dictionary<string, CompetitorRecord> GetCompetitorRecords(Competitor competitor)
        {
            return competitor.CompetitorRecords.ToDictionary(cr => cr.SportColumn.Name);
        }

        public static void WriteCompetitorHistoryRecords(Competitor competitor, Dictionary<string, CompetitorRecord> competitorRecords)
        {
            DateTime now = DateTime.Now;

            foreach (var competitorRecord in competitorRecords)
            {
                competitor.CompetitorHistoryRecords.Add(new CompetitorHistoryRecord()
                {
                    Competitor = competitor,
                    SportColumn = competitorRecord.Value.SportColumn,
                    Value = competitorRecord.Value.Value,
                    TimeStamp = now
                });
            }
        }
    }
}
