using BusinessServices;
using BusinessServices.Builders;
using BusinessServices.Builders.KnockoutCompetition;
using BusinessServices.Dtos.Knockout;
using BusinessServices.Dtos.League;
using BusinessServices.Enums;
using BusinessServices.Interfaces;
using BusinessServices.Schedulers;
using BusinessServices.Updaters;
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

namespace Test
{
    [TestClass]
    public class KnockMatchSchedulerTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private IAuditLogger _auditLogger;
        private List<Side> _sides;
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
        private Team _t17;
        private Team _t18;
        private Team _t19;
        private Team _t20;
        private Team _t21;
        private Team _t22;
        private Team _t23;
        private Team _t24;
        private Team _t25;
        private Team _t26;
        private Team _t27;
        private Team _t28;
        private Team _t29;
        private Team _t30;
        private Team _t31;
        private Team _t32;

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

            _t17 = new Team() { Name = "Charlton" };
            _t18 = new Team() { Name = "Plymouth" };
            _t19 = new Team() { Name = "Milwall" };
            _t20 = new Team() { Name = "Yeovil" };
            _t21 = new Team() { Name = "QPR" };
            _t22 = new Team() { Name = "Sheffield" };
            _t23 = new Team() { Name = "Sheffield Wednesday" };
            _t24 = new Team() { Name = "Salford" };
            _t25 = new Team() { Name = "Bristol City" };
            _t26 = new Team() { Name = "Bristol Rovers" };
            _t27 = new Team() { Name = "Nottingham Forest" };
            _t28 = new Team() { Name = "Birmingham" };
            _t29 = new Team() { Name = "Everton" };
            _t30 = new Team() { Name = "Swansea" };
            _t31 = new Team() { Name = "Southampton" };
            _t32 = new Team() { Name = "Portsmouth" };
        }

        // TODO: test the exceptions

        [TestMethod]
        public void Schedule_Knockout_With_2_Rounds_No_Playoff()
        {
            // Arrange

            KnockoutCreatorDto knockoutCreatorDto = new KnockoutCreatorDto() { NumberOfRounds = 2, IsSeeded = false, IncludeThirdPlacePlayoff = false };
            List<Side> sides = new List<Side>() { _t1, _t2, _t3, _t4 };

            Knockout knockout = new Knockout();

            List<KnockoutCompetitor> knockoutCompetitors = sides.Select(x => new KnockoutCompetitor() { Side = x }).ToList();

            knockout.KnockoutCompetitors.AddRange(knockoutCompetitors);

            // Act

            KnockoutMatchScheduler scheduler = new KnockoutMatchScheduler(knockout, knockoutCreatorDto);
            List<RoundInformation> roundInformation = scheduler.RoundInformation;
            scheduler.Schedule();
            List<KnockoutMatch> knockoutMatches = knockout.KnockoutMatches.ToList();

            // Assert

            Assert.IsTrue(scheduler.TotalNumberOfMatches == 3);
            Assert.IsTrue(scheduler.TotalNumberOfCompetitors == 4);

            Assert.IsTrue(roundInformation[0].Round == EnumRound.SemiFinal && roundInformation[0].MatchesForRound == 2);
            Assert.IsTrue(roundInformation[1].Round == EnumRound.Final && roundInformation[1].MatchesForRound == 1);

            Assert.IsTrue(knockoutMatches.Count == 3);

            Assert.IsTrue(knockoutMatches[0].Round == EnumRound.Final && knockoutMatches[0].KnockoutSide == EnumKnockoutSide.Center && knockoutMatches[0].NextRoundMatch == null && knockoutMatches[0].MatchNumberForRound == 1);

            // Left side matches
            Assert.IsTrue(knockoutMatches[1].Round == EnumRound.SemiFinal && knockoutMatches[1].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[1].NextRoundMatch == knockoutMatches[0] && knockoutMatches[1].MatchNumberForRound == 1);

            // Right side matches
            Assert.IsTrue(knockoutMatches[2].Round == EnumRound.SemiFinal && knockoutMatches[2].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[2].NextRoundMatch == knockoutMatches[0] && knockoutMatches[2].MatchNumberForRound == 2);
        }

        [TestMethod]
        public void Schedule_Knockout_With_3_Rounds_No_Playoff()
        {
            // Arrange

            KnockoutCreatorDto knockoutCreatorDto = new KnockoutCreatorDto() { NumberOfRounds = 3, IsSeeded = false, IncludeThirdPlacePlayoff = false };
            List<Side> sides = new List<Side>() { _t1, _t2, _t3, _t4, _t5, _t6, _t7, _t8 };

            Knockout knockout = new Knockout();

            List<KnockoutCompetitor> knockoutCompetitors = sides.Select(x => new KnockoutCompetitor() { Side = x }).ToList();

            knockout.KnockoutCompetitors.AddRange(knockoutCompetitors);

            // Act

            KnockoutMatchScheduler scheduler = new KnockoutMatchScheduler(knockout, knockoutCreatorDto);
            List<RoundInformation> roundInformation = scheduler.RoundInformation;
            scheduler.Schedule();
            List<KnockoutMatch> knockoutMatches = knockout.KnockoutMatches.ToList();

            // Assert

            Assert.IsTrue(scheduler.TotalNumberOfMatches == 7);
            Assert.IsTrue(scheduler.TotalNumberOfCompetitors == 8);

            Assert.IsTrue(roundInformation[0].Round == EnumRound.QuarterFinal && roundInformation[0].MatchesForRound == 4);
            Assert.IsTrue(roundInformation[1].Round == EnumRound.SemiFinal && roundInformation[1].MatchesForRound == 2);
            Assert.IsTrue(roundInformation[2].Round == EnumRound.Final && roundInformation[2].MatchesForRound == 1);

            Assert.IsTrue(knockoutMatches.Count == 7);

            Assert.IsTrue(knockoutMatches[0].Round == EnumRound.Final && knockoutMatches[0].KnockoutSide == EnumKnockoutSide.Center && knockoutMatches[0].NextRoundMatch == null && knockoutMatches[0].MatchNumberForRound == 1);

            // Left side matches
            Assert.IsTrue(knockoutMatches[1].Round == EnumRound.SemiFinal && knockoutMatches[1].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[1].NextRoundMatch == knockoutMatches[0] && knockoutMatches[1].MatchNumberForRound == 1);
            Assert.IsTrue(knockoutMatches[2].Round == EnumRound.QuarterFinal && knockoutMatches[2].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[2].NextRoundMatch == knockoutMatches[1] && knockoutMatches[2].MatchNumberForRound == 1);
            Assert.IsTrue(knockoutMatches[3].Round == EnumRound.QuarterFinal && knockoutMatches[3].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[3].NextRoundMatch == knockoutMatches[1] && knockoutMatches[3].MatchNumberForRound == 2);

            // Right side matches
            Assert.IsTrue(knockoutMatches[4].Round == EnumRound.SemiFinal && knockoutMatches[4].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[4].NextRoundMatch == knockoutMatches[0] && knockoutMatches[4].MatchNumberForRound == 2);
            Assert.IsTrue(knockoutMatches[5].Round == EnumRound.QuarterFinal && knockoutMatches[5].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[5].NextRoundMatch == knockoutMatches[4] && knockoutMatches[5].MatchNumberForRound == 3);
            Assert.IsTrue(knockoutMatches[6].Round == EnumRound.QuarterFinal && knockoutMatches[6].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[6].NextRoundMatch == knockoutMatches[4] && knockoutMatches[6].MatchNumberForRound == 4);
        }

        [TestMethod]
        public void Schedule_Knockout_With_4_Rounds_No_Playoff()
        {
            KnockoutCreatorDto knockoutCreatorDto = new KnockoutCreatorDto() { NumberOfRounds = 4, IsSeeded = false, IncludeThirdPlacePlayoff = false };
            List<Side> sides = new List<Side>() { _t1, _t2, _t3, _t4, _t5, _t6, _t7, _t8, _t9, _t10, _t11, _t12, _t13, _t14, _t15, _t16 };

            Knockout knockout = new Knockout();

            List<KnockoutCompetitor> knockoutCompetitors = sides.Select(x => new KnockoutCompetitor() { Side = x }).ToList();

            knockout.KnockoutCompetitors.AddRange(knockoutCompetitors);

            // Act

            KnockoutMatchScheduler scheduler = new KnockoutMatchScheduler(knockout, knockoutCreatorDto);
            List<RoundInformation> roundInformation = scheduler.RoundInformation;
            scheduler.Schedule();
            List<KnockoutMatch> knockoutMatches = knockout.KnockoutMatches.ToList();

            // Assert

            Assert.IsTrue(scheduler.TotalNumberOfMatches == 15);
            Assert.IsTrue(scheduler.TotalNumberOfCompetitors == 16);

            Assert.IsTrue(roundInformation[0].Round == EnumRound.FirstRound && roundInformation[0].MatchesForRound == 8);
            Assert.IsTrue(roundInformation[1].Round == EnumRound.QuarterFinal && roundInformation[1].MatchesForRound == 4);
            Assert.IsTrue(roundInformation[2].Round == EnumRound.SemiFinal && roundInformation[2].MatchesForRound == 2);
            Assert.IsTrue(roundInformation[3].Round == EnumRound.Final && roundInformation[3].MatchesForRound == 1);

            Assert.IsTrue(knockoutMatches[0].Round == EnumRound.Final && knockoutMatches[0].KnockoutSide == EnumKnockoutSide.Center && knockoutMatches[0].NextRoundMatch == null && knockoutMatches[0].MatchNumberForRound == 1);

            // Left side matches
            Assert.IsTrue(knockoutMatches[1].Round == EnumRound.SemiFinal && knockoutMatches[1].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[1].NextRoundMatch == knockoutMatches[0] && knockoutMatches[1].MatchNumberForRound == 1);
            
            Assert.IsTrue(knockoutMatches[2].Round == EnumRound.QuarterFinal && knockoutMatches[2].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[2].NextRoundMatch == knockoutMatches[1] && knockoutMatches[3].MatchNumberForRound == 1);
            Assert.IsTrue(knockoutMatches[3].Round == EnumRound.FirstRound && knockoutMatches[3].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[3].NextRoundMatch == knockoutMatches[2] && knockoutMatches[3].MatchNumberForRound == 1);
            Assert.IsTrue(knockoutMatches[4].Round == EnumRound.FirstRound && knockoutMatches[4].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[4].NextRoundMatch == knockoutMatches[2] && knockoutMatches[4].MatchNumberForRound == 2);

            Assert.IsTrue(knockoutMatches[5].Round == EnumRound.QuarterFinal && knockoutMatches[5].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[5].NextRoundMatch == knockoutMatches[1] && knockoutMatches[5].MatchNumberForRound == 2);
            Assert.IsTrue(knockoutMatches[6].Round == EnumRound.FirstRound && knockoutMatches[6].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[6].NextRoundMatch == knockoutMatches[5] && knockoutMatches[6].MatchNumberForRound == 3);
            Assert.IsTrue(knockoutMatches[7].Round == EnumRound.FirstRound && knockoutMatches[7].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[7].NextRoundMatch == knockoutMatches[5] && knockoutMatches[7].MatchNumberForRound == 4);

            // Right side matches
            Assert.IsTrue(knockoutMatches[8].Round == EnumRound.SemiFinal && knockoutMatches[8].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[8].NextRoundMatch == knockoutMatches[0] && knockoutMatches[8].MatchNumberForRound == 2);

            Assert.IsTrue(knockoutMatches[9].Round == EnumRound.QuarterFinal && knockoutMatches[9].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[9].NextRoundMatch == knockoutMatches[8] && knockoutMatches[9].MatchNumberForRound == 3);
            Assert.IsTrue(knockoutMatches[10].Round == EnumRound.FirstRound && knockoutMatches[10].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[10].NextRoundMatch == knockoutMatches[9] && knockoutMatches[10].MatchNumberForRound == 5);
            Assert.IsTrue(knockoutMatches[11].Round == EnumRound.FirstRound && knockoutMatches[11].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[11].NextRoundMatch == knockoutMatches[9] && knockoutMatches[11].MatchNumberForRound == 6);

            Assert.IsTrue(knockoutMatches[12].Round == EnumRound.QuarterFinal && knockoutMatches[10].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[12].NextRoundMatch == knockoutMatches[8] && knockoutMatches[12].MatchNumberForRound == 4);
            Assert.IsTrue(knockoutMatches[13].Round == EnumRound.FirstRound && knockoutMatches[13].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[13].NextRoundMatch == knockoutMatches[12] && knockoutMatches[13].MatchNumberForRound == 7);
            Assert.IsTrue(knockoutMatches[14].Round == EnumRound.FirstRound && knockoutMatches[14].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[14].NextRoundMatch == knockoutMatches[12] && knockoutMatches[14].MatchNumberForRound == 8);
        }

        [TestMethod]
        public void Schedule_Knockout_With_5_Rounds_No_Playoff()
        {
            KnockoutCreatorDto knockoutCreatorDto = new KnockoutCreatorDto() { NumberOfRounds = 5, IsSeeded = false, IncludeThirdPlacePlayoff = false };
            List<Side> sides = new List<Side>() { _t1, _t2, _t3, _t4, _t5, _t6, _t7, _t8, _t9, _t10, _t11, _t12, _t13, _t14, _t15, _t16, _t17, _t18, _t19, _t20, _t21, _t22, _t23, _t24, _t25, _t26, _t27, _t28, _t29, _t30, _t31, _t32 };

            Knockout knockout = new Knockout();

            List<KnockoutCompetitor> knockoutCompetitors = sides.Select(x => new KnockoutCompetitor() { Side = x }).ToList();

            knockout.KnockoutCompetitors.AddRange(knockoutCompetitors);

            // Act

            KnockoutMatchScheduler scheduler = new KnockoutMatchScheduler(knockout, knockoutCreatorDto);
            List<RoundInformation> roundInformation = scheduler.RoundInformation;
            scheduler.Schedule();
            List<KnockoutMatch> knockoutMatches = knockout.KnockoutMatches.ToList();

            // Assert

            Assert.IsTrue(scheduler.TotalNumberOfMatches == 31);
            Assert.IsTrue(scheduler.TotalNumberOfCompetitors == 32);

            Assert.IsTrue(roundInformation[0].Round == EnumRound.FirstRound && roundInformation[0].MatchesForRound == 16);
            Assert.IsTrue(roundInformation[1].Round == EnumRound.SecondRound && roundInformation[1].MatchesForRound == 8);
            Assert.IsTrue(roundInformation[2].Round == EnumRound.QuarterFinal && roundInformation[2].MatchesForRound == 4);
            Assert.IsTrue(roundInformation[3].Round == EnumRound.SemiFinal && roundInformation[3].MatchesForRound == 2);
            Assert.IsTrue(roundInformation[4].Round == EnumRound.Final && roundInformation[4].MatchesForRound == 1);

            Assert.IsTrue(knockoutMatches[0].Round == EnumRound.Final && knockoutMatches[0].KnockoutSide == EnumKnockoutSide.Center && knockoutMatches[0].NextRoundMatch == null && knockoutMatches[0].MatchNumberForRound == 1);

            // Left side matches
            Assert.IsTrue(knockoutMatches[1].Round == EnumRound.SemiFinal && knockoutMatches[1].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[1].NextRoundMatch == knockoutMatches[0] && knockoutMatches[1].MatchNumberForRound == 1);

            Assert.IsTrue(knockoutMatches[2].Round == EnumRound.QuarterFinal && knockoutMatches[2].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[2].NextRoundMatch == knockoutMatches[1] && knockoutMatches[3].MatchNumberForRound == 1);
            Assert.IsTrue(knockoutMatches[3].Round == EnumRound.SecondRound && knockoutMatches[3].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[3].NextRoundMatch == knockoutMatches[2] && knockoutMatches[3].MatchNumberForRound == 1);
            Assert.IsTrue(knockoutMatches[4].Round == EnumRound.FirstRound && knockoutMatches[4].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[4].NextRoundMatch == knockoutMatches[3] && knockoutMatches[4].MatchNumberForRound == 1);
            Assert.IsTrue(knockoutMatches[5].Round == EnumRound.FirstRound && knockoutMatches[5].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[5].NextRoundMatch == knockoutMatches[3] && knockoutMatches[5].MatchNumberForRound == 2);

            Assert.IsTrue(knockoutMatches[6].Round == EnumRound.SecondRound && knockoutMatches[6].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[6].NextRoundMatch == knockoutMatches[2] && knockoutMatches[6].MatchNumberForRound == 2);
            Assert.IsTrue(knockoutMatches[7].Round == EnumRound.FirstRound && knockoutMatches[7].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[7].NextRoundMatch == knockoutMatches[6] && knockoutMatches[7].MatchNumberForRound == 3);
            Assert.IsTrue(knockoutMatches[8].Round == EnumRound.FirstRound && knockoutMatches[8].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[8].NextRoundMatch == knockoutMatches[6] && knockoutMatches[8].MatchNumberForRound == 4);

            Assert.IsTrue(knockoutMatches[9].Round == EnumRound.QuarterFinal && knockoutMatches[9].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[9].NextRoundMatch == knockoutMatches[1] && knockoutMatches[9].MatchNumberForRound == 2);
            Assert.IsTrue(knockoutMatches[10].Round == EnumRound.SecondRound && knockoutMatches[10].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[10].NextRoundMatch == knockoutMatches[9] && knockoutMatches[10].MatchNumberForRound == 3);
            Assert.IsTrue(knockoutMatches[11].Round == EnumRound.FirstRound && knockoutMatches[11].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[11].NextRoundMatch == knockoutMatches[10] && knockoutMatches[11].MatchNumberForRound == 5);
            Assert.IsTrue(knockoutMatches[12].Round == EnumRound.FirstRound && knockoutMatches[12].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[12].NextRoundMatch == knockoutMatches[10] && knockoutMatches[12].MatchNumberForRound == 6);

            Assert.IsTrue(knockoutMatches[13].Round == EnumRound.SecondRound && knockoutMatches[13].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[13].NextRoundMatch == knockoutMatches[9] && knockoutMatches[13].MatchNumberForRound == 4);
            Assert.IsTrue(knockoutMatches[14].Round == EnumRound.FirstRound && knockoutMatches[14].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[14].NextRoundMatch == knockoutMatches[13] && knockoutMatches[14].MatchNumberForRound == 7);
            Assert.IsTrue(knockoutMatches[15].Round == EnumRound.FirstRound && knockoutMatches[15].KnockoutSide == EnumKnockoutSide.Left && knockoutMatches[15].NextRoundMatch == knockoutMatches[13] && knockoutMatches[15].MatchNumberForRound == 8);

            // Right side matches
            Assert.IsTrue(knockoutMatches[16].Round == EnumRound.SemiFinal && knockoutMatches[16].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[16].NextRoundMatch == knockoutMatches[0] && knockoutMatches[16].MatchNumberForRound == 2);

            Assert.IsTrue(knockoutMatches[17].Round == EnumRound.QuarterFinal && knockoutMatches[17].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[17].NextRoundMatch == knockoutMatches[16] && knockoutMatches[17].MatchNumberForRound == 3);
            Assert.IsTrue(knockoutMatches[18].Round == EnumRound.SecondRound && knockoutMatches[18].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[18].NextRoundMatch == knockoutMatches[17] && knockoutMatches[18].MatchNumberForRound == 5);
            Assert.IsTrue(knockoutMatches[19].Round == EnumRound.FirstRound && knockoutMatches[19].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[19].NextRoundMatch == knockoutMatches[18] && knockoutMatches[19].MatchNumberForRound == 9);
            Assert.IsTrue(knockoutMatches[20].Round == EnumRound.FirstRound && knockoutMatches[20].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[20].NextRoundMatch == knockoutMatches[18] && knockoutMatches[20].MatchNumberForRound == 10);

            Assert.IsTrue(knockoutMatches[21].Round == EnumRound.SecondRound && knockoutMatches[21].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[21].NextRoundMatch == knockoutMatches[17] && knockoutMatches[21].MatchNumberForRound == 6);
            Assert.IsTrue(knockoutMatches[22].Round == EnumRound.FirstRound && knockoutMatches[22].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[22].NextRoundMatch == knockoutMatches[21] && knockoutMatches[22].MatchNumberForRound == 11);
            Assert.IsTrue(knockoutMatches[23].Round == EnumRound.FirstRound && knockoutMatches[23].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[23].NextRoundMatch == knockoutMatches[21] && knockoutMatches[23].MatchNumberForRound == 12);

            Assert.IsTrue(knockoutMatches[24].Round == EnumRound.QuarterFinal && knockoutMatches[24].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[24].NextRoundMatch == knockoutMatches[16] && knockoutMatches[24].MatchNumberForRound == 4);
            Assert.IsTrue(knockoutMatches[25].Round == EnumRound.SecondRound && knockoutMatches[25].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[25].NextRoundMatch == knockoutMatches[24] && knockoutMatches[25].MatchNumberForRound == 7);
            Assert.IsTrue(knockoutMatches[26].Round == EnumRound.FirstRound && knockoutMatches[26].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[26].NextRoundMatch == knockoutMatches[25] && knockoutMatches[26].MatchNumberForRound == 13);
            Assert.IsTrue(knockoutMatches[27].Round == EnumRound.FirstRound && knockoutMatches[27].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[27].NextRoundMatch == knockoutMatches[25] && knockoutMatches[27].MatchNumberForRound == 14);

            Assert.IsTrue(knockoutMatches[28].Round == EnumRound.SecondRound && knockoutMatches[28].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[28].NextRoundMatch == knockoutMatches[24] && knockoutMatches[28].MatchNumberForRound == 8);
            Assert.IsTrue(knockoutMatches[29].Round == EnumRound.FirstRound && knockoutMatches[29].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[29].NextRoundMatch == knockoutMatches[28] && knockoutMatches[29].MatchNumberForRound == 15);
            Assert.IsTrue(knockoutMatches[30].Round == EnumRound.FirstRound && knockoutMatches[30].KnockoutSide == EnumKnockoutSide.Right && knockoutMatches[30].NextRoundMatch == knockoutMatches[28] && knockoutMatches[30].MatchNumberForRound == 16);
        }
    }
}
