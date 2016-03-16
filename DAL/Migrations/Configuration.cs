namespace DAL.Migrations
{
    using Model;
    using Model.Actors;
    using Model.Leagues;
    using Model.Scheduling;
    using Model.ReferenceData;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Model.Packages;
    using System.Collections.Generic;
    using Model.UserManagement;
    using Model.Sports;
    using Model.Record;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.ActiveOfficeContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.ActiveOfficeContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //

            SportType Badminton = new SportType() { Id = 1, Name = "Badminton" };
            SportType GoKarting = new SportType() { Id = 2, Name = "GoKarting" };
            SportType Foosball = new SportType() { Id = 3, Name = "Foosball" };
            SportType Football = new SportType() { Id = 4, Name = "Football" };
            SportType Hockey = new SportType() { Id = 5, Name = "Hockey" };
            SportType Netball = new SportType() { Id = 6, Name = "Netball" };
            SportType Rugby = new SportType() { Id = 7, Name = "Rugby" };
            SportType TableTennis = new SportType() { Id = 8, Name = "Table Tennis" };
            SportType Tennis = new SportType() { Id = 9, Name = "Tennis" };

            context.SportTypes.AddOrUpdate(
              s => s.Name,
              Badminton, GoKarting, Foosball, Football, Hockey, Netball, Rugby, TableTennis, Tennis
            );

            CompetitionType knockout = new CompetitionType() { Name = "Knockout" };
            CompetitionType pointsLeague = new CompetitionType() { Name = "PointsLeague" };
            CompetitionType challengeLeague = new CompetitionType() { Name = "ChallengeLeague" };
            CompetitionType tournament = new CompetitionType() { Name = "Tournament" };

            context.CompetitionTypes.AddOrUpdate(
              l => l.Name,
              knockout, pointsLeague, challengeLeague, tournament
            );

            SportColumn Played = new SportColumn() { Id = 1, Name = "Played" };
            SportColumn Points = new SportColumn() { Id = 2, Name = "Points" };
            SportColumn Wins = new SportColumn() { Id = 3, Name = "Wins" };
            SportColumn Draws = new SportColumn() { Id = 4, Name = "Draws" };
            SportColumn Losses = new SportColumn() { Id = 5, Name = "Losses" };
            SportColumn GoalsFor = new SportColumn() { Id = 6, Name = "GoalsFor" };
            SportColumn GoalsAgainst = new SportColumn() { Id = 7, Name = "GoalsAgainst" };
            SportColumn GoalDifference = new SportColumn() { Id = 8, Name = "GoalDifference" };
            SportColumn Laps = new SportColumn() { Id = 9, Name = "Laps" };
            SportColumn Races = new SportColumn() { Id = 10, Name = "Races" };

            context.SportColumns.AddOrUpdate(
              sc => sc.Name,
              Played, Points, Wins, Draws, Losses, GoalsFor, GoalsAgainst, Laps 
            );

            context.CompetitionTypeSportColumns.AddOrUpdate(
              x => x.Id,
              new CompetitionTypeSportColumn() { Id = 1, CompetitionType = pointsLeague, SportType = Football, SportColumn = Played, ColumnOrder = 1, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 2, CompetitionType = pointsLeague, SportType = Football, SportColumn = Wins, ColumnOrder = 2, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 3, CompetitionType = pointsLeague, SportType = Football, SportColumn = Draws, ColumnOrder = 3, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 4, CompetitionType = pointsLeague, SportType = Football, SportColumn = Losses, ColumnOrder = 4, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 5, CompetitionType = pointsLeague, SportType = Football, SportColumn = GoalsFor, ColumnOrder = 5, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 6, CompetitionType = pointsLeague, SportType = Football, SportColumn = GoalsAgainst, ColumnOrder = 6, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 7, CompetitionType = pointsLeague, SportType = Football, SportColumn = Points, ColumnOrder = 7, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 9, CompetitionType = pointsLeague, SportType = Football, SportColumn = GoalDifference, ColumnOrder = 7, IsOptional = false },

              new CompetitionTypeSportColumn() { Id = 10, CompetitionType = challengeLeague, SportType = Football, SportColumn = Played, ColumnOrder = 1, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 11, CompetitionType = challengeLeague, SportType = Football, SportColumn = Wins, ColumnOrder = 2, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 12, CompetitionType = challengeLeague, SportType = Football, SportColumn = Draws, ColumnOrder = 3, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 13, CompetitionType = challengeLeague, SportType = Football, SportColumn = Losses, ColumnOrder = 4, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 14, CompetitionType = challengeLeague, SportType = Football, SportColumn = GoalsFor, ColumnOrder = 5, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 15, CompetitionType = challengeLeague, SportType = Football, SportColumn = GoalsAgainst, ColumnOrder = 6, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 16, CompetitionType = challengeLeague, SportType = Football, SportColumn = GoalDifference, ColumnOrder = 7, IsOptional = false },
              
              new CompetitionTypeSportColumn() { Id = 17, CompetitionType = pointsLeague, SportType = GoKarting, SportColumn = Points, ColumnOrder = 1, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 18, CompetitionType = pointsLeague, SportType = GoKarting, SportColumn = Wins, ColumnOrder = 2, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 19, CompetitionType = pointsLeague, SportType = GoKarting, SportColumn = Races, ColumnOrder = 3, IsOptional = false },
              new CompetitionTypeSportColumn() { Id = 20, CompetitionType = pointsLeague, SportType = GoKarting, SportColumn = Laps, ColumnOrder = 4, IsOptional = false }
            );
            
            context.Players.AddOrUpdate(
              s => s.Name,
              new Player() { Id = 1, Name = "Alex" }
            );

            context.PlayerRecords.AddOrUpdate(
              s => s.PlayerId,
              new PlayerRecord() { PlayerId = 1 }
            );

            context.Accounts.AddOrUpdate(
              s => s.Id,
              new Account() 
              { 
                  Id = 1,
                  CompanyName = "My Company",
                  ExpiryDate = DateTime.Now.AddDays(100),
                  RenewalDate = DateTime.Now.AddYears(1),
                  BillingDate = DateTime.Now.AddDays(2)
              }
            );

            context.Users.AddOrUpdate(
              s => s.Email,
              new User() 
              { 
                  Email = "alexwilliams57@hotmail.com",
                  Age = 30,
                  PlayerId = 1,
                  Name = "Alex",
                  AccountId = 1
              }
            );

            context.MatchStates.AddOrUpdate(
              s => s.Name,
              new MatchState() { Name = "Played" },
              new MatchState() { Name = "Postponed" },
              new MatchState() { Name = "Forfeited" }
            );

            context.Packages.AddOrUpdate(
              p => p.Name,
              new Package() 
                { 
                    Name = "Free",
                    NumberOfSidesPerCompetition = 10,
                    NumberOfCompetitions = 1,
                    NumberOfSportTypes = 1,
                    CompetitionTypes = new List<CompetitionType>() { pointsLeague },
                    HasLeagueDivisions = false,
                    CanGenerateFixtures = true,
                    CanViewStats = false,
                    CanAccessApp = true,
                    AdvertisedCostPerMonth = 0.00M,
                    AdvertisedCostPerYear = 0.00M,
                    IsDiscountActive = false
                },
                new Package()
                {
                    Name = "Standard",
                    NumberOfSidesPerCompetition = 10,
                    NumberOfCompetitions = 5,
                    NumberOfSportTypes = 1,
                    CompetitionTypes = new List<CompetitionType>() { pointsLeague },
                    HasLeagueDivisions = false,
                    CanGenerateFixtures = true,
                    CanViewStats = false,
                    CanAccessApp = true,
                    AdvertisedCostPerMonth = 5.00M,
                    AdvertisedCostPerYear = 60.00M,
                    IsDiscountActive = false
                },
                new Package()
                {
                    Name = "Pro",
                    NumberOfSidesPerCompetition = 10,
                    NumberOfCompetitions = 999,
                    NumberOfSportTypes = 1,
                    CompetitionTypes = new List<CompetitionType>() { pointsLeague },
                    HasLeagueDivisions = true,
                    CanGenerateFixtures = true,
                    CanViewStats = false,
                    CanAccessApp = true,
                    AdvertisedCostPerMonth = 10.00M,
                    AdvertisedCostPerYear = 120.00M,
                    IsDiscountActive = false
                }
            );


            //Player p1 = new Player { Name = "Alex" };
            //Player p2 = new Player { Name = "John" };
            //Player p3 = new Player { Name = "Peter" };
            //Player p4 = new Player { Name = "Archie" };

            //context.Players.AddOrUpdate(
            //  p => p.Name,
            //  p1, p2, p3, p4
            //);

            //PointsLeague pl1 = new PointsLeague { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30), Name = "Premier League" };
            //PointsLeague pl2 = new PointsLeague { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(25), Name = "Championship" };

            //context.PointsLeagues.AddOrUpdate(
            //  p => p.Name,
            //  pl1, pl2
            //);

            //LeagueCompetitor lc1 = new LeagueCompetitor { League = pl1, Points = 9, Played = 3, Wins = 3, Losses = 0, Draws = 0, Side = p1 };
            //LeagueCompetitor lc2 = new LeagueCompetitor { League = pl1, Points = 4, Played = 3, Wins = 1, Losses = 0, Draws = 1, Side = p2 };
            //LeagueCompetitor lc3 = new LeagueCompetitor { League = pl1, Points = 2, Played = 3, Wins = 0, Losses = 1, Draws = 2, Side = p3 };
            //LeagueCompetitor lc4 = new LeagueCompetitor { League = pl1, Points = 1, Played = 3, Wins = 0, Losses = 2, Draws = 1, Side = p4 };
            //LeagueCompetitor lc5 = new LeagueCompetitor { League = pl2, Played = 0, Wins = 0, Losses = 0, Draws = 0, Side = p3 };

            //context.LeagueCompetitors.AddOrUpdate(
            //  lc1, lc2, lc3, lc4, lc5
            //);

            //Match m1 = new Match() { LeagueCompetitorA = lc1, LeagueCompetitorB = lc2 };
            //Match m2 = new Match() { LeagueCompetitorA = lc2, LeagueCompetitorB = lc1 };

            //context.Matches.AddOrUpdate(
            //  p => p.Id,
            //  m1, m2
            //);

        }
    }
}
