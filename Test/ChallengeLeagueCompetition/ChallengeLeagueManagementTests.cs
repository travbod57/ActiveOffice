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

namespace Test.ChallengeLeagueCompetition
{
    [TestClass]
    public class ChallengeLeagueManagementTests
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

            _leagueCreatorDto = new LeagueCreatorDto() { NumberOfCompetitors = 5, CanSidePlayMoreThanOncePerMatchDay = true, Occurrance = Occurrance.Daily, ScheduleType = ScheduleType.Scheduled, DayOfWeek = DayOfWeek.Saturday };

            IList<SportColumn> sportColumns = new List<SportColumn>()
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

            LeagueBuilderDirector<ChallengeLeague> director = new LeagueBuilderDirector<ChallengeLeague>("League 1", DateTime.Now, DateTime.Now.AddDays(30), 5, 4, sides, _auditLogger, sportColumns);

            ChallengeLeague newChallengeLeague = new ChallengeLeague() { CompetitionType = new CompetitionType() { Id = 1, Name = "ChallengeLeague" } };
            NonMatchScheduler scheduler = new NonMatchScheduler();

            LeagueBuilder<ChallengeLeague> b1 = new LeagueBuilder<ChallengeLeague>(newChallengeLeague, scheduler);

            _challengeLeague = director.Construct(b1);
        }

        [TestMethod]
        public void Award_Win_To_Challenger_Football()
        {
            // Arrange

            LeagueMatch leagueMatch = new LeagueMatch()
            {
                CompetitorA = _challengeLeague.LeagueCompetitors.Last(),
                CompetitorAScore = 2,
                CompetitorB = _challengeLeague.LeagueCompetitors.First(),
                CompetitorBScore = 1
            };

            int intialChallengerPosition = ((LeagueCompetitor)leagueMatch.CompetitorA).InitialPositionNumber;
            int intialChallengeePosition = ((LeagueCompetitor)leagueMatch.CompetitorB).InitialPositionNumber;

            LeagueCompetitor winner = (LeagueCompetitor)leagueMatch.CompetitorA;
            LeagueCompetitor loser = (LeagueCompetitor)leagueMatch.CompetitorB;

            ISportManager footballManager = new FootballManager(_challengeLeague.CompetitionType);

            ChallengeLeagueManager manager = new ChallengeLeagueManager(_challengeLeague, footballManager);

            // Act

            manager.AwardWin(leagueMatch, winner, loser);

            // Assert

            Assert.IsTrue(winner.CurrentPositionNumber == intialChallengeePosition);
            Assert.IsTrue(winner.Side.Name == "Arsenal");
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Played").Value == 1);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Wins").Value == 1);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Draws").Value == 0);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Losses").Value == 0);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsFor").Value == 2);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsAgainst").Value == 1);
            Assert.IsTrue(winner.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalDifference").Value == 1);

            Assert.IsTrue(loser.CurrentPositionNumber == intialChallengeePosition + 1);
            Assert.IsTrue(loser.Side.Name == "West Ham");
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Played").Value == 1);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Wins").Value == 0);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Draws").Value == 0);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "Losses").Value == 1);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsFor").Value == 1);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalsAgainst").Value == 2);
            Assert.IsTrue(loser.CompetitorRecords.Single(cr => cr.SportColumn.Name == "GoalDifference").Value == -1);

            Assert.IsTrue(_challengeLeague.LeagueCompetitors.Single( lc => lc.CurrentPositionNumber == 3).Side.Name == "Spurs");
            Assert.IsTrue(_challengeLeague.LeagueCompetitors.Single(lc => lc.CurrentPositionNumber == 4).Side.Name == "Leicester");
            Assert.IsTrue(_challengeLeague.LeagueCompetitors.Single(lc => lc.CurrentPositionNumber == 5).Side.Name == "Norwich");
        }

        [TestMethod]
        public void Get_League_Standings()
        {
            // Arrange

            LeagueMatch leagueMatch = new LeagueMatch()
            {
                CompetitorA = _challengeLeague.LeagueCompetitors.Last(),
                CompetitorAScore = 2,
                CompetitorB = _challengeLeague.LeagueCompetitors.First(),
                CompetitorBScore = 1
            };

            int intialChallengerPosition = ((LeagueCompetitor)leagueMatch.CompetitorA).InitialPositionNumber;
            int intialChallengeePosition = ((LeagueCompetitor)leagueMatch.CompetitorB).InitialPositionNumber;

            LeagueCompetitor winner = (LeagueCompetitor)leagueMatch.CompetitorA;
            LeagueCompetitor loser = (LeagueCompetitor)leagueMatch.CompetitorB;

            ISportManager footballManager = new FootballManager(_challengeLeague.CompetitionType);

            ChallengeLeagueManager manager = new ChallengeLeagueManager(_challengeLeague, footballManager);

            // Act

            manager.AwardWin(leagueMatch, winner, loser);
            List<LeagueTableRowDto> standings = manager.GetLeagueStandings();

            // Assert

            Assert.IsTrue(standings[0].SideName == "Arsenal");
            Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "Wins").Item2 == 1);
            Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "Draws").Item2 == 0);
            Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "Losses").Item2 == 0);
            Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "GoalsFor").Item2 == 2);
            Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "GoalsAgainst").Item2 == 1);
            Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "GoalDifference").Item2 == 1);
            Assert.IsTrue(standings[0].ColumnValues.Single(x => x.Item1 == "Played").Item2 == 1);

            Assert.IsTrue(standings[1].SideName == "West Ham");
            Assert.IsTrue(standings[1].ColumnValues.Single(x => x.Item1 == "Wins").Item2 == 0);
            Assert.IsTrue(standings[1].ColumnValues.Single(x => x.Item1 == "Draws").Item2 == 0);
            Assert.IsTrue(standings[1].ColumnValues.Single(x => x.Item1 == "Losses").Item2 == 1);
            Assert.IsTrue(standings[1].ColumnValues.Single(x => x.Item1 == "GoalsFor").Item2 == 1);
            Assert.IsTrue(standings[1].ColumnValues.Single(x => x.Item1 == "GoalsAgainst").Item2 == 2);
            Assert.IsTrue(standings[1].ColumnValues.Single(x => x.Item1 == "GoalDifference").Item2 == -1);
            Assert.IsTrue(standings[1].ColumnValues.Single(x => x.Item1 == "Played").Item2 == 1);

            Assert.IsTrue(standings[2].SideName == "Spurs");
            Assert.IsTrue(standings[2].ColumnValues.Single(x => x.Item1 == "Wins").Item2 == 0);
            Assert.IsTrue(standings[2].ColumnValues.Single(x => x.Item1 == "Draws").Item2 == 0);
            Assert.IsTrue(standings[2].ColumnValues.Single(x => x.Item1 == "Losses").Item2 == 0);
            Assert.IsTrue(standings[2].ColumnValues.Single(x => x.Item1 == "GoalsFor").Item2 == 0);
            Assert.IsTrue(standings[2].ColumnValues.Single(x => x.Item1 == "GoalsAgainst").Item2 == 0);
            Assert.IsTrue(standings[2].ColumnValues.Single(x => x.Item1 == "GoalDifference").Item2 == 0);
            Assert.IsTrue(standings[2].ColumnValues.Single(x => x.Item1 == "Played").Item2 == 0);

            Assert.IsTrue(standings[3].SideName == "Leicester");
            Assert.IsTrue(standings[3].ColumnValues.Single(x => x.Item1 == "Wins").Item2 == 0);
            Assert.IsTrue(standings[3].ColumnValues.Single(x => x.Item1 == "Draws").Item2 == 0);
            Assert.IsTrue(standings[3].ColumnValues.Single(x => x.Item1 == "Losses").Item2 == 0);
            Assert.IsTrue(standings[3].ColumnValues.Single(x => x.Item1 == "GoalsFor").Item2 == 0);
            Assert.IsTrue(standings[3].ColumnValues.Single(x => x.Item1 == "GoalsAgainst").Item2 == 0);
            Assert.IsTrue(standings[3].ColumnValues.Single(x => x.Item1 == "GoalDifference").Item2 == 0);
            Assert.IsTrue(standings[3].ColumnValues.Single(x => x.Item1 == "Played").Item2 == 0);

            Assert.IsTrue(standings[4].SideName == "Norwich");
            Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "Wins").Item2 == 0);
            Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "Draws").Item2 == 0);
            Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "Losses").Item2 == 0);
            Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "GoalsFor").Item2 == 0);
            Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "GoalsAgainst").Item2 == 0);
            Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "GoalDifference").Item2 == 0);
            Assert.IsTrue(standings[4].ColumnValues.Single(x => x.Item1 == "Played").Item2 == 0);
        }

        [TestMethod]
        public void Create_Challenge()
        {
            // Arrange

            ISportManager footballManager = new FootballManager(_challengeLeague.CompetitionType);

            ChallengeLeagueManager manager = new ChallengeLeagueManager(_challengeLeague, footballManager);

            List<LeagueCompetitor> leagueCompetitors = _challengeLeague.LeagueCompetitors.ToList();

            LeagueCompetitor challenger = leagueCompetitors[0];
            LeagueCompetitor defender = leagueCompetitors[1];
            DateTime dateTimeOfPlay = DateTime.Now;

            // Act

            manager.CreateChallenge(challenger, defender, dateTimeOfPlay);

            // Assert
            List<LeagueMatch> leagueMatches = _challengeLeague.LeagueMatches.ToList();

            Assert.IsTrue(leagueMatches.Count == 1);
            Assert.IsTrue(leagueMatches[0].CompetitorA == challenger);
            Assert.IsTrue(leagueMatches[0].CompetitorB == defender);
            Assert.IsTrue(leagueMatches[0].DateTimeOfPlay == dateTimeOfPlay);
        }


        // TODO: Do a test for all positions inbetween after a challenge inbetween
    }
}
