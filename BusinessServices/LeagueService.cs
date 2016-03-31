using BusinessServices.Builders;
using BusinessServices.Builders.LeagueCompetition;
using BusinessServices.Dtos;
using BusinessServices.Interfaces;
using BusinessServices.Managers;
using BusinessServices.Managers.LeagueCompetition;
using BusinessServices.Sports;
using Core.Extensions;
using DAL;
using Model.Actors;
using Model.Competitors;
using Model.LeagueArrangements;
using Model.Leagues;
using Model.ReferenceData;
using Model.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessServices
{
    public interface ILeagueService
    {
        IList<LeagueTableDto> GetLeagueStandings(int leagueId);
        IList<LeagueCompetitor> GetLeagueCompetitors(int leagueId);
        void CreatePointsLeague(string leagueName, int durationInDays, IMatchScheduler matchScheduler);
        void CreateChallengeLeague(string leagueName, int durationInDays, IMatchScheduler matchScheduler);
        void AwardPointsLeagueWin(int matchId, int winnerId, int loserId);
        void AwardPointsLeagueDraw(int matchId);
        void ActivateLeague(int leagueId);
        void AddCompetitorToLeague(int leagueId, int competitorId);
    }

    public class LeagueService : ILeagueService
    {
        private IUnitOfWork _unitOfWork;

        public LeagueService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region General
        public IList<LeagueTableDto> GetLeagueStandings(int leagueId)
        {
            // return from the manager based on the type of league it is
            return null;
        }

        public IList<LeagueCompetitor> GetLeagueCompetitors(int leagueId)
        {
            var league = _unitOfWork.GetRepository<League>().GetById(leagueId);
            var competitors = league.LeagueCompetitors.ToList();

            return competitors;
        } 
        #endregion

        #region Cluster Management
        public void CreateLeagueCluster(string clusterName, int numberOfDivisions)
        {
            // TODO: do you have permissions to create clusters? - are you Admin, what is your package?

            Cluster cluster = new Cluster();
            cluster.Name = clusterName;
            cluster.NumberOfDivisions = numberOfDivisions;
            _unitOfWork.GetRepository<Cluster>().Add(cluster);
            _unitOfWork.Save();
        }

        public void RenameCluster(int clusterId, string name)
        {
            Cluster cluster = _unitOfWork.GetRepository<Cluster>().GetById(clusterId);

            ClusterManager manager = new ClusterManager(cluster);
            manager.ChangeName(name);

            _unitOfWork.Save();
        }

        public void ActivateCluster(int clusterId)
        {
            Cluster cluster = _unitOfWork.GetRepository<Cluster>().GetById(clusterId);

            ClusterManager manager = new ClusterManager(cluster);
            manager.Activate();

            _unitOfWork.Save();
        }

        public void ReorderClusterDivisions(int clusterId)
        {
            // TODO: do you have permissions to reorder clusters? - are you Admin, what is your package?

            Cluster cluster = _unitOfWork.GetRepository<Cluster>().GetById(clusterId);

            if (cluster.IsActive)
                throw new Exception("You cannot reorder divisions once the cluster is active");

                
            
        }

        public void AddLeaguesToCluster(int clusterId, int[] leagueIds)
        {
            // TODO: do you have permissions to add leagues clusters?

            Cluster cluster = _unitOfWork.GetRepository<Cluster>().GetById(clusterId);
            IEnumerable<League> leagues = _unitOfWork.GetRepository<League>().Find(l => leagueIds.Contains(l.Id));

            ClusterManager manager = new ClusterManager(cluster);
            manager.AddLeagues(leagues);

            _unitOfWork.Save();
        }

        public void RemoveLeaguesFromCluster(int clusterId, int[] leagueIds)
        {
            // TODO: do you have permissions to remove leagues clusters? - are you Admin, what is your package, is league/season active?

            Cluster cluster = _unitOfWork.GetRepository<Cluster>().GetById(clusterId);
            IEnumerable<League> leagues = _unitOfWork.GetRepository<League>().Find(l => leagueIds.Contains(l.Id));

            ClusterManager manager = new ClusterManager(cluster);
            manager.RemoveLeagues(leagues);

            _unitOfWork.Save();
        }
        #endregion

        #region Season Management
        public void CreateSeason(string name)
        {
            // TODO: do you have permissions?

            Season season = new Season() { Name = name };
            _unitOfWork.GetRepository<Season>().Add(season);

            _unitOfWork.Save();
        }

        public void ActivateSeason(int seasonId)
        {
            // TODO: do you have permissions?

            Season season = _unitOfWork.GetRepository<Season>().GetById(seasonId);
            season.IsActive = true;

            _unitOfWork.Save();
        }

        public void AddLeagueToSeason(int leagueId, int seasonId)
        {
            Season season = _unitOfWork.GetRepository<Season>().GetById(seasonId);
            League league = _unitOfWork.GetRepository<League>().GetById(leagueId);

            if (season.League != null)
                throw new Exception("This season already contains a league. To add multiple leagues to a season you must create divisions");

            season.League = league;

            _unitOfWork.Save();
        }

        public void EndOfSeason()
        {
            // TODO: Implement
        }
        #endregion

        #region League Creation
        public void CreatePointsLeague(string leagueName, int durationInDays, IMatchScheduler matchScheduler)
        {
            List<Side> sides = _unitOfWork.GetRepository<Side>().All().ToList();

            IAuditLogger auditLogger = new AuditLogger(_unitOfWork);

            CompetitionType competitionType = _unitOfWork.GetRepository<CompetitionType>().Find(ct => ct.Name == "Points").SingleOrDefault();
            SportType sportType = _unitOfWork.GetRepository<SportType>().Find(st => st.Name == "Football").SingleOrDefault();

            LeagueConfig leagueConfig = new LeagueConfig()
            {
                Name = leagueName,
                NumberOfMatchUps = 4,
                NumberOfPositions = 5,
                Sides = sides,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(durationInDays),
                AuditLogger = auditLogger
            };

            LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>(leagueConfig);

            PointsLeague newPointsLeague = new PointsLeague();
            LeagueBuilder<PointsLeague> builder = new LeagueBuilder<PointsLeague>(newPointsLeague, matchScheduler);

            director.Construct(builder);

            _unitOfWork.GetRepository<League>().Add(newPointsLeague);

            _unitOfWork.Save();
        }

        public void CreateChallengeLeague(string leagueName, int durationInDays, IMatchScheduler matchScheduler)
        {
            List<Side> sides = _unitOfWork.GetRepository<Side>().All().ToList();
            IAuditLogger auditLogger = new AuditLogger(_unitOfWork);

            CompetitionType competitionType = _unitOfWork.GetRepository<CompetitionType>().Find(ct => ct.Name == "Challenge").SingleOrDefault();
            SportType sportType = _unitOfWork.GetRepository<SportType>().Find(st => st.Name == "Football").SingleOrDefault();

            LeagueConfig leagueConfig = new LeagueConfig()
            {
                Name = leagueName,
                NumberOfMatchUps = 4,
                NumberOfPositions = 5,
                Sides = sides,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(durationInDays),
                AuditLogger = auditLogger
            };

            LeagueBuilderDirector<ChallengeLeague> director = new LeagueBuilderDirector<ChallengeLeague>(leagueConfig);

            ChallengeLeague newChallengeLeague = new ChallengeLeague();

            LeagueBuilder<ChallengeLeague> builder = new LeagueBuilder<ChallengeLeague>(newChallengeLeague, matchScheduler);

            director.Construct(builder);

            _unitOfWork.GetRepository<League>().Add(newChallengeLeague);

            _unitOfWork.Save();
        }

        public void AddCompetitorToLeague(int leagueId, int sideId)
        {
            League league = _unitOfWork.GetRepository<League>().GetById(leagueId);
            Side side = _unitOfWork.GetRepository<Side>().GetById(sideId);

            // TODO:

            _unitOfWork.Save();
        }

        public void AddCompetitorsToLeague(int leagueId, int[] sideIds)
        {
            League league = _unitOfWork.GetRepository<League>().GetById(leagueId);
            IEnumerable<Side> sides = _unitOfWork.GetRepository<Side>().All().Where(s => sideIds.Contains(s.Id));

            // TODO:

            //foreach (var side in sides)
            //{
            //    AddCompetitorToLeague(league, side);
            //}
            _unitOfWork.Save();
        }

        public void ActivateLeague(int leagueId)
        {
            League league = _unitOfWork.GetRepository<League>().GetById(leagueId);

            if (!league.IsCreated)
                throw new ApplicationException("League cannot be activated until it is completely created");

            if (league.IsActive)
                throw new ApplicationException("League cannot be activated because it is already active");

            league.IsActive = true;

            _unitOfWork.Save();
        }

        #endregion

        #region Results
        public void AwardPointsLeagueWin(int matchId, int winnerId, int loserId)
        {
            LeagueMatch leagueMatch = _unitOfWork.GetRepository<LeagueMatch>().GetById(matchId);
            PointsLeague pointsLeague = _unitOfWork.GetRepository<PointsLeague>().GetById(leagueMatch.League.Id);

            LeagueCompetitor winner = pointsLeague.LeagueCompetitors.Single(lc => lc.Id == winnerId);
            LeagueCompetitor loser = pointsLeague.LeagueCompetitors.Single(lc => lc.Id == loserId);

            ISportManager footballManager = new FootballManager(pointsLeague.CompetitionType, new PointsDto() { Win = pointsLeague.PointsForWin, Draw = pointsLeague.PointsForDraw, Loss = pointsLeague.PointsForDraw });

            PointsLeagueManager manager = new PointsLeagueManager(pointsLeague, footballManager);
            manager.AwardWin(leagueMatch, winner, loser);

            _unitOfWork.Save();
        }

        public void AwardPointsLeagueDraw(int matchId)
        {
            LeagueMatch leagueMatch = _unitOfWork.GetRepository<LeagueMatch>().GetById(matchId);
            PointsLeague pointsLeague = _unitOfWork.GetRepository<PointsLeague>().GetById(leagueMatch.League.Id);

            LeagueCompetitor competitorA = pointsLeague.LeagueCompetitors.Single(lc => lc.Id == leagueMatch.CompetitorA.Id);
            LeagueCompetitor competitorB = pointsLeague.LeagueCompetitors.Single(lc => lc.Id == leagueMatch.CompetitorB.Id);

            ISportManager footballManager = new FootballManager(pointsLeague.CompetitionType, new PointsDto() { Win = pointsLeague.PointsForWin, Draw = pointsLeague.PointsForDraw, Loss = pointsLeague.PointsForDraw });

            PointsLeagueManager updater = new PointsLeagueManager(pointsLeague, footballManager);
            updater.AwardDraw(leagueMatch, competitorA, competitorB);

            _unitOfWork.Save();
        }

        public void AwardChallengeLeagueWin(int matchId, int winnerId, int loserId)
        {
            LeagueMatch leagueMatch = _unitOfWork.GetRepository<LeagueMatch>().GetById(matchId);
            ChallengeLeague challengeLeague = _unitOfWork.GetRepository<ChallengeLeague>().GetById(leagueMatch.League.Id);

            LeagueCompetitor winner = challengeLeague.LeagueCompetitors.Single(lc => lc.Id == winnerId);
            LeagueCompetitor loser = challengeLeague.LeagueCompetitors.Single(lc => lc.Id == loserId);

            leagueMatch.Winner = winner;
            leagueMatch.Loser = loser;

            ISportManager footballManager = new FootballManager(challengeLeague.CompetitionType);

            ChallengeLeagueManager manager = new ChallengeLeagueManager(challengeLeague, footballManager);

            manager.AwardWin(leagueMatch, winner, loser);

            _unitOfWork.Save();
        } 
        #endregion

        #region Matches
        /// <summary>
        /// WAHT DOES THIS DO?
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="competitorAId"></param>
        /// <param name="competitorBId"></param>
        public void AddPointsLeagueMatch(int leagueId, int competitorAId, int competitorBId)
        {
            PointsLeague pointsLeague = _unitOfWork.GetRepository<PointsLeague>().GetById(leagueId);

            LeagueCompetitor competitorA = _unitOfWork.GetRepository<LeagueCompetitor>().GetById(competitorAId);
            LeagueCompetitor competitorB = _unitOfWork.GetRepository<LeagueCompetitor>().GetById(competitorBId);

            ISportManager footballManager = new FootballManager(pointsLeague.CompetitionType, new PointsDto() { Win = pointsLeague.PointsForWin, Draw = pointsLeague.PointsForDraw, Loss = pointsLeague.PointsForDraw });

            PointsLeagueManager manager = new PointsLeagueManager(pointsLeague, footballManager);
            manager.AddMatch(competitorA, competitorB);

        }

        public void AddChallengeMatch(int leagueId, int competitorAId, int competitorBId)
        {
            ChallengeLeague challengeLeague = _unitOfWork.GetRepository<ChallengeLeague>().GetById(leagueId);

            LeagueCompetitor competitorA = _unitOfWork.GetRepository<LeagueCompetitor>().GetById(competitorAId);
            LeagueCompetitor competitorB = _unitOfWork.GetRepository<LeagueCompetitor>().GetById(competitorBId);

            ISportManager footballManager = new FootballManager(challengeLeague.CompetitionType);

            ChallengeLeagueManager manager = new ChallengeLeagueManager(challengeLeague, footballManager);
            manager.AddMatch(competitorA, competitorB);
        } 
        #endregion

        #region End of League
        public void RenewPointsLeague(int leagueId)
        {
            PointsLeague pointsLeague = _unitOfWork.GetRepository<PointsLeague>().GetById(leagueId);

            if (pointsLeague.Cluster != null)
                throw new Exception("You cannot renew this Points League because it exists as part of a cluster");

            ISportManager footballManager = new FootballManager(pointsLeague.CompetitionType, new PointsDto() { Win = pointsLeague.PointsForWin, Draw = pointsLeague.PointsForDraw, Loss = pointsLeague.PointsForDraw });
            PointsLeagueManager manager = new PointsLeagueManager(pointsLeague, footballManager);

            manager.RenewLeague();
        }

        public void FinalisePointsLeague(int leagueId)
        {
            PointsLeague pointsLeague = _unitOfWork.GetRepository<PointsLeague>().GetById(leagueId);

            ISportManager footballManager = new FootballManager(pointsLeague.CompetitionType, new PointsDto() { Win = pointsLeague.PointsForWin, Draw = pointsLeague.PointsForDraw, Loss = pointsLeague.PointsForDraw });
            PointsLeagueManager manager = new PointsLeagueManager(pointsLeague, footballManager);
            manager.Finalise();

            _unitOfWork.Save();
        }

        public void RenewChallengeLeague(int leagueId)
        {
            ChallengeLeague challengeLeague = _unitOfWork.GetRepository<ChallengeLeague>().GetById(leagueId);

            if (challengeLeague.Cluster != null)
                throw new Exception("You cannot renew this Challenge League because it exists as part of a cluster");

            ISportManager footballManager = new FootballManager(challengeLeague.CompetitionType);
            ChallengeLeagueManager manager = new ChallengeLeagueManager(challengeLeague, footballManager);

            manager.RenewLeague();
        }

        public void FinaliseChallengeLeague(int leagueId)
        {
            ChallengeLeague challengeLeague = _unitOfWork.GetRepository<ChallengeLeague>().GetById(leagueId);

            ISportManager footballManager = new FootballManager(challengeLeague.CompetitionType);
            ChallengeLeagueManager manager = new ChallengeLeagueManager(challengeLeague, footballManager);
            manager.Finalise();

            _unitOfWork.Save();
        } 
        #endregion
    }
}
