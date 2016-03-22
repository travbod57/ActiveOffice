using BusinessServices;
using BusinessServices.Builders;
using BusinessServices.Builders.KnockoutCompetition;
using BusinessServices.Builders.LeagueCompetition;
using BusinessServices.Builders.TournamentCompetition;
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
using Model.Tournaments;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.ChallengeLeagueCompetition
{
    [TestClass]
    public class TournamentCreationTests
    {
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
                new SportColumn() { Id = 7, Name = "GoalsAgainst" }
            };
        }

        [TestMethod]
        public void Create_Football_Tournament()
        {
            // Arrange

            TournamentConfig config = new TournamentConfig()
            {
                NumberOfRounds = 4,
                IsSeeded = false,
                IncludeThirdPlacePlayoff = false,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                NumberOfPools = 6,
                Sides = _sides,
                SportColumns = _footballSportColumns,
                Name = "My Tournament",
                AuditLogger = _auditLogger,
                NumberOfPositionsPerPool = 5
            };

            // Act

            TournamentBuilderDirector director = new TournamentBuilderDirector(config);

            Tournament tournament = new Tournament();
            NonMatchScheduler scheduler = new NonMatchScheduler();
            ITournamentSorter tournamentSorter = new TournamentSorter(tournament);

            TournamentBuilder builder = new TournamentBuilder(tournament, tournamentSorter);

            director.Construct(builder);

            // Assert

            Assert.IsTrue(tournament.Leagues.Count == 6);
            Assert.IsNotNull(tournament.Knockout);
            Assert.IsTrue(tournament.Knockout.KnockoutMatches.Count == 15);
        }
    }
}
