using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BusinessServices.Builders;
using BusinessServices.Schedulers;
using Model.Leagues;
using Model.Actors;
using Model.Scheduling;
using System.Linq;
using BusinessServices.Dtos.League;
using BusinessServices.Enums;
using BusinessServices.Interfaces;
using DAL;
using Moq;
using BusinessServices;

namespace Test
{
    [TestClass]
    public class SchedulerTests
    {
        private Team _t1;
        private Team _t2;
        private Team _t3;
        private Team _t4;
        private Team _t5;
        private LeagueCreatorDto _leagueCreatorDto;
        private Mock<IUnitOfWork> _unitOfWork;
        private IAuditLogger _auditLogger;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _auditLogger = new AuditLogger(_unitOfWork.Object);

            _t1 = new Team() { Name = "West Ham" };
            _t2 = new Team() { Name = "Spurs" };
            _t3 = new Team() { Name = "Leicester" };
            _t4 = new Team() { Name = "Norwich" };
            _t5 = new Team() { Name = "Arsenal" };

            _leagueCreatorDto = new LeagueCreatorDto() { CanSidePlayMoreThanOncePerMatchDay = true, Occurrance = Occurrance.Daily, ScheduleType = ScheduleType.Scheduled, DayOfWeek = DayOfWeek.Saturday };
        }

        //[TestMethod]
        //public void Random_2_Sides_2_MatchUps()
        //{
        //    // Arrange
        //    List<Side> sides = new List<Side>() { _t1, _t2 };
        //    LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>("League 1", DateTime.Now, DateTime.Now.AddDays(30), 2, 2, sides, _auditLogger);

        //    PointsLeague newPointsLeague = new PointsLeague();
        //    PointsLeagueSorter sorter = new PointsLeagueSorter(newPointsLeague);
        //    NonMatchScheduler nonMatchScheduler = new NonMatchScheduler();
        //    LeagueBuilder<PointsLeague> leagueBuilder = new LeagueBuilder<PointsLeague>(newPointsLeague, sorter, nonMatchScheduler);
        //    PointsLeague pointsLeague = director.Construct(leagueBuilder);

        //    // Act

        //    RandomMatchScheduler randomMatchScheduler = new RandomMatchScheduler(pointsLeague, _leagueCreatorDto);
        //    randomMatchScheduler.Schedule();

        //    // Assert

        //    var leagueMatches = pointsLeague.LeagueMatches.ToList();

        //    Assert.IsTrue(leagueMatches.Count == 2);
        //}

        //[TestMethod]
        //public void Random_3_Sides_2_MatchUps()
        //{
        //    // Arrange
        //    List<Side> sides = new List<Side>() { _t1, _t2, _t3 };
        //    LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>("League 1", DateTime.Now, DateTime.Now.AddDays(30), 3, 2, sides, _auditLogger);

        //    PointsLeague newPointsLeague = new PointsLeague();
        //    PointsLeagueSorter sorter = new PointsLeagueSorter(newPointsLeague);
        //    NonMatchScheduler nonMatchScheduler = new NonMatchScheduler();
        //    LeagueBuilder<PointsLeague> leagueBuilder = new LeagueBuilder<PointsLeague>(newPointsLeague, sorter, nonMatchScheduler);
        //    PointsLeague pointsLeague = director.Construct(leagueBuilder);

        //    // Act

        //    RandomMatchScheduler randomMatchScheduler = new RandomMatchScheduler(pointsLeague, _leagueCreatorDto);
        //    randomMatchScheduler.Schedule();

        //    // Assert

        //    var leagueMatches = pointsLeague.LeagueMatches.ToList();

        //    Assert.IsTrue(leagueMatches.Count == 6);
        //}

        //[TestMethod]
        //public void Random_4_Sides_2_MatchUps()
        //{
        //    // Arrange
        //    List<Side> sides = new List<Side>() { _t1, _t2, _t3, _t4 };
        //    LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>("League 1", DateTime.Now, DateTime.Now.AddDays(30), 4, 2, sides, _auditLogger);

        //    PointsLeague newPointsLeague = new PointsLeague();
        //    PointsLeagueSorter sorter = new PointsLeagueSorter(newPointsLeague);
        //    NonMatchScheduler nonMatchScheduler = new NonMatchScheduler();
        //    LeagueBuilder<PointsLeague> leagueBuilder = new LeagueBuilder<PointsLeague>(newPointsLeague, sorter, nonMatchScheduler);
        //    PointsLeague pointsLeague = director.Construct(leagueBuilder);

        //    // Act

        //    RandomMatchScheduler randomMatchScheduler = new RandomMatchScheduler(pointsLeague, _leagueCreatorDto);
        //    randomMatchScheduler.Schedule();

        //    // Assert

        //    var leagueMatches = pointsLeague.LeagueMatches.ToList();

        //    Assert.IsTrue(leagueMatches.Count == 12);
        //}

        //[TestMethod]
        //public void Random_3_Sides_Matches_1_MatchUp()
        //{
        //    // Arrange
        //    List<Side> sides = new List<Side>() { _t1, _t2, _t3 };
        //    LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>("League 1", DateTime.Now, DateTime.Now.AddDays(30), 3, 1, sides);

        //    PointsLeague newPointsLeague = new PointsLeague();
        //    PointsLeagueSorter sorter = new PointsLeagueSorter(newPointsLeague);
        //    NonMatchScheduler nonMatchScheduler = new NonMatchScheduler();
        //    LeagueBuilder<PointsLeague> leagueBuilder = new LeagueBuilder<PointsLeague>(newPointsLeague, sorter, nonMatchScheduler);
        //    PointsLeague pointsLeague = director.Construct(leagueBuilder);

        //    // Act

        //    RandomMatchScheduler randomMatchScheduler = new RandomMatchScheduler();
        //    randomMatchScheduler.Schedule(pointsLeague);
        //    randomMatchScheduler.Shuffle();

        //    // Assert

        //    var leagueMatches = pointsLeague.LeagueMatches.ToList();

        //    Assert.IsTrue(leagueMatches.Count == 3);
        //}

        //[TestMethod]
        //public void Random_4_Sides_Matches_1_MatchUp()
        //{
        //    // Arrange
        //    List<Side> sides = new List<Side>() { _t1, _t2, _t3, _t4 };
        //    LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>("League 1", DateTime.Now, DateTime.Now.AddDays(30), 4, 1, sides);

        //    PointsLeague newPointsLeague = new PointsLeague();
        //    PointsLeagueSorter sorter = new PointsLeagueSorter(newPointsLeague);
        //    NonMatchScheduler nonMatchScheduler = new NonMatchScheduler();
        //    LeagueBuilder<PointsLeague> leagueBuilder = new LeagueBuilder<PointsLeague>(newPointsLeague, sorter, nonMatchScheduler);
        //    PointsLeague pointsLeague = director.Construct(leagueBuilder);

        //    // Act

        //    RandomMatchScheduler randomMatchScheduler = new RandomMatchScheduler();
        //    randomMatchScheduler.Schedule(pointsLeague);
        //    randomMatchScheduler.Shuffle();

        //    // Assert

        //    var leagueMatches = pointsLeague.LeagueMatches.ToList();

        //    Assert.IsTrue(leagueMatches.Count == 6);
        //}

        //[TestMethod]
        //public void Random_5_Sides_Matches_1_MatchUp()
        //{
        //    // Arrange

        //    List<Side> sides = new List<Side>() { _t1, _t2, _t3, _t4, _t5 };
        //    LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>("League 1", DateTime.Now, DateTime.Now.AddDays(30), 5, 1, sides);

        //    PointsLeague newPointsLeague = new PointsLeague();
        //    PointsLeagueSorter sorter = new PointsLeagueSorter(newPointsLeague);
        //    NonMatchScheduler nonMatchScheduler = new NonMatchScheduler();
        //    LeagueBuilder<PointsLeague> leagueBuilder = new LeagueBuilder<PointsLeague>(newPointsLeague, sorter, nonMatchScheduler);
        //    PointsLeague pointsLeague = director.Construct(leagueBuilder);

        //    // Act

        //    RandomMatchScheduler randomMatchScheduler = new RandomMatchScheduler();
        //    randomMatchScheduler.Schedule(pointsLeague);
        //    randomMatchScheduler.Shuffle();

        //    // Assert

        //    var leagueMatches = pointsLeague.LeagueMatches.ToList();

        //    Assert.IsTrue(leagueMatches.Count == 10);
        //    Assert.IsTrue(leagueMatches[0].CompetitorA.Side.Name == "West Ham" && leagueMatches[0].CompetitorB.Side.Name == "Spurs");
        //    Assert.IsTrue(leagueMatches[1].CompetitorA.Side.Name == "West Ham" && leagueMatches[1].CompetitorB.Side.Name == "Leicester");
        //    Assert.IsTrue(leagueMatches[2].CompetitorA.Side.Name == "West Ham" && leagueMatches[2].CompetitorB.Side.Name == "Norwich");
        //    Assert.IsTrue(leagueMatches[3].CompetitorA.Side.Name == "West Ham" && leagueMatches[3].CompetitorB.Side.Name == "Arsenal");
        //    Assert.IsTrue(leagueMatches[4].CompetitorA.Side.Name == "Spurs" && leagueMatches[4].CompetitorB.Side.Name == "Leicester");
        //    Assert.IsTrue(leagueMatches[5].CompetitorA.Side.Name == "Spurs" && leagueMatches[5].CompetitorB.Side.Name == "Norwich");
        //    Assert.IsTrue(leagueMatches[6].CompetitorA.Side.Name == "Spurs" && leagueMatches[6].CompetitorB.Side.Name == "Arsenal");
        //    Assert.IsTrue(leagueMatches[7].CompetitorA.Side.Name == "Leicester" && leagueMatches[7].CompetitorB.Side.Name == "Norwich");
        //    Assert.IsTrue(leagueMatches[8].CompetitorA.Side.Name == "Leicester" && leagueMatches[8].CompetitorB.Side.Name == "Arsenal");
        //    Assert.IsTrue(leagueMatches[9].CompetitorA.Side.Name == "Norwich" && leagueMatches[9].CompetitorB.Side.Name == "Arsenal");
        //}
    }
}
