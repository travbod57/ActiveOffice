using BusinessServices;
using BusinessServices.Builders;
using BusinessServices.Builders.KnockoutCompetition;
using BusinessServices.Dtos.Knockout;
using BusinessServices.Dtos.League;
using BusinessServices.Enums;
using BusinessServices.Interfaces;
using BusinessServices.Schedulers;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.Actors;
using Model.Competitors;
using Model.Knockouts;
using Model.Leagues;
using Model.ReferenceData;
using Model.Schedule;
using Model.Scheduling;
using Model.Sports;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;
using BusinessServices.Managers.KnockoutCompetition;
using BusinessServices.Sports;

namespace Test.KnockoutCompetition
{
    [TestClass]
    public class KnockMatchManagerTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private IAuditLogger _auditLogger;
        private List<Side> _sides;
        private CompetitionType _competitionType;
        private IList<SportColumn> _sportColumns;
        private Knockout _knockout;
        private KnockoutMatchScheduler _matchScheduler;

        private Team _t1;
        private Team _t2; 
        private Team _t3; 
        private Team _t4; 
        private Team _t5; 
        private Team _t6; 
        private Team _t7; 
        private Team _t8; 
        private Team _t9; 
        private Team _t10;
        private Team _t11;
        private Team _t12;
        private Team _t13;
        private Team _t14;
        private Team _t15;
        private Team _t16;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _auditLogger = new AuditLogger(_unitOfWork.Object);
            _unitOfWork.Setup(x => x.GetRepository<Audit>().All());

            _t1 = new Team() { Name = "West Ham" };
            _t2 = new Team() { Name = "Spurs" };
            _t3 = new Team() { Name = "Leicester" };
            _t4 = new Team() { Name = "Norwich" };
            _t5 = new Team() { Name = "Man Utd" };
            _t6 = new Team() { Name = "Newcastle" };
            _t7 = new Team() { Name = "West Brom" };
            _t8 = new Team() { Name = "Chelsea" };
            _t9 = new Team() { Name = "Sunderland" };
            _t10 = new Team() { Name = "Stoke" };
            _t11 = new Team() { Name = "Man City" };
            _t12 = new Team() { Name = "Aston Villa" };
            _t13 = new Team() { Name = "Watford" };
            _t14 = new Team() { Name = "Crystal Palace" };
            _t15 = new Team() { Name = "Liverpool" };
            _t16 = new Team() { Name = "Bournemouth" };

            _sportColumns = new List<SportColumn>()
            {
                new SportColumn() { Id = 1, Name = "Played" },
                new SportColumn() { Id = 2, Name = "Wins" },
                new SportColumn() { Id = 3, Name = "Draws" },
                new SportColumn() { Id = 4, Name = "Losses" },
                new SportColumn() { Id = 5, Name = "GoalsFor" },
                new SportColumn() { Id = 6, Name = "GoalsAgainst" },
                new SportColumn() { Id = 7, Name = "GoalDifference" }
            };

            _auditLogger = new AuditLogger(_unitOfWork.Object);

            _sides = new List<Side>() { _t1, _t2, _t3, _t4 };

            KnockoutConfig config = new KnockoutConfig()
            {
                Name = "My Knockout",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                IncludeThirdPlacePlayoff = false,
                IsSeeded = false,
                Sides = _sides,
                AuditLogger = _auditLogger,
                SportColumns = _sportColumns,
                NumberOfRounds = 2
            };

            KnockoutBuilderDirector director = new KnockoutBuilderDirector(config);

            _knockout = new Knockout() { CompetitionType = new CompetitionType() { Id = 1, Name = "Knockout" } };
            KnockoutSorter sorter = new KnockoutSorter(_knockout);

            _matchScheduler = new KnockoutMatchScheduler(_knockout, config);

            KnockoutBuilder builder = new KnockoutBuilder(_knockout, sorter, _matchScheduler);

            director.Construct(builder);
        }

        [TestMethod]
        public void Knockout_AwardWin_Move_To_Next_Match()
        {
            // Arrange

            List<RoundInformationDto> roundInformation = _matchScheduler.RoundInformation;

            // Act
            
            List<KnockoutMatch> knockoutMatches = _knockout.KnockoutMatches.ToList();

            ISportManager footballManager = new FootballManager(_knockout.CompetitionType);
            KnockoutManager knockoutManager = new KnockoutManager(_knockout, footballManager);

            KnockoutMatch semiFinalA = knockoutMatches.Single( km => km.Round == EnumRound.SemiFinal && km.MatchNumberForRound == 1);
            KnockoutMatch semiFinalB = knockoutMatches.Single( km => km.Round == EnumRound.SemiFinal && km.MatchNumberForRound == 2);

            KnockoutCompetitor winnerSemiFinalA = (KnockoutCompetitor)semiFinalA.CompetitorA;
            KnockoutCompetitor loserSemiFinalA = (KnockoutCompetitor)semiFinalA.CompetitorB;

            KnockoutCompetitor winnerSemiFinalB = (KnockoutCompetitor)semiFinalB.CompetitorB;
            KnockoutCompetitor loserSemiFinalB = (KnockoutCompetitor)semiFinalB.CompetitorA;

            knockoutManager.AwardWin(semiFinalA, winnerSemiFinalA, loserSemiFinalA, 2, 1);
            knockoutManager.AwardWin(semiFinalB, winnerSemiFinalB, loserSemiFinalB, 2, 1);

            // Assert

            // semi final A

            Assert.IsTrue(winnerSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Played").Value == 1);
            Assert.IsTrue(winnerSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Wins").Value == 1);
            Assert.IsTrue(winnerSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Draws").Value == 0);
            Assert.IsTrue(winnerSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Losses").Value == 0);
            Assert.IsTrue(winnerSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsFor").Value == 2);
            Assert.IsTrue(winnerSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsAgainst").Value == 1);
            Assert.IsTrue(winnerSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalDifference").Value == 1);

            Assert.IsTrue(loserSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Played").Value == 1);
            Assert.IsTrue(loserSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Wins").Value == 0);
            Assert.IsTrue(loserSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Draws").Value == 0);
            Assert.IsTrue(loserSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Losses").Value == 1);
            Assert.IsTrue(loserSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsFor").Value == 1);
            Assert.IsTrue(loserSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsAgainst").Value == 2);
            Assert.IsTrue(loserSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalDifference").Value == -1);

            // semi final B

            Assert.IsTrue(winnerSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Played").Value == 1);
            Assert.IsTrue(winnerSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Wins").Value == 1);
            Assert.IsTrue(winnerSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Draws").Value == 0);
            Assert.IsTrue(winnerSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Losses").Value == 0);
            Assert.IsTrue(winnerSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsFor").Value == 2);
            Assert.IsTrue(winnerSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsAgainst").Value == 1);
            Assert.IsTrue(winnerSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalDifference").Value == 1);

            Assert.IsTrue(loserSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Played").Value == 1);
            Assert.IsTrue(loserSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Wins").Value == 0);
            Assert.IsTrue(loserSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Draws").Value == 0);
            Assert.IsTrue(loserSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Losses").Value == 1);
            Assert.IsTrue(loserSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsFor").Value == 1);
            Assert.IsTrue(loserSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsAgainst").Value == 2);
            Assert.IsTrue(loserSemiFinalA.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalDifference").Value == -1);

            Assert.AreEqual(semiFinalA.NextRoundMatch, semiFinalB.NextRoundMatch);
            Assert.AreEqual(semiFinalA.NextRoundMatch.CompetitorA, winnerSemiFinalA);
            Assert.AreEqual(semiFinalB.NextRoundMatch.CompetitorB, winnerSemiFinalB);
        }
    }
}
