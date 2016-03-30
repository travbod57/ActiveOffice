using BusinessServices.Dtos;
using BusinessServices.Interfaces;
using Model;
using Model.Interfaces;
using Model.Record;
using Model.ReferenceData;
using System;
using System.Collections.Generic;

namespace BusinessServices.Sports
{
    public class FootballManager : ISportManager
    {
        public CompetitorRecord CompetitorRecord { get; set; }
        private CompetitionType _competitionType { get; set; }
        private PointsDto _points { get; set; }

        public FootballManager(CompetitionType competitionType)
        {
            _competitionType = competitionType;
        }

        public FootballManager(CompetitionType competitionType, PointsDto points)
        {
            _competitionType = competitionType;
            _points = points;
        }

        public void AwardWin(int winnerScore, int loserScore)
        {
            IFootballRecord footballRecord = CompetitorRecord;

            if (_competitionType.Name == EnumCompetitionType.PointsLeague.ToString())
            {
                footballRecord.Points += _points.Win;
                footballRecord.Played += 1;
                footballRecord.Wins += 1;
                footballRecord.For += winnerScore;
                footballRecord.Against += loserScore;
                footballRecord.Difference = CompetitorRecord.For - CompetitorRecord.Against;

                return;
            }

            if (_competitionType.Name == EnumCompetitionType.ChallengeLeague.ToString() || _competitionType.Name == EnumCompetitionType.Knockout.ToString())
            {
                footballRecord.Played += 1;
                footballRecord.Wins += 1;
                footballRecord.For += winnerScore;
                footballRecord.Against += loserScore;
                footballRecord.Difference = CompetitorRecord.For - CompetitorRecord.Against;

                return;
            }
        }

        public void AwardLoss(int winnerScore, int loserScore)
        {
            IFootballRecord footballRecord = CompetitorRecord;

            if (_competitionType.Name == EnumCompetitionType.PointsLeague.ToString())
            {
                footballRecord.Points += _points.Loss;
                footballRecord.Played += 1;
                footballRecord.Losses += 1;
                footballRecord.For += loserScore;
                footballRecord.Against += winnerScore;
                footballRecord.Difference = CompetitorRecord.For - CompetitorRecord.Against;

                return;
            }

            if (_competitionType.Name == EnumCompetitionType.ChallengeLeague.ToString() || _competitionType.Name == EnumCompetitionType.Knockout.ToString())
            {
                footballRecord.Played += 1;
                footballRecord.Losses += 1;
                footballRecord.For += loserScore;
                footballRecord.Against += winnerScore;
                footballRecord.Difference = CompetitorRecord.For - CompetitorRecord.Against;

                return;
            }
        }

        public void AwardDraw(int winnerScore, int loserScore)
        {
            IFootballRecord footballRecord = CompetitorRecord;

            if (_competitionType.Name == EnumCompetitionType.PointsLeague.ToString())
            {
                footballRecord.Points += _points.Draw;
                footballRecord.Played += 1;
                footballRecord.Draws += 1;
                footballRecord.For += loserScore;
                footballRecord.Against += winnerScore;
                footballRecord.Difference = CompetitorRecord.For - CompetitorRecord.Against;

                return;
            }

            if (_competitionType.Name == EnumCompetitionType.ChallengeLeague.ToString())
            {
                footballRecord.Played += 1;
                footballRecord.Draws += 1;
                footballRecord.For += loserScore;
                footballRecord.Against += winnerScore;
                footballRecord.Difference = CompetitorRecord.For - CompetitorRecord.Against;

                return;
            }
        }

        public void WriteCompetitorHistoryRecord()
        {
            IFootballRecord footballHistory = new CompetitorHistoryRecord();
            footballHistory.Competitor = CompetitorRecord.Competitor;
            footballHistory.Points = CompetitorRecord.Points;
            footballHistory.Played = CompetitorRecord.Played;
            footballHistory.Wins = CompetitorRecord.Wins;
            footballHistory.Draws = CompetitorRecord.Draws;
            footballHistory.Losses = CompetitorRecord.Losses;
            footballHistory.For = CompetitorRecord.For;
            footballHistory.Against = CompetitorRecord.Against;
            footballHistory.Difference = CompetitorRecord.Difference;
        }
    }
}
