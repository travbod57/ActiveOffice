using BusinessServices.Builders;
using BusinessServices.Dto;
using BusinessServices.Dtos;
using BusinessServices.Interfaces;
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
using Model.Sports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessServices
{
    public interface ILeagueService
    {
        IList<LeagueDto> GetLeagueStandings(int leagueId);
        IList<LeagueCompetitor> GetLeagueCompetitors(int leagueId);
        League GetLeague(int leagueId);
        void CreatePointsLeague(string leagueName, int durationInDays, IMatchScheduler matchScheduler);
        void CreateChallengeLeague(string leagueName, int durationInDays, IMatchScheduler matchScheduler);
        void AwardPointsLeagueWin(int matchId, int winnerId, int loserId);
        void AwardPointsLeagueDraw(int matchId);
        void ActivateLeague(int leagueId);
        void AddCompetitorToLeague(int leagueId, int competitorId);
        void RenewLeague(int leagueId);
    }

    public class LeagueService : ILeagueService
    {
        private IUnitOfWork _unitOfWork;

        public LeagueService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void ActivateLeague(int leagueId)
        {
            League league = _unitOfWork.GetRepository<League>().GetById(leagueId);

            if (!league.IsCreated)
                throw new ApplicationException("League cannot be activated until it is completely created");

            league.IsActive = true;

            _unitOfWork.Save();
        }


        public League GetLeague(int leagueId)
        {
            return _unitOfWork.GetRepository<League>().GetById(leagueId);
        }

        public IList<LeagueDto> GetLeagueStandings(int leagueId)
        {
            var league = (from l in _unitOfWork.GetRepository<League>().All()
                          join lc in _unitOfWork.GetRepository<LeagueCompetitor>().All() on l.Id equals lc.League.Id
                          where l.Id == leagueId
                          orderby lc.CurrentPositionNumber
                          select new LeagueDto()
                          {
                              SideName = lc.Side.Name,
                              CompetitorStandings = lc.CompetitorRecords.ToList()
                          }).ToList();

            return league;
        }

        public IList<LeagueCompetitor> GetLeagueCompetitors(int leagueId)
        {
            var league = _unitOfWork.GetRepository<League>().GetById(leagueId);
            var competitors = league.LeagueCompetitors.ToList();

            return competitors;
        }

        public void AddCompetitorToLeague(int leagueId, int sideId)
        {
            League league = GetLeague(leagueId);
            Side side = _unitOfWork.GetRepository<Side>().GetById(sideId);

            AddCompetitorToLeague(league, side);

            _unitOfWork.Save();
        }

        public void AddCompetitorsToLeague(int leagueId, int[] sideIds)
        {
            League league = GetLeague(leagueId);
            IEnumerable<Side> sides = _unitOfWork.GetRepository<Side>().All().Where(s => sideIds.Contains(s.Id));

            foreach (var side in sides)
            {
                AddCompetitorToLeague(league, side);
            }
            _unitOfWork.Save();
        }

        private void AddCompetitorToLeague(League league, Side side)
        {
            league.LeagueCompetitors.Add(new LeagueCompetitor() { Side = side, InitialPositionNumber = league.LeagueCompetitors.Count + 1 });
        }

        public void AddChallengeMatch(int leagueId, int competitorAId, int competitorBId)
        {
            ChallengeLeague challengeLeague = _unitOfWork.GetRepository<ChallengeLeague>().GetById(leagueId);

            LeagueCompetitor competitorA = _unitOfWork.GetRepository<LeagueCompetitor>().GetById(competitorAId);
            LeagueCompetitor competitorB = _unitOfWork.GetRepository<LeagueCompetitor>().GetById(competitorBId);

            ISportManager footballManager = new FootballManager(challengeLeague.CompetitionType);

            LeagueManager manager = new LeagueManager(new ChallengeLeagueManager(challengeLeague, footballManager));
            manager.AddMatch(competitorA, competitorB);

        }

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

        public void ReorderClusterDivisions(int clusterId)
        {
            // TODO: if active cannot reorder
            // TODO: do you have permissions to reorder clusters? - are you Admin, what is your package?
        }

        public void AddLeaguesToCluster(int clusterId, int[] leagueIds)
        {
            // TODO: do you have permissions to add leagues clusters?

            Cluster cluster = _unitOfWork.GetRepository<Cluster>().GetById(clusterId);

            IEnumerable<League> leagues = _unitOfWork.GetRepository<League>().Find(l => leagueIds.Contains(l.Id));

            if (leagues.Intersect(cluster.Leagues).Count() > 0)
                throw new Exception("You are attempting to add leagues to a cluster that are already in the cluster");

            IEnumerable<League> allLeagues = leagues.Union(cluster.Leagues);
            bool sameCompetitionType = allLeagues.Select(x => x.CompetitionType).Distinct().Count() == 1;

            if (!sameCompetitionType)
                throw new Exception("Clusters are not allowed to have leagues of different types");

            // add the new leagues to the cluster
            cluster.Leagues.AddRange<League>(leagues);
            _unitOfWork.Save();
        }

        public void RemoveLeaguesFromCluster(int clusterId, int[] leagueIds)
        {
            // TODO: do you have permissions to remove leagues clusters? - are you Admin, what is your package?

            Cluster cluster = _unitOfWork.GetRepository<Cluster>().GetById(clusterId);

            IEnumerable<League> leagues = _unitOfWork.GetRepository<League>().Find(l => leagueIds.Contains(l.Id));

            if (leagues.Count(l => l.IsActive) > 0)
                throw new Exception("One of the leagues you are trying to remove is active");

            cluster.Leagues.RemoveRange(leagues);
            _unitOfWork.Save();
        }
        #endregion

        #region Season Management
        public void CreateSeason()
        {

        }

        public void ActivateSeason(int seasonId)
        {

        }
        #endregion

        public void CreatePointsLeague(string leagueName, int durationInDays, IMatchScheduler matchScheduler)
        {
            List<Side> sides = _unitOfWork.GetRepository<Side>().All().ToList();

            IAuditLogger auditLogger = new AuditLogger(_unitOfWork);

            CompetitionType competitionType = _unitOfWork.GetRepository<CompetitionType>().Find( ct => ct.Name == "Points").SingleOrDefault();
            SportType sportType = _unitOfWork.GetRepository<SportType>().Find( st => st.Name == "Football").SingleOrDefault();

            IList<SportColumn> sportColumns = _unitOfWork.GetRepository<CompetitionTypeSportColumn>().All().Where(ctpc => ctpc.CompetitionType == competitionType && ctpc.SportType == sportType).Select( x => x.SportColumn).ToList();

            LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>(leagueName, DateTime.Now, DateTime.Now.AddDays(durationInDays), 5, 4, sides, auditLogger, sportColumns);

            PointsLeague newPointsLeague = new PointsLeague();
            LeagueBuilder<PointsLeague> builder = new LeagueBuilder<PointsLeague>(newPointsLeague, matchScheduler);

            director.Construct(builder);

            _unitOfWork.GetRepository<League>().Add(newPointsLeague);

            _unitOfWork.Save();
        }

        public void AwardPointsLeagueWin(int matchId, int winnerId, int loserId)
        {
            LeagueMatch leagueMatch = _unitOfWork.GetRepository<LeagueMatch>().GetById(matchId);
            PointsLeague pointsLeague = _unitOfWork.GetRepository<PointsLeague>().GetById(leagueMatch.League.Id);

            LeagueCompetitor winner = _unitOfWork.GetRepository<LeagueCompetitor>().GetById(winnerId);
            LeagueCompetitor loser = _unitOfWork.GetRepository<LeagueCompetitor>().GetById(loserId);

            ISportManager footballManager = new FootballManager(pointsLeague.CompetitionType, new PointsDto() { Win = pointsLeague.PointsForWin, Draw = pointsLeague.PointsForDraw, Loss = pointsLeague.PointsForDraw });

            PointsLeagueManager manager = new PointsLeagueManager(pointsLeague, footballManager);
            manager.AwardWin(leagueMatch, winner, loser);

            _unitOfWork.Save();
        }

        public void AwardPointsLeagueDraw(int matchId)
        {
            LeagueMatch leagueMatch = _unitOfWork.GetRepository<LeagueMatch>().GetById(matchId);
            PointsLeague pointsLeague = _unitOfWork.GetRepository<PointsLeague>().GetById(leagueMatch.League.Id);

            LeagueCompetitor competitorA = _unitOfWork.GetRepository<LeagueCompetitor>().GetById(leagueMatch.CompetitorA.Id);
            LeagueCompetitor competitorB = _unitOfWork.GetRepository<LeagueCompetitor>().GetById(leagueMatch.CompetitorB.Id);

            ISportManager footballManager = new FootballManager(pointsLeague.CompetitionType, new PointsDto() { Win = pointsLeague.PointsForWin, Draw = pointsLeague.PointsForDraw, Loss = pointsLeague.PointsForDraw });

            PointsLeagueManager updater = new PointsLeagueManager(pointsLeague, footballManager);
            updater.AwardDraw(leagueMatch, competitorA, competitorB);

            _unitOfWork.Save();
        }

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

            LeagueManager manager = new LeagueManager(new PointsLeagueManager(pointsLeague, footballManager));
            manager.AddMatch(competitorA, competitorB);

        }


        public void CreateChallengeLeague(string leagueName, int durationInDays, IMatchScheduler matchScheduler)
        {
            List<Side> sides = _unitOfWork.GetRepository<Side>().All().ToList();
            IAuditLogger auditLogger = new AuditLogger(_unitOfWork);

            CompetitionType competitionType = _unitOfWork.GetRepository<CompetitionType>().Find(ct => ct.Name == "Challenge").SingleOrDefault();
            SportType sportType = _unitOfWork.GetRepository<SportType>().Find(st => st.Name == "Football").SingleOrDefault();

            IList<SportColumn> sportColumns = _unitOfWork.GetRepository<CompetitionTypeSportColumn>().All().Where(ctpc => ctpc.CompetitionType == competitionType && ctpc.SportType == sportType).Select(x => x.SportColumn).ToList();

            LeagueBuilderDirector<ChallengeLeague> director = new LeagueBuilderDirector<ChallengeLeague>(leagueName, DateTime.Now, DateTime.Now.AddDays(durationInDays), 5, 4, sides, auditLogger, sportColumns);

            ChallengeLeague newChallengeLeague = new ChallengeLeague();

            LeagueBuilder<ChallengeLeague> builder = new LeagueBuilder<ChallengeLeague>(newChallengeLeague, matchScheduler);

            director.Construct(builder);

            _unitOfWork.GetRepository<League>().Add(newChallengeLeague);

            _unitOfWork.Save();
        }

        public void AwardChallengeLeagueWin(int matchId, int winnerId, int loserId)
        {
            LeagueMatch leagueMatch = _unitOfWork.GetRepository<LeagueMatch>().GetById(matchId);
            ChallengeLeague challengeLeague = _unitOfWork.GetRepository<ChallengeLeague>().GetById(leagueMatch.League.Id);

            LeagueCompetitor winner = _unitOfWork.GetRepository<LeagueCompetitor>().GetById(winnerId);
            LeagueCompetitor loser = _unitOfWork.GetRepository<LeagueCompetitor>().GetById(loserId);

            leagueMatch.Winner = winner;
            leagueMatch.Loser = loser;

            ISportManager footballManager = new FootballManager(challengeLeague.CompetitionType);

            ChallengeLeagueManager manager = new ChallengeLeagueManager(challengeLeague, footballManager);

            manager.AwardWin(leagueMatch, winner, loser);

            _unitOfWork.Save();
        }

        public void RenewLeague(int leagueId)
        {
            throw new NotImplementedException("Renew League not implemented");
        }
    }
}
