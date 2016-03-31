using BusinessServices.Dtos;
using BusinessServices.Interfaces;
using Model;
using Model.Competitors;
using Model.Interfaces;
using Model.Record;
using Model.ReferenceData;
using System;
using System.Collections.Generic;

namespace BusinessServices.Sports
{
    public class FootballManager : ISportManager
    {
        public Competitor Competitor { get; set; }
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
            IFootballRecord footballRecord = Competitor;

            if (_competitionType.Name == EnumCompetitionType.PointsLeague.ToString())
            {
                footballRecord.Points += _points.Win;
                footballRecord.Played += 1;
                footballRecord.Wins += 1;
                footballRecord.For += winnerScore;
                footballRecord.Against += loserScore;
                footballRecord.Difference = footballRecord.For - footballRecord.Against;

                return;
            }

            if (_competitionType.Name == EnumCompetitionType.ChallengeLeague.ToString() || _competitionType.Name == EnumCompetitionType.Knockout.ToString())
            {
                footballRecord.Played += 1;
                footballRecord.Wins += 1;
                footballRecord.For += winnerScore;
                footballRecord.Against += loserScore;
                footballRecord.Difference = footballRecord.For - footballRecord.Against;

                return;
            }
        }

        public void AwardLoss(int winnerScore, int loserScore)
        {
            IFootballRecord footballRecord = Competitor;

            if (_competitionType.Name == EnumCompetitionType.PointsLeague.ToString())
            {
                footballRecord.Points += _points.Loss;
                footballRecord.Played += 1;
                footballRecord.Losses += 1;
                footballRecord.For += loserScore;
                footballRecord.Against += winnerScore;
                footballRecord.Difference = footballRecord.For - footballRecord.Against;

                return;
            }

            if (_competitionType.Name == EnumCompetitionType.ChallengeLeague.ToString() || _competitionType.Name == EnumCompetitionType.Knockout.ToString())
            {
                footballRecord.Played += 1;
                footballRecord.Losses += 1;
                footballRecord.For += loserScore;
                footballRecord.Against += winnerScore;
                footballRecord.Difference = footballRecord.For - footballRecord.Against;

                return;
            }
        }

        public void AwardDraw(int winnerScore, int loserScore)
        {
            IFootballRecord footballRecord = Competitor;

            if (_competitionType.Name == EnumCompetitionType.PointsLeague.ToString())
            {
                footballRecord.Points += _points.Draw;
                footballRecord.Played += 1;
                footballRecord.Draws += 1;
                footballRecord.For += loserScore;
                footballRecord.Against += winnerScore;
                footballRecord.Difference = footballRecord.For - footballRecord.Against;

                return;
            }

            if (_competitionType.Name == EnumCompetitionType.ChallengeLeague.ToString())
            {
                footballRecord.Played += 1;
                footballRecord.Draws += 1;
                footballRecord.For += loserScore;
                footballRecord.Against += winnerScore;
                footballRecord.Difference = footballRecord.For - footballRecord.Against;

                return;
            }
        }

        public void WriteCompetitorHistoryRecord()
        {
            CompetitorHistoryRecord history = new CompetitorHistoryRecord();
            history.Competitor = Competitor;
            history.Points = Competitor.Points;
            history.Played = Competitor.Played;
            history.Wins = Competitor.Wins;
            history.Draws = Competitor.Draws;
            history.Losses = Competitor.Losses;
            history.For = Competitor.For;
            history.Against = Competitor.Against;
            history.Difference = Competitor.Difference;
        }

        public List<string> GetColumnHeadings()
        {
            if (_competitionType.Name == EnumCompetitionType.PointsLeague.ToString())
            {
                return new List<string>() {
                    "P",
                    "W",
                    "D",
                    "L",
                    "F",
                    "A",
                    "GD",
                    "Pts"
                };
            }

            if (_competitionType.Name == EnumCompetitionType.ChallengeLeague.ToString())
            {
                return new List<string>() {
                    "P",
                    "W",
                    "D",
                    "L",
                    "F",
                    "A",
                    "GD"
                };
            }

            throw new Exception("Not able to determine a Competition type in order to return League Standings");
        }

        public List<int> GetColumnData(IFootballRecord competitor)
        {
            if (_competitionType.Name == EnumCompetitionType.PointsLeague.ToString())
            {
                return new List<int>() {
                    competitor.Played,
                    competitor.Wins,
                    competitor.Draws,
                    competitor.Losses,
                    competitor.For,
                    competitor.Against,
                    competitor.Difference,
                    competitor.Points
                };
            }

            if (_competitionType.Name == EnumCompetitionType.ChallengeLeague.ToString())
            {
                return new List<int>() {
                    competitor.Played,
                    competitor.Wins,
                    competitor.Draws,
                    competitor.Losses,
                    competitor.For,
                    competitor.Against,
                    competitor.Difference
                };
            }

            throw new Exception("Not able to determine a Competition type in order to return League Standings");
        }
    }
}
