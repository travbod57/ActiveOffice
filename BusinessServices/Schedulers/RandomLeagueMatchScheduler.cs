using BusinessServices.Dtos.League;
using BusinessServices.Enums;
using BusinessServices.Interfaces;
using Model.Competitors;
using Model.Leagues;
using Model.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;

namespace BusinessServices.Schedulers
{
    public class RandomLeagueMatchScheduler : IMatchScheduler
    {
        private List<LeagueMatch> _validLeagueMatchCombinations;
        private List<LeagueMatch> _tmpLeagueMatchUps;
        private League _league;
        private LeagueCreatorDto _leagueCreatorConfig;

        public RandomLeagueMatchScheduler(League league, LeagueCreatorDto leagueCreatorConfig)
        {
            _validLeagueMatchCombinations = new List<LeagueMatch>();
            _tmpLeagueMatchUps = new List<LeagueMatch>();
            _league = league;
            _leagueCreatorConfig = leagueCreatorConfig;
        }

        #region IMatchScheduler
        public void Schedule()
        {
            Validate();
            CreateLeagueMatchUpCombinations();
            CreateMatchups();
        }

        public void ReSchedule()
        {
            // delete all matches and replace. Only when league is not underway
        }

        // http://www.neelsagar.co.uk/how-many-games-in-a-tournament/
        public int TotalNumberOfMatches
        {
            get { return (int)(((0.5 * _leagueCreatorConfig.NumberOfCompetitors) * (_leagueCreatorConfig.NumberOfCompetitors - 1)) * _league.NumberOfMatchUps); }
        }
        #endregion

        public int NumberOfMatchesPerMatchDay { get; set; }

        #region Private
        private void Validate()
        {
            //if (_leagueCreatorConfig.CanSidePlayMoreThanOncePerMatchDay && true)
            //    throw new Exception("Matches will schedule with a team playing more than once a week");

            if (_leagueCreatorConfig.NumberOfCompetitors != _league.LeagueCompetitors.Count)
                throw new Exception("This league does not have enough competitors to fill all the positions");

            if (_leagueCreatorConfig.ScheduleType == ScheduleType.Adhoc)
                throw new Exception("Matches are attempting to be scheduled but the league schedule type is Adhoc");
        }

        private void CreateMatchups()
        {
            for (int i = 1; i <= _league.NumberOfMatchUps; i++)
            {
                if (i % 2 == 0)
                {
                    // every even just replicate list previous N match ups, shuffle and switch home and away competitor
                    List<LeagueMatch> previousMatchups = _tmpLeagueMatchUps.Skip(_validLeagueMatchCombinations.Count * (i - 2))
                                                                           .Take(_validLeagueMatchCombinations.Count)
                                                                           .Select(lm => new LeagueMatch() { CompetitorA = lm.CompetitorA, CompetitorB = lm.CompetitorB }).ToList();

                    _tmpLeagueMatchUps.AddRange(previousMatchups.Shuffle<LeagueMatch>().ReverseFixtures());
                }
                else
                {
                    // every odd shuffle the match up list and set random home team
                    _tmpLeagueMatchUps.AddRange(_validLeagueMatchCombinations.Shuffle<LeagueMatch>().SetRandomHomeTeam());
                }
            }

            _league.LeagueMatches.AddRange(_tmpLeagueMatchUps);
        }

        private void CreateLeagueMatchUpCombinations()
        {
            // create list of all matchups for given competitor
            foreach (var competitorA in _league.LeagueCompetitors)
            {
                foreach (var competitorB in _league.LeagueCompetitors.Where(lc => !lc.Equals(competitorA)))
                {
                    // if the fixture exists in any form already don't re-add it
                    if (_validLeagueMatchCombinations.Count(x => (x.CompetitorA == competitorA && x.CompetitorB == competitorB) || (x.CompetitorA == competitorB && x.CompetitorB == competitorA)) == 0)
                    {
                        _validLeagueMatchCombinations.Add(new LeagueMatch() { CompetitorA = competitorA, CompetitorB = competitorB });
                    }
                }
            }
        } 
        #endregion
    }

    public static class IListExtensions
    {
        public static List<LeagueMatch> SetRandomHomeTeam(this List<LeagueMatch> leagueMatches)
        {
            Random random = new Random();

            foreach (LeagueMatch leagueMatch in leagueMatches)
            {
                Competitor[] competitors = new Competitor[2] { leagueMatch.CompetitorA, leagueMatch.CompetitorB };

                int competitorPosition = random.Next(0, 2);

                leagueMatch.CompetitorA = competitors[competitorPosition];
                leagueMatch.CompetitorB = competitors[competitorPosition == 1 ? 0 : 1];
            }

            return leagueMatches;
        }

        public static List<LeagueMatch> ReverseFixtures(this List<LeagueMatch> leagueMatches)
        {
            foreach (LeagueMatch leagueMatch in leagueMatches)
            {
                Competitor tmpCompetitor = leagueMatch.CompetitorA;
                leagueMatch.CompetitorA = leagueMatch.CompetitorB;
                leagueMatch.CompetitorB = tmpCompetitor;
            }

            return leagueMatches;
        }
    }
}
