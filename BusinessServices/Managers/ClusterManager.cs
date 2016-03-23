using BusinessServices.Interfaces;
using BusinessServices.Managers.LeagueCompetition;
using Model.Actors;
using Model.LeagueArrangements;
using Model.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;

namespace BusinessServices.Managers
{
    public class ClusterManager
    {
        private Cluster _cluster;
        private ILeagueManager _leagueManager;

        public ClusterManager(Cluster cluster)
        {
            _cluster = cluster;
        }

        public ClusterManager(Cluster cluster, ILeagueManager leagueManager)
        {
            _cluster = cluster;
            _leagueManager = leagueManager;
        }

        public void ChangeName(string name)
        {
            _cluster.Name = name;
        }

        public void AddLeagues(IEnumerable<League> leagues)
        {
            if (leagues.Intersect(_cluster.Leagues).Count() > 0)
                throw new Exception("You are attempting to add leagues to a cluster that are already in the cluster");

            IEnumerable<League> allLeagues = leagues.Union(_cluster.Leagues);
            bool sameCompetitionType = allLeagues.Select(x => x.CompetitionType).Distinct().Count() == 1;

            if (!sameCompetitionType)
                throw new Exception("Clusters are not allowed to have leagues of different types");

            // add the new leagues to the cluster
            _cluster.Leagues.AddRange<League>(leagues);
        }

        public void RemoveLeagues(IEnumerable<League> leagues)
        {
            if (leagues.Count(l => l.IsActive) > 0)
                throw new Exception("At least one of the leagues you are trying to remove is active");

            _cluster.Leagues.RemoveRange(leagues);
        }

        public void Activate()
        {
            _cluster.IsActive = true;

            foreach (var league in _cluster.Leagues)
            {
                league.IsActive = true;
            }
        }

        public void RenewDivisions()
        {
            if (_cluster.Leagues.Any(l => !l.IsFinalised))
                throw new Exception("You cannot renew a cluster until all divisions have been finalised");

            foreach (var league in _cluster.Leagues)
	        {
                _leagueManager.RenewLeague();
	        }     
        }
    }
}
