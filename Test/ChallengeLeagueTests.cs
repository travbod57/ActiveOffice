﻿using BusinessServices;
using BusinessServices.Builders;
using BusinessServices.Dtos;
using BusinessServices.Dtos.League;
using BusinessServices.Enums;
using BusinessServices.Interfaces;
using BusinessServices.Schedulers;
using BusinessServices.Sports;
using BusinessServices.Updaters;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.Actors;
using Model.Competitors;
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

namespace Test
{
    [TestClass]
    public class ChallengeLeagueTests
    {
        /* TODO: 
         * Match scheduling based on number of face offs
         * Match scheduling to do home and away macth ups etc
        */

        private ChallengeLeague _challengeLeague;
        private LeagueCreatorDto _leagueCreatorDto;
        private Mock<IUnitOfWork> _unitOfWork;
        private IAuditLogger _auditLogger;

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

            List<Side> sides = new List<Side>() { t1, t2, t3, t4, t5 };

            _leagueCreatorDto = new LeagueCreatorDto() { CanSidePlayMoreThanOncePerMatchDay = true, Occurrance = Occurrance.Daily, ScheduleType = ScheduleType.Scheduled, DayOfWeek = DayOfWeek.Saturday };

            IList<SportColumn> sportColumns = new List<SportColumn>()
            {
                new SportColumn() { Id = 1, Name = "Played" },
                new SportColumn() { Id = 2, Name = "Points" },
                new SportColumn() { Id = 3, Name = "Wins" },
                new SportColumn() { Id = 4, Name = "Draws" },
                new SportColumn() { Id = 5, Name = "Losses" },
                new SportColumn() { Id = 6, Name = "GoalsFor" },
                new SportColumn() { Id = 7, Name = "GoalsAgainst" }
            };

            LeagueBuilderDirector<ChallengeLeague> director = new LeagueBuilderDirector<ChallengeLeague>("League 1", DateTime.Now, DateTime.Now.AddDays(30), 5, 4, sides, _auditLogger, sportColumns);

            ChallengeLeague newChallengeLeague = new ChallengeLeague();
            ChallengeLeagueSorter sorter = new ChallengeLeagueSorter(newChallengeLeague);
            RandomMatchScheduler scheduler = new RandomMatchScheduler(newChallengeLeague, _leagueCreatorDto);

            LeagueBuilder<ChallengeLeague> b1 = new LeagueBuilder<ChallengeLeague>(newChallengeLeague, sorter, scheduler);

            _challengeLeague = director.Construct(b1);
        }

        [TestMethod]
        public void Award_Win_Football()
        {
            // Arrange

            LeagueMatch leagueMatch = _challengeLeague.LeagueMatches.First();

            Competitor winner = leagueMatch.CompetitorA;
            leagueMatch.CompetitorAScore = 2;
            Competitor loser = leagueMatch.CompetitorB;
            leagueMatch.CompetitorBScore = 1;

            ISportManager footballManager = new FootballManager(_challengeLeague.CompetitionType);

            ChallengeLeagueManager manager = new ChallengeLeagueManager(_challengeLeague, footballManager);

            // Act

            manager.AwardWin(leagueMatch, winner, loser);

            // Assert

            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Position").Value == 3);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Played").Value == 1);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Wins").Value == 1);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Draws").Value == 0);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Losses").Value == 0);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsFor").Value == 2);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsAgainst").Value == 1);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalDifference").Value == 1);

            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Position").Value == 0);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Played").Value == 1);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Wins").Value == 0);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Draws").Value == 0);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Losses").Value == 1);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsFor").Value == 1);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsAgainst").Value == 2);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalDifference").Value == -1); 
        }
    }
}
