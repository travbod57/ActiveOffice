﻿using BusinessServices;
using BusinessServices.Builders;
using BusinessServices.Builders.LeagueCompetition;
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
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.PointsLeagueCompetition
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

            _leagueCreatorDto = new LeagueCreatorDto() { NumberOfCompetitors = 5, CanSidePlayMoreThanOncePerMatchDay = true, Occurrance = Occurrance.Daily, ScheduleType = ScheduleType.Scheduled, DayOfWeek = DayOfWeek.Saturday };

            LeagueConfig leagueConfig = new LeagueConfig()
            {
                Name = "League 1",
                NumberOfMatchUps = 4,
                NumberOfPositions = 5,
                Sides = _sides,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                AuditLogger = _auditLogger
            };

            // Act

            LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>(leagueConfig);

            PointsLeague newPointsLeague = new PointsLeague() { PointsForWin = 3, PointsForDraw = 1, PointsForLoss = 0, CompetitionType = new CompetitionType() { Id = 1, Name = "PointsLeague" }};
            RandomLeagueMatchScheduler scheduler = new RandomLeagueMatchScheduler(newPointsLeague, _leagueCreatorDto);

            LeagueBuilder<PointsLeague> b1 = new LeagueBuilder<PointsLeague>(newPointsLeague, scheduler);

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

            Assert.IsTrue(winner.Points == 3);
            Assert.IsTrue(winner.Played == 1);
            Assert.IsTrue(winner.Wins == 1);
            Assert.IsTrue(winner.Draws == 0);
            Assert.IsTrue(winner.Losses == 0);
            Assert.IsTrue(winner.For == 2);
            Assert.IsTrue(winner.Against == 1);
            Assert.IsTrue(winner.Difference == 1);

            Assert.IsTrue(loser.Points == 0);
            Assert.IsTrue(loser.Played == 1);
            Assert.IsTrue(loser.Wins == 0);
            Assert.IsTrue(loser.Draws == 0);
            Assert.IsTrue(loser.Losses == 1);
            Assert.IsTrue(loser.For == 1);
            Assert.IsTrue(loser.Against == 2);
            Assert.IsTrue(loser.Difference == -1); 
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

            //Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "Points").Item2 == 3);
            //Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "Wins").Item2 == 1);
            //Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "Draws").Item2 == 0);
            //Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "Losses").Item2 == 0);
            //Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "GoalsFor").Item2 == 2);
            //Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "GoalsAgainst").Item2 == 1);
            //Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "GoalDifference").Item2 == 1);
            //Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "Played").Item2 == 1);

            //Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "Points").Item2 == 0);
            //Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "Wins").Item2 == 0);
            //Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "Draws").Item2 == 0);
            //Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "Losses").Item2 == 1);
            //Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "GoalsFor").Item2 == 1);
            //Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "GoalsAgainst").Item2 == 2);
            //Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "GoalDifference").Item2 == -1);
            //Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "Played").Item2 == 1);
        }

        // TODO: Test League Standings more with more results
    }
}
