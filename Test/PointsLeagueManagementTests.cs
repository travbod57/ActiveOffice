using BusinessServices;
using BusinessServices.Builders;
using BusinessServices.Dtos;
using BusinessServices.Dtos.League;
using BusinessServices.Enums;
using BusinessServices.Interfaces;
using BusinessServices.Managers.LeagueCompetition;
using BusinessServices.Schedulers;
using BusinessServices.Sports;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.Actors;
using Model.Competitors;
using Model.Leagues;
using Model.ReferenceData;
using Model.Schedule;
using Model.Sports;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    [TestClass]
    public class PointsLeagueManagementTests
    {
        /* TODO: 
         * Match scheduling based on number of face offs
         * Match scheduling to do home and away macth ups etc
        */

        private PointsLeague _pointsLeague;
        private LeagueCreatorDto _leagueCreatorDto;
        private Mock<IUnitOfWork> _unitOfWork;
        private IAuditLogger _auditLogger;
        private List<Side> _sides;
        private List<SportColumn> _footballSportColumns;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _auditLogger = new AuditLogger(_unitOfWork.Object);
            _unitOfWork.Setup(x => x.GetRepository<Audit>().All());

            Team t1 = new Team() { Name = "West Ham" };
            Team t2 = new Team() { Name = "Spurs" };
            Team t3 = new Team() { Name = "Leicester" };
            Team t4 = new Team() { Name = "Norwich" };
            Team t5 = new Team() { Name = "Arsenal" };

            _sides = new List<Side>() { t1, t2, t3, t4, t5 };

            _footballSportColumns = new List<SportColumn>()
            {
                new SportColumn() { Id = 1, Name = "Played" },
                new SportColumn() { Id = 2, Name = "Points" },
                new SportColumn() { Id = 3, Name = "Wins" },
                new SportColumn() { Id = 4, Name = "Draws" },
                new SportColumn() { Id = 5, Name = "Losses" },
                new SportColumn() { Id = 6, Name = "GoalsFor" },
                new SportColumn() { Id = 7, Name = "GoalsAgainst" },
                new SportColumn() { Id = 8, Name = "GoalDifference" }
            };

            _leagueCreatorDto = new LeagueCreatorDto() { NumberOfCompetitors = 5, CanSidePlayMoreThanOncePerMatchDay = true, Occurrance = Occurrance.Daily, ScheduleType = ScheduleType.Scheduled, DayOfWeek = DayOfWeek.Saturday };

            // Act

            LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>("League 1", DateTime.Now, DateTime.Now.AddDays(30), 5, 4, _sides, _auditLogger, _footballSportColumns);

            PointsLeague newPointsLeague = new PointsLeague() { PointsForWin = 3, PointsForDraw = 1, PointsForLoss = 0, CompetitionType = new CompetitionType() { Id = 1, Name = "PointsLeague" }};
            PointsLeagueSorter sorter = new PointsLeagueSorter(newPointsLeague);
            RandomLeagueMatchScheduler scheduler = new RandomLeagueMatchScheduler(newPointsLeague, _leagueCreatorDto);

            LeagueBuilder<PointsLeague> b1 = new LeagueBuilder<PointsLeague>(newPointsLeague, sorter, scheduler);

            _pointsLeague = director.Construct(b1);
        }

        [TestMethod]
        public void Award_Win_Football()
        {
            // Arrange
            LeagueMatch leagueMatch = _pointsLeague.LeagueMatches.First();

            LeagueCompetitor winner = (LeagueCompetitor)leagueMatch.CompetitorA;
            leagueMatch.CompetitorAScore = 2;
            LeagueCompetitor loser = (LeagueCompetitor)leagueMatch.CompetitorB;
            leagueMatch.CompetitorBScore = 1;

            ISportManager footballManager = new FootballManager(_pointsLeague.CompetitionType, new PointsDto() { Win = _pointsLeague.PointsForWin, Draw = _pointsLeague.PointsForDraw, Loss = _pointsLeague.PointsForLoss });

            PointsLeagueManager manager = new PointsLeagueManager(_pointsLeague, footballManager);

            // Act

            manager.AwardWin(leagueMatch, winner, loser);

            // Assert

            Assert.IsTrue(winner.CompetitorRecords.Single( cr => cr.SportColumn.Name == "Points").Value == 3);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Played").Value == 1);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Wins").Value == 1);             
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Draws").Value == 0);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Losses").Value == 0);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsFor").Value == 2);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsAgainst").Value == 1);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalDifference").Value == 1); 

            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Points").Value == 0);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Played").Value == 1);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Wins").Value == 0);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Draws").Value == 0);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Losses").Value == 1);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsFor").Value == 1);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsAgainst").Value == 2);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalDifference").Value == -1); 
        }

        [TestMethod]
        public void Get_League_Standings()
        {
            // Arrange

            LeagueMatch leagueMatch = _pointsLeague.LeagueMatches.First();

            LeagueCompetitor winner = (LeagueCompetitor)leagueMatch.CompetitorA;
            leagueMatch.CompetitorAScore = 2;
            LeagueCompetitor loser = (LeagueCompetitor)leagueMatch.CompetitorB;
            leagueMatch.CompetitorBScore = 1;

            ISportManager footballManager = new FootballManager(_pointsLeague.CompetitionType, new PointsDto() { Win = _pointsLeague.PointsForWin, Draw = _pointsLeague.PointsForDraw, Loss = _pointsLeague.PointsForLoss });

            PointsLeagueManager manager = new PointsLeagueManager(_pointsLeague, footballManager);

            // Act

            manager.AwardWin(leagueMatch, winner, loser);
            List<LeagueTableRowDto> standings = manager.GetLeagueStandings();

            // Assert

            Assert.IsTrue(standings[0].ColumnValues.Single( x => x.Item1 == "Points").Item2 == 3);
            Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "Wins").Item2 == 1);
            Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "Draws").Item2 == 0);
            Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "Losses").Item2 == 0);
            Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "GoalsFor").Item2 == 2);
            Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "GoalsAgainst").Item2 == 1);
            Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "GoalDifference").Item2 == 1);
            Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "Played").Item2 == 1);

            Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "Points").Item2 == 0);
            Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "Wins").Item2 == 0);
            Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "Draws").Item2 == 0);
            Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "Losses").Item2 == 1);
            Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "GoalsFor").Item2 == 1);
            Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "GoalsAgainst").Item2 == 2);
            Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "GoalDifference").Item2 == -1);
            Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "Played").Item2 == 1);
        }

        // TODO: Test League Standings more with more results
    }
}
