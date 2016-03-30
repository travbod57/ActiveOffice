using BusinessServices.Dtos;
using DAL;
using Model.Competitors;
using Model.Leagues;
using Model.Record;
using Model.ReferenceData;
using Model.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using Model.Extensions;
using BusinessServices.Interfaces;
using Model;
using Model.Actors;
using BusinessServices.Builders.LeagueCompetition;
using BusinessServices.Dtos.League;

namespace BusinessServices.Managers.LeagueCompetition
{
    public abstract class LeagueManagerBase
    {
        private League _league { get; set; }
        protected ISportManager SportManager;

        public LeagueManagerBase(League league, ISportManager sportManager)
        {
            _league = league;
            SportManager = sportManager;
        }

        public void AddMatch(LeagueCompetitor competitorA, LeagueCompetitor competitorB)
        {
            _league.LeagueMatches.Add(new LeagueMatch() { CompetitorA = competitorA, CompetitorB = competitorB, MatchState = EnumMatchState.Unscheduled });
        }

        public virtual void AwardWin(LeagueMatch leagueMatch, LeagueCompetitor winner, LeagueCompetitor loser)
        {
            // update competitor record

            int winningScore = leagueMatch.GetWinningScore();
            int losingScore = leagueMatch.GetLosingScore();

            SportManager.CompetitorRecord = winner.CompetitorRecord;
            SportManager.AwardWin(winningScore, losingScore);
            SportManager.WriteCompetitorHistoryRecord();

            SportManager.CompetitorRecord = loser.CompetitorRecord;
            SportManager.AwardLoss(winningScore, losingScore);
            SportManager.WriteCompetitorHistoryRecord();

            // update match result

            leagueMatch.Winner = winner;
            leagueMatch.Loser = loser;
            leagueMatch.MatchState = EnumMatchState.Played;
        }

        public virtual void AwardDraw(LeagueMatch leagueMatch, LeagueCompetitor competitorA, LeagueCompetitor competitorB)
        {
            // update competitor record

            int drawScore = leagueMatch.GetDrawScore();

            SportManager.CompetitorRecord = competitorA.CompetitorRecord;
            SportManager.AwardDraw(drawScore, drawScore);
            SportManager.WriteCompetitorHistoryRecord();

            SportManager.CompetitorRecord = competitorB.CompetitorRecord;
            SportManager.AwardDraw(drawScore, drawScore);
            SportManager.WriteCompetitorHistoryRecord();

            // update match result

            leagueMatch.IsDraw = true;
            leagueMatch.MatchState = EnumMatchState.Played;
        }

        public abstract List<LeagueTableRowDto> GetLeagueStandings();

        public abstract void RenewLeague();

        public void Finalise()
        {
            if (_league.Cluster != null)
            {
                int numberOfRelegationPositions = _league.NumberOfRelegationPositions;
                int numberOfPromotionPositions = _league.NumberOfRelegationPositions;

                foreach (var competitor in _league.LeagueCompetitors)
	            {
                    if (numberOfPromotionPositions > 0)
                    {
                        competitor.IsRelegated = true;
                        numberOfPromotionPositions--;
                    }

                    if (numberOfRelegationPositions > 0)
                    {
                        competitor.IsPromoted = true;
                        numberOfRelegationPositions--;
                    }
	            }
            }
        }
    }
}
