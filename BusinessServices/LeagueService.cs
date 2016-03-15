﻿using BusinessServices.Builders;
using BusinessServices.Dto;
using BusinessServices.Dtos;
using BusinessServices.Interfaces;
using BusinessServices.Sports;
using BusinessServices.Updaters;
using DAL;
using Model.Actors;
using Model.Competitors;
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


        public void CreatePointsLeague(string leagueName, int durationInDays, IMatchScheduler matchScheduler)
        {
            List<Side> sides = _unitOfWork.GetRepository<Side>().All().ToList();

            IAuditLogger auditLogger = new AuditLogger(_unitOfWork);

            CompetitionType competitionType = _unitOfWork.GetRepository<CompetitionType>().Find( ct => ct.Name == "Points").SingleOrDefault();
            SportType sportType = _unitOfWork.GetRepository<SportType>().Find( st => st.Name == "Football").SingleOrDefault();

            IList<SportColumn> sportColumns = _unitOfWork.GetRepository<CompetitionTypeSportColumn>().All().Where(ctpc => ctpc.CompetitionType == competitionType && ctpc.SportType == sportType).Select( x => x.SportColumn).ToList();

            LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>(leagueName, DateTime.Now, DateTime.Now.AddDays(durationInDays), 5, 4, sides, auditLogger, sportColumns);

            PointsLeague newPointsLeague = new PointsLeague();
            PointsLeagueSorter sorter = new PointsLeagueSorter(newPointsLeague);

            LeagueBuilder<PointsLeague> builder = new LeagueBuilder<PointsLeague>(newPointsLeague, sorter, matchScheduler);

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
        public void AddPointsMatch(int leagueId, int competitorAId, int competitorBId)
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
            ChallengeLeagueSorter sorter = new ChallengeLeagueSorter(newChallengeLeague);

            LeagueBuilder<ChallengeLeague> builder = new LeagueBuilder<ChallengeLeague>(newChallengeLeague, sorter, matchScheduler);

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
