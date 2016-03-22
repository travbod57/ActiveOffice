﻿using BusinessServices;
using BusinessServices.Builders;
using BusinessServices.Builders.LeagueCompetition;
using BusinessServices.Dtos.League;
using BusinessServices.Enums;
using BusinessServices.Interfaces;
using BusinessServices.Schedulers;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.Actors;
using Model.Leagues;
using Model.Schedule;
using Model.Scheduling;
using Model.Sports;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.PointsLeagueCompetition
{
    [TestClass]
    public class PointsLeagueCreationTests
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
        private List<SportColumn> _goKartingSportColumns;

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
                new SportColumn() { Id = 7, Name = "GoalsAgainst" }
            };

            _goKartingSportColumns = new List<SportColumn>()
            {
                new SportColumn() { Id = 1, Name = "Points" },
                new SportColumn() { Id = 2, Name = "Wins" },
                new SportColumn() { Id = 3, Name = "Races" },
                new SportColumn() { Id = 4, Name = "Laps" }
            };
        }

        [TestMethod]
        public void Create_Football_Points_League()
        {
            // Arrange

            _leagueCreatorDto = new LeagueCreatorDto() { NumberOfCompetitors = 5, CanSidePlayMoreThanOncePerMatchDay = true, Occurrance = Occurrance.Daily, ScheduleType = ScheduleType.Scheduled, DayOfWeek = DayOfWeek.Saturday };

            LeagueConfig leagueConfig = new LeagueConfig()
            {
                Name = "League 1",
                NumberOfMatchUps = 4,
                NumberOfPositions = 5,
                Sides = _sides,
                SportColumns = _footballSportColumns,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                AuditLogger = _auditLogger
            };

            // Act

            LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>(leagueConfig);

            PointsLeague newPointsLeague = new PointsLeague();
            RandomLeagueMatchScheduler scheduler = new RandomLeagueMatchScheduler(newPointsLeague, _leagueCreatorDto);

            LeagueBuilder<PointsLeague> b1 = new LeagueBuilder<PointsLeague>(newPointsLeague, scheduler);

            _pointsLeague = director.Construct(b1);

            // Assert

            // correct number of sides in league
            Assert.IsTrue(_pointsLeague.LeagueCompetitors.Count == _sides.Count);

            // correct number of competitor records for the number of sides
            Assert.IsTrue(_pointsLeague.LeagueCompetitors.SelectMany(x => x.CompetitorRecords).Count() == _footballSportColumns.Count * _sides.Count);

            // correct number of matches got created
            Assert.IsTrue(_pointsLeague.LeagueMatches.Count == scheduler.TotalNumberOfMatches);
        }

        [TestMethod]
        public void Create_GoKart_Points_League()
        {
            // Arrange

            _leagueCreatorDto = new LeagueCreatorDto() { NumberOfCompetitors = 5, CanSidePlayMoreThanOncePerMatchDay = true, Occurrance = Occurrance.Daily, ScheduleType = ScheduleType.Scheduled, DayOfWeek = DayOfWeek.Saturday };

            LeagueConfig leagueConfig = new LeagueConfig()
            {
                Name = "League 1",
                NumberOfMatchUps = 4,
                NumberOfPositions = 5,
                Sides = _sides,
                SportColumns = _goKartingSportColumns,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                AuditLogger = _auditLogger
            };

            // Act

            LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>(leagueConfig);

            PointsLeague newPointsLeague = new PointsLeague();
            RandomLeagueMatchScheduler scheduler = new RandomLeagueMatchScheduler(newPointsLeague, _leagueCreatorDto);

            LeagueBuilder<PointsLeague> b1 = new LeagueBuilder<PointsLeague>(newPointsLeague, scheduler);

            _pointsLeague = director.Construct(b1);

            // Assert

            // correct number of sides in league
            Assert.IsTrue(_pointsLeague.LeagueCompetitors.Count == _sides.Count);

            // correct number of competitor records for the number of sides
            Assert.IsTrue(_pointsLeague.LeagueCompetitors.SelectMany(x => x.CompetitorRecords).Count() == _goKartingSportColumns.Count * _sides.Count);

            // correct number of matches got created
            Assert.IsTrue(_pointsLeague.LeagueMatches.Count == scheduler.TotalNumberOfMatches);
        }
    }
}
