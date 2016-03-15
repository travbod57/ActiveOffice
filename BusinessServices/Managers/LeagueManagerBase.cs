using BusinessServices.Dtos;
using BusinessServices.Helpers;
using DAL;
using Model.Competitors;
using Model.Leagues;
using Model.Record;
using Model.ReferenceData;
using Model.Schedule;
using Model.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using Model.Extensions;
using BusinessServices.Interfaces;

namespace BusinessServices.Updaters
{
    public abstract class LeagueManagerBase
    {
        private League _league { get; set; }
        protected ISportManager SportManager;
        protected Dictionary<string, CompetitorRecord> WinnerRecords;
        protected Dictionary<string, CompetitorRecord> LoserRecords;

        public LeagueManagerBase(League league, ISportManager sportManager)
        {
            _league = league;
            SportManager = sportManager;
        }

        public void AddMatch(Competitor competitorA, Competitor competitorB)
        {
            _league.LeagueMatches.Add(new LeagueMatch() { CompetitorA = competitorA, CompetitorB = competitorB });
        }

        public virtual void AwardWin(LeagueMatch leagueMatch, Competitor winner, Competitor loser)
        {
            WinnerRecords = CompetitorRecordHelpers.GetCompetitorRecords(winner);
            LoserRecords = CompetitorRecordHelpers.GetCompetitorRecords(loser);

            // update competitor record

            int winningScore = leagueMatch.GetWinningScore();
            int losingScore = leagueMatch.GetLosingScore();

            SportManager.CompetitorRecords = WinnerRecords;
            SportManager.AwardWin(winningScore, losingScore);

            SportManager.CompetitorRecords = LoserRecords;
            SportManager.AwardLoss(losingScore, winningScore);

            // update match result

            leagueMatch.Winner = winner;
            leagueMatch.Loser = loser;

            // insert history record

            CompetitorRecordHelpers.WriteCompetitorHistoryRecords(winner, WinnerRecords);
            CompetitorRecordHelpers.WriteCompetitorHistoryRecords(loser, LoserRecords);
        }

        public virtual void AwardDraw(LeagueMatch leagueMatch, Competitor competitorA, Competitor competitorB)
        {
            Dictionary<string, CompetitorRecord> competitorARecords = CompetitorRecordHelpers.GetCompetitorRecords(competitorA);
            Dictionary<string, CompetitorRecord> competitorBRecords = CompetitorRecordHelpers.GetCompetitorRecords(competitorB);

            // update competitor record

            int drawScore = leagueMatch.GetDrawScore();

            SportManager.CompetitorRecords = competitorARecords;
            SportManager.AwardDraw(drawScore, drawScore);

            SportManager.CompetitorRecords = competitorBRecords;
            SportManager.AwardDraw(drawScore, drawScore);

            // update match result

            leagueMatch.IsDraw = true;

            // insert history record

            CompetitorRecordHelpers.WriteCompetitorHistoryRecords(competitorA, competitorARecords);
            CompetitorRecordHelpers.WriteCompetitorHistoryRecords(competitorB, competitorBRecords);
        }

        public virtual List<LeagueTableRowDto> GetLeagueStandings()
        {
            List<LeagueTableRowDto> rows = new List<LeagueTableRowDto>();

            foreach (var competitor in _league.LeagueCompetitors)
            {
                LeagueTableRowDto row = new LeagueTableRowDto();
                row.SideName = competitor.Side.Name;

                foreach (var competitorRecord in CompetitorRecordHelpers.GetCompetitorRecords(competitor))
                {
                    row.ColumnValues.Add(Tuple.Create(competitorRecord.Key, competitorRecord.Value.Value));
                }

                rows.Add(row);
            }

            return rows;
        }
    }
}
