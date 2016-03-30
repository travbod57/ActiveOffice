using Model;
using Model.Actors;
using Model.CompetitionOwnership;
using Model.Competitors;
using Model.Knockouts;
using Model.LeagueArrangements;
using Model.Leagues;
using Model.Packages;
using Model.Record;
using Model.ReferenceData;
using Model.Schedule;
using Model.Scheduling;
using Model.Tournaments;
using Model.UserManagement;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DAL
{
    public class ActiveOfficeContext : DbContext
    {
        public ActiveOfficeContext() : base("ActiveOfficeContext")
        {

        }

        #region Actors
        public DbSet<Player> Players { get; set; }
        public DbSet<Side> Sides { get; set; }
        public DbSet<Team> Teams { get; set; }

        #endregion

        #region CompetitionOwnership
        public DbSet<KnockoutAdmin> KnockoutAdmins { get; set; }
        public DbSet<LeagueAdmin> LeagueAdmins { get; set; }
        public DbSet<TournamentAdmin> TournamentAdmins { get; set; } 
        #endregion

        #region Competitors
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<LeagueCompetitor> LeagueCompetitors { get; set; }
        public DbSet<TournamentCompetitor> TournamentCompetitors { get; set; }
        #endregion

        #region Knockout
        public DbSet<Knockout> Knockouts { get; set; } 
        #endregion

        #region LeagueArrangements
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Cluster> Clusters { get; set; } 
        #endregion

        #region Leagues
        public DbSet<ChallengeLeague> ChallengeLeagues { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<PointsLeague> PointsLeagues { get; set; } 
        #endregion

        #region Packages
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageDiscount> PackageDiscounts { get; set; }
        public DbSet<PackageType> PackageTypes { get; set; } 
        #endregion

        #region Record
        public DbSet<CompetitorRecord> CompetitorRecords { get; set; } 
        public DbSet<CompetitorHistoryRecord> CompetitorHistoryRecords { get; set; }
        public DbSet<TeamRecord> TeamRecords { get; set; }
        public DbSet<PlayerRecord> PlayerRecords { get; set; }
        #endregion

        #region ReferenceData
        public DbSet<CompetitionType> CompetitionTypes { get; set; }
        public DbSet<SportType> SportTypes { get; set; } 
        #endregion

        #region Schedule
        public DbSet<KnockoutMatch> KnockoutMatches { get; set; }
        public DbSet<LeagueMatch> LeagueMatches { get; set; }
        public DbSet<Match> Matches { get; set; }
        #endregion

        #region Sports

        #endregion

        #region Tournament
        public DbSet<Tournament> Tournaments { get; set; } 
        #endregion

        #region UserManagement
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; } 
        #endregion

        public DbSet<Audit> Audits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ActiveOfficeContext>(new CreateDatabaseIfNotExists<ActiveOfficeContext>());
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
