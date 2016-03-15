namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        CompanyAddress = c.String(),
                        CompanyEmail = c.String(),
                        CompanyTelephone = c.String(),
                        IsPayingAdvertisedPackageCost = c.Boolean(nullable: false),
                        IsPayingMonthly = c.Boolean(nullable: false),
                        IsPayingAnnually = c.Boolean(nullable: false),
                        ActualPackageMonthlyPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActualPackageAnnualPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillingDate = c.DateTime(nullable: false),
                        RenewalDate = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        Package_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Package", t => t.Package_Id)
                .Index(t => t.Package_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        PlayerId = c.Int(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Age = c.Int(nullable: false),
                        GravitarURL = c.String(),
                        AccountId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlayerId)
                .ForeignKey("dbo.Account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Player", t => t.PlayerId)
                .Index(t => t.PlayerId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.KnockoutAdmin",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Knockout_Id = c.Int(),
                        User_PlayerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Knockout", t => t.Knockout_Id)
                .ForeignKey("dbo.User", t => t.User_PlayerId)
                .Index(t => t.Knockout_Id)
                .Index(t => t.User_PlayerId);
            
            CreateTable(
                "dbo.Knockout",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberOfRounds = c.Int(nullable: false),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LeagueAdmin",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        League_Id = c.Int(),
                        User_PlayerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.League", t => t.League_Id)
                .ForeignKey("dbo.User", t => t.User_PlayerId)
                .Index(t => t.League_Id)
                .Index(t => t.User_PlayerId);
            
            CreateTable(
                "dbo.League",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberOfPositions = c.Int(nullable: false),
                        NumberOfMatchUps = c.Int(nullable: false),
                        HasScheduledMatches = c.Boolean(nullable: false),
                        HasAdhocMatches = c.Boolean(nullable: false),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsCreated = c.Boolean(nullable: false),
                        SportType_Id = c.Int(),
                        Tournament_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SportType", t => t.SportType_Id)
                .ForeignKey("dbo.Tournament", t => t.Tournament_Id)
                .Index(t => t.SportType_Id)
                .Index(t => t.Tournament_Id);
            
            CreateTable(
                "dbo.LeagueCompetitor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InitialPositionNumber = c.Int(nullable: false),
                        CurrentPositionNumber = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        Played = c.Int(nullable: false),
                        Wins = c.Int(nullable: false),
                        Draws = c.Int(nullable: false),
                        Losses = c.Int(nullable: false),
                        League_Id = c.Int(),
                        Side_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.League", t => t.League_Id)
                .ForeignKey("dbo.Side", t => t.Side_Id)
                .Index(t => t.League_Id)
                .Index(t => t.Side_Id);
            
            CreateTable(
                "dbo.Side",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlayerProfile",
                c => new
                    {
                        PlayerId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlayerId)
                .ForeignKey("dbo.Player", t => t.PlayerId)
                .Index(t => t.PlayerId);
            
            CreateTable(
                "dbo.TeamProfile",
                c => new
                    {
                        TeamId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TeamId)
                .ForeignKey("dbo.Team", t => t.TeamId)
                .Index(t => t.TeamId);
            
            CreateTable(
                "dbo.Match",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScoreToWin = c.Int(nullable: false),
                        BestOf = c.Int(nullable: false),
                        IsDraw = c.Boolean(nullable: false),
                        DateTimeOfPlay = c.DateTime(),
                        MatchState_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MatchState", t => t.MatchState_Id)
                .Index(t => t.MatchState_Id);
            
            CreateTable(
                "dbo.MatchState",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SportType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tournament",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberOfPools = c.Int(nullable: false),
                        NumberOfRounds = c.Int(nullable: false),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Knockout_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Knockout", t => t.Knockout_Id)
                .Index(t => t.Knockout_Id);
            
            CreateTable(
                "dbo.TournamentAdmin",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tournament_Id = c.Int(),
                        User_PlayerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tournament", t => t.Tournament_Id)
                .ForeignKey("dbo.User", t => t.User_PlayerId)
                .Index(t => t.Tournament_Id)
                .Index(t => t.User_PlayerId);
            
            CreateTable(
                "dbo.CompetitionType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Package",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NumberOfSportTypes = c.Int(nullable: false),
                        NumberOfCompetitionTypes = c.Int(nullable: false),
                        NumberOfCompetitions = c.Int(nullable: false),
                        NumberOfSidesPerCompetition = c.Int(nullable: false),
                        HasLeagueDivisions = c.Boolean(nullable: false),
                        CanGenerateFixtures = c.Boolean(nullable: false),
                        CanViewStats = c.Boolean(nullable: false),
                        CanAccessApp = c.Boolean(nullable: false),
                        AdvertisedCostPerMonth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AdvertisedCostPerYear = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDiscountActive = c.Boolean(nullable: false),
                        AdvertisedDiscountedCostPerMonth = c.Decimal(precision: 18, scale: 2),
                        AdvertisedDiscountedCostPerYear = c.Decimal(precision: 18, scale: 2),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                        IsAvailable = c.Boolean(nullable: false),
                        PackageDiscount_Id = c.Int(),
                        PackageType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PackageDiscount", t => t.PackageDiscount_Id)
                .ForeignKey("dbo.PackageType", t => t.PackageType_Id)
                .Index(t => t.PackageDiscount_Id)
                .Index(t => t.PackageType_Id);
            
            CreateTable(
                "dbo.PackageDiscount",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DiscountPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActiveFrom = c.DateTime(),
                        ActiveTo = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PackageType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ActorType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Audit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActorName = c.String(),
                        SubjectName = c.String(),
                        DateTimeStamp = c.DateTime(nullable: false),
                        ActorType_Id = c.Int(),
                        LogType_Id = c.Int(),
                        SubjectType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActorType", t => t.ActorType_Id)
                .ForeignKey("dbo.LogType", t => t.LogType_Id)
                .ForeignKey("dbo.SubjectType", t => t.SubjectType_Id)
                .Index(t => t.ActorType_Id)
                .Index(t => t.LogType_Id)
                .Index(t => t.SubjectType_Id);
            
            CreateTable(
                "dbo.LogType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubjectType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeamPlayer",
                c => new
                    {
                        Team_Id = c.Int(nullable: false),
                        Player_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team_Id, t.Player_Id })
                .ForeignKey("dbo.Team", t => t.Team_Id)
                .ForeignKey("dbo.Player", t => t.Player_Id)
                .Index(t => t.Team_Id)
                .Index(t => t.Player_Id);
            
            CreateTable(
                "dbo.SportTypeAccount",
                c => new
                    {
                        SportType_Id = c.Int(nullable: false),
                        Account_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SportType_Id, t.Account_Id })
                .ForeignKey("dbo.SportType", t => t.SportType_Id, cascadeDelete: true)
                .ForeignKey("dbo.Account", t => t.Account_Id, cascadeDelete: true)
                .Index(t => t.SportType_Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.CompetitionTypeAccount",
                c => new
                    {
                        CompetitionType_Id = c.Int(nullable: false),
                        Account_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompetitionType_Id, t.Account_Id })
                .ForeignKey("dbo.CompetitionType", t => t.CompetitionType_Id, cascadeDelete: true)
                .ForeignKey("dbo.Account", t => t.Account_Id, cascadeDelete: true)
                .Index(t => t.CompetitionType_Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.PackageCompetitionType",
                c => new
                    {
                        Package_Id = c.Int(nullable: false),
                        CompetitionType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Package_Id, t.CompetitionType_Id })
                .ForeignKey("dbo.Package", t => t.Package_Id, cascadeDelete: true)
                .ForeignKey("dbo.CompetitionType", t => t.CompetitionType_Id, cascadeDelete: true)
                .Index(t => t.Package_Id)
                .Index(t => t.CompetitionType_Id);
            
            CreateTable(
                "dbo.ChallengeLeague",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.League", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.KnockoutMatch",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Match", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.LeagueMatch",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CompetitorA_Id = c.Int(),
                        CompetitorB_Id = c.Int(),
                        League_Id = c.Int(),
                        Loser_Id = c.Int(),
                        Winner_Id = c.Int(),
                        CompetitorAScore = c.Int(nullable: false),
                        CompetitorBScore = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Match", t => t.Id)
                .ForeignKey("dbo.LeagueCompetitor", t => t.CompetitorA_Id)
                .ForeignKey("dbo.LeagueCompetitor", t => t.CompetitorB_Id)
                .ForeignKey("dbo.League", t => t.League_Id)
                .ForeignKey("dbo.LeagueCompetitor", t => t.Loser_Id)
                .ForeignKey("dbo.LeagueCompetitor", t => t.Winner_Id)
                .Index(t => t.Id)
                .Index(t => t.CompetitorA_Id)
                .Index(t => t.CompetitorB_Id)
                .Index(t => t.League_Id)
                .Index(t => t.Loser_Id)
                .Index(t => t.Winner_Id);
            
            CreateTable(
                "dbo.Player",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Side", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.PointsLeague",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PointsForWin = c.Int(nullable: false),
                        PointsForDraw = c.Int(nullable: false),
                        PointsForLoss = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.League", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Side", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Team", "Id", "dbo.Side");
            DropForeignKey("dbo.PointsLeague", "Id", "dbo.League");
            DropForeignKey("dbo.Player", "Id", "dbo.Side");
            DropForeignKey("dbo.LeagueMatch", "Winner_Id", "dbo.LeagueCompetitor");
            DropForeignKey("dbo.LeagueMatch", "Loser_Id", "dbo.LeagueCompetitor");
            DropForeignKey("dbo.LeagueMatch", "League_Id", "dbo.League");
            DropForeignKey("dbo.LeagueMatch", "CompetitorB_Id", "dbo.LeagueCompetitor");
            DropForeignKey("dbo.LeagueMatch", "CompetitorA_Id", "dbo.LeagueCompetitor");
            DropForeignKey("dbo.LeagueMatch", "Id", "dbo.Match");
            DropForeignKey("dbo.KnockoutMatch", "Id", "dbo.Match");
            DropForeignKey("dbo.ChallengeLeague", "Id", "dbo.League");
            DropForeignKey("dbo.Audit", "SubjectType_Id", "dbo.SubjectType");
            DropForeignKey("dbo.Audit", "LogType_Id", "dbo.LogType");
            DropForeignKey("dbo.Audit", "ActorType_Id", "dbo.ActorType");
            DropForeignKey("dbo.Package", "PackageType_Id", "dbo.PackageType");
            DropForeignKey("dbo.Package", "PackageDiscount_Id", "dbo.PackageDiscount");
            DropForeignKey("dbo.PackageCompetitionType", "CompetitionType_Id", "dbo.CompetitionType");
            DropForeignKey("dbo.PackageCompetitionType", "Package_Id", "dbo.Package");
            DropForeignKey("dbo.Account", "Package_Id", "dbo.Package");
            DropForeignKey("dbo.CompetitionTypeAccount", "Account_Id", "dbo.Account");
            DropForeignKey("dbo.CompetitionTypeAccount", "CompetitionType_Id", "dbo.CompetitionType");
            DropForeignKey("dbo.User", "PlayerId", "dbo.Player");
            DropForeignKey("dbo.LeagueAdmin", "User_PlayerId", "dbo.User");
            DropForeignKey("dbo.TournamentAdmin", "User_PlayerId", "dbo.User");
            DropForeignKey("dbo.TournamentAdmin", "Tournament_Id", "dbo.Tournament");
            DropForeignKey("dbo.League", "Tournament_Id", "dbo.Tournament");
            DropForeignKey("dbo.Tournament", "Knockout_Id", "dbo.Knockout");
            DropForeignKey("dbo.League", "SportType_Id", "dbo.SportType");
            DropForeignKey("dbo.SportTypeAccount", "Account_Id", "dbo.Account");
            DropForeignKey("dbo.SportTypeAccount", "SportType_Id", "dbo.SportType");
            DropForeignKey("dbo.Match", "MatchState_Id", "dbo.MatchState");
            DropForeignKey("dbo.LeagueCompetitor", "Side_Id", "dbo.Side");
            DropForeignKey("dbo.TeamProfile", "TeamId", "dbo.Team");
            DropForeignKey("dbo.TeamPlayer", "Player_Id", "dbo.Player");
            DropForeignKey("dbo.TeamPlayer", "Team_Id", "dbo.Team");
            DropForeignKey("dbo.PlayerProfile", "PlayerId", "dbo.Player");
            DropForeignKey("dbo.LeagueCompetitor", "League_Id", "dbo.League");
            DropForeignKey("dbo.LeagueAdmin", "League_Id", "dbo.League");
            DropForeignKey("dbo.KnockoutAdmin", "User_PlayerId", "dbo.User");
            DropForeignKey("dbo.KnockoutAdmin", "Knockout_Id", "dbo.Knockout");
            DropForeignKey("dbo.User", "AccountId", "dbo.Account");
            DropIndex("dbo.Team", new[] { "Id" });
            DropIndex("dbo.PointsLeague", new[] { "Id" });
            DropIndex("dbo.Player", new[] { "Id" });
            DropIndex("dbo.LeagueMatch", new[] { "Winner_Id" });
            DropIndex("dbo.LeagueMatch", new[] { "Loser_Id" });
            DropIndex("dbo.LeagueMatch", new[] { "League_Id" });
            DropIndex("dbo.LeagueMatch", new[] { "CompetitorB_Id" });
            DropIndex("dbo.LeagueMatch", new[] { "CompetitorA_Id" });
            DropIndex("dbo.LeagueMatch", new[] { "Id" });
            DropIndex("dbo.KnockoutMatch", new[] { "Id" });
            DropIndex("dbo.ChallengeLeague", new[] { "Id" });
            DropIndex("dbo.PackageCompetitionType", new[] { "CompetitionType_Id" });
            DropIndex("dbo.PackageCompetitionType", new[] { "Package_Id" });
            DropIndex("dbo.CompetitionTypeAccount", new[] { "Account_Id" });
            DropIndex("dbo.CompetitionTypeAccount", new[] { "CompetitionType_Id" });
            DropIndex("dbo.SportTypeAccount", new[] { "Account_Id" });
            DropIndex("dbo.SportTypeAccount", new[] { "SportType_Id" });
            DropIndex("dbo.TeamPlayer", new[] { "Player_Id" });
            DropIndex("dbo.TeamPlayer", new[] { "Team_Id" });
            DropIndex("dbo.Audit", new[] { "SubjectType_Id" });
            DropIndex("dbo.Audit", new[] { "LogType_Id" });
            DropIndex("dbo.Audit", new[] { "ActorType_Id" });
            DropIndex("dbo.Package", new[] { "PackageType_Id" });
            DropIndex("dbo.Package", new[] { "PackageDiscount_Id" });
            DropIndex("dbo.TournamentAdmin", new[] { "User_PlayerId" });
            DropIndex("dbo.TournamentAdmin", new[] { "Tournament_Id" });
            DropIndex("dbo.Tournament", new[] { "Knockout_Id" });
            DropIndex("dbo.Match", new[] { "MatchState_Id" });
            DropIndex("dbo.TeamProfile", new[] { "TeamId" });
            DropIndex("dbo.PlayerProfile", new[] { "PlayerId" });
            DropIndex("dbo.LeagueCompetitor", new[] { "Side_Id" });
            DropIndex("dbo.LeagueCompetitor", new[] { "League_Id" });
            DropIndex("dbo.League", new[] { "Tournament_Id" });
            DropIndex("dbo.League", new[] { "SportType_Id" });
            DropIndex("dbo.LeagueAdmin", new[] { "User_PlayerId" });
            DropIndex("dbo.LeagueAdmin", new[] { "League_Id" });
            DropIndex("dbo.KnockoutAdmin", new[] { "User_PlayerId" });
            DropIndex("dbo.KnockoutAdmin", new[] { "Knockout_Id" });
            DropIndex("dbo.User", new[] { "AccountId" });
            DropIndex("dbo.User", new[] { "PlayerId" });
            DropIndex("dbo.Account", new[] { "Package_Id" });
            DropTable("dbo.Team");
            DropTable("dbo.PointsLeague");
            DropTable("dbo.Player");
            DropTable("dbo.LeagueMatch");
            DropTable("dbo.KnockoutMatch");
            DropTable("dbo.ChallengeLeague");
            DropTable("dbo.PackageCompetitionType");
            DropTable("dbo.CompetitionTypeAccount");
            DropTable("dbo.SportTypeAccount");
            DropTable("dbo.TeamPlayer");
            DropTable("dbo.SubjectType");
            DropTable("dbo.LogType");
            DropTable("dbo.Audit");
            DropTable("dbo.ActorType");
            DropTable("dbo.PackageType");
            DropTable("dbo.PackageDiscount");
            DropTable("dbo.Package");
            DropTable("dbo.CompetitionType");
            DropTable("dbo.TournamentAdmin");
            DropTable("dbo.Tournament");
            DropTable("dbo.SportType");
            DropTable("dbo.MatchState");
            DropTable("dbo.Match");
            DropTable("dbo.TeamProfile");
            DropTable("dbo.PlayerProfile");
            DropTable("dbo.Side");
            DropTable("dbo.LeagueCompetitor");
            DropTable("dbo.League");
            DropTable("dbo.LeagueAdmin");
            DropTable("dbo.Knockout");
            DropTable("dbo.KnockoutAdmin");
            DropTable("dbo.User");
            DropTable("dbo.Account");
        }
    }
}
