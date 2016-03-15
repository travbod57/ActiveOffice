using DAL;
using Model.Actors;
using Model.Competitors;
using Model.Leagues;
using System.Collections.Generic;
using System.Linq;

namespace BusinessServices
{
    public interface IActorsService
    {
        void AddTeam(string teamName);
        Player GetPlayer(int playerId);
        IEnumerable<Player> GetPlayersInLeague(int leagueId);
        IEnumerable<Player> GetPlayersInTeam(int teamId);
        Team GetTeam(int teamId);
        IEnumerable<Team> GetTeamsInLeague(int leagueId);
    }

    public class ActorsService : IActorsService
    {
        private IUnitOfWork _unitOfWork;

        public ActorsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Player GetPlayer(int playerId)
        {
            var player = _unitOfWork.GetRepository<Player>().GetById(playerId);
            return player;
        }

        public Team GetTeam(int teamId)
        {
            var team = _unitOfWork.GetRepository<Team>().GetById(teamId);
            return team;
        }

        public IEnumerable<Player> GetPlayersInTeam(int teamId)
        {
            Team team = _unitOfWork.GetRepository<Team>().GetById(teamId);
            return team.Players;
        }

        public IEnumerable<Team> GetTeamsInLeague(int leagueId)
        {
            var teams = from l in _unitOfWork.GetRepository<League>().All()
                        join lc in _unitOfWork.GetRepository<LeagueCompetitor>().All() on l.Id equals lc.League.Id
                        join s in _unitOfWork.GetRepository<Side>().All() on lc.Side.Id equals s.Id
                        join t in _unitOfWork.GetRepository<Team>().All() on s.Id equals t.Id
                        where l.Id == leagueId
                        select t;

            return teams;
        }

        public IEnumerable<Player> GetPlayersInLeague(int leagueId)
        {
            var players = from l in _unitOfWork.GetRepository<League>().All()
                          join lc in _unitOfWork.GetRepository<LeagueCompetitor>().All() on l.Id equals lc.League.Id
                          join s in _unitOfWork.GetRepository<Side>().All() on lc.Side.Id equals s.Id
                          join p in _unitOfWork.GetRepository<Player>().All() on s.Id equals p.Id
                          where l.Id == leagueId
                          select p;

            return players;
        }

        public void AddTeam(string teamName)
         {
            Team team = new Team() { Name = teamName };
            _unitOfWork.GetRepository<Team>().Add(team);

            _unitOfWork.Save();
        }
    }
}
