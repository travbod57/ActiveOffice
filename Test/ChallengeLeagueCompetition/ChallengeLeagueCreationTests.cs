using BusinessServices;
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
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.ChallengeLeagueCompetition
{
    [TestClass]
    public class ChallengeLeagueCreationTests
    {
        /* TODO: 
         * Match scheduling based on number of face offs
         * Match scheduling to do home and away macth ups etc
        */

        private ChallengeLeague _challengeLeague;
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
        }

        // TODO: tests for schedule exceptions

        [TestMethod]
        public void Create_Football_Challenge_League()
        {
            // Arrange

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

            LeagueBuilderDirector<ChallengeLeague> director = new LeagueBuilderDirector<ChallengeLeague>(leagueConfig);

            ChallengeLeague newChallengeLeague = new ChallengeLeague();
            NonMatchScheduler scheduler = new NonMatchScheduler();

            LeagueBuilder<ChallengeLeague> b1 = new LeagueBuilder<ChallengeLeague>(newChallengeLeague, scheduler);

            _challengeLeague = director.Construct(b1);

            // Assert

            // correct number of sides in league
            Assert.IsTrue(_challengeLeague.LeagueCompetitors.Count == _sides.Count);

            // correct number of matches got created
            Assert.IsTrue(_challengeLeague.LeagueMatches.Count == scheduler.TotalNumberOfMatches);
        }

        [TestMethod]
        public void Create_GoKart_Challenge_League()
        {
            // Arrange

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

            LeagueBuilderDirector<ChallengeLeague> director = new LeagueBuilderDirector<ChallengeLeague>(leagueConfig);

            ChallengeLeague newChallengeLeague = new ChallengeLeague();
            NonMatchScheduler scheduler = new NonMatchScheduler();

            LeagueBuilder<ChallengeLeague> b1 = new LeagueBuilder<ChallengeLeague>(newChallengeLeague, scheduler);

            _challengeLeague = director.Construct(b1);

            // Assert

            // correct number of sides in league
            Assert.IsTrue(_challengeLeague.LeagueCompetitors.Count == _sides.Count);

            // correct number of matches got created
            Assert.IsTrue(_challengeLeague.LeagueMatches.Count == scheduler.TotalNumberOfMatches);
        }
    }
}
