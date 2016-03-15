using BusinessServices.Dtos;
using BusinessServices.Interfaces;
using Model;
using Model.Record;
using Model.ReferenceData;
using System.Collections.Generic;

namespace BusinessServices.Sports
{
    public class FootballManager : ISportManager
    {
        public Dictionary<string, CompetitorRecord> CompetitorRecords { get; set; }
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

        public void AwardWin(int goalsFor, int goalsAgainst)
        {
            if (_competitionType.Name == EnumCompetitionType.PointsLeague.ToString())
            {
                CompetitorRecords["Points"].Value += _points.Win;
                CompetitorRecords["Played"].Value += 1;
                CompetitorRecords["Wins"].Value += 1;
                CompetitorRecords["GoalsFor"].Value += goalsFor;
                CompetitorRecords["GoalsAgainst"].Value += goalsAgainst;
                CompetitorRecords["GoalDifference"].Value = goalsFor - goalsAgainst;
                return;
            }

            if (_competitionType.Name == EnumCompetitionType.ChallengeLeague.ToString())
            {
                CompetitorRecords["Played"].Value += 1;
                CompetitorRecords["Wins"].Value += 1;
                CompetitorRecords["GoalsFor"].Value += goalsFor;
                CompetitorRecords["GoalsAgainst"].Value += goalsAgainst;
                CompetitorRecords["GoalDifference"].Value = goalsFor - goalsAgainst;
                return;
            }
        }

        public void AwardLoss(int goalsFor, int goalsAgainst)
        {
            if (_competitionType.Name == EnumCompetitionType.PointsLeague.ToString())
            {
                CompetitorRecords["Points"].Value += _points.Loss;
                CompetitorRecords["Played"].Value += 1;
                CompetitorRecords["Losses"].Value += 1;
                CompetitorRecords["GoalsFor"].Value += goalsFor;
                CompetitorRecords["GoalsAgainst"].Value += goalsAgainst;
                CompetitorRecords["GoalDifference"].Value = goalsFor - goalsAgainst;
                return;
            }

            if (_competitionType.Name == EnumCompetitionType.ChallengeLeague.ToString())
            {
                CompetitorRecords["Played"].Value += 1;
                CompetitorRecords["Losses"].Value += 1;
                CompetitorRecords["GoalsFor"].Value += goalsFor;
                CompetitorRecords["GoalsAgainst"].Value += goalsAgainst;
                CompetitorRecords["GoalDifference"].Value = goalsFor - goalsAgainst;
                return;
            }
        }

        public void AwardDraw(int goalsFor, int goalsAgainst)
        {
            if (_competitionType.Name == EnumCompetitionType.PointsLeague.ToString())
            {
                CompetitorRecords["Points"].Value += _points.Draw;
                CompetitorRecords["Played"].Value += 1;
                CompetitorRecords["Draws"].Value += 1;
                CompetitorRecords["GoalsFor"].Value += goalsFor;
                CompetitorRecords["GoalsAgainst"].Value += goalsAgainst;
                CompetitorRecords["GoalDifference"].Value = goalsFor - goalsAgainst;
                return;
            }

            if (_competitionType.Name == EnumCompetitionType.ChallengeLeague.ToString())
            {
                CompetitorRecords["Points"].Value += _points.Draw;
                CompetitorRecords["Played"].Value += 1;
                CompetitorRecords["Draws"].Value += 1;
                CompetitorRecords["GoalsFor"].Value += goalsFor;
                CompetitorRecords["GoalsAgainst"].Value += goalsAgainst;
                CompetitorRecords["GoalDifference"].Value = goalsFor - goalsAgainst;
                return;
            }
        }
    }
}
