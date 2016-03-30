using BusinessServices.Interfaces;
using Model;
using Model.Actors;
using Model.Competitors;
using Model.Interfaces;
using Model.Leagues;
using System;
using System.Collections.Generic;
using Core.Extensions;
using Model.Record;
using System.Diagnostics;
using NLog;

namespace BusinessServices.Builders.LeagueCompetition
{
    public class LeagueBuilder<T> where T : League
    {
        private T _league;

        private IMatchScheduler _matchScheduler { get; set; }

        public LeagueBuilder(T league, IMatchScheduler matchScheduler)
        {
            _league = league;
            _matchScheduler = matchScheduler;
        }

        public void InitialSetup(LeagueConfig leagueConfig)
        {
            _league.Name = leagueConfig.Name;
            _league.StartDate = leagueConfig.StartDate;
            _league.EndDate = leagueConfig.EndDate;
            _league.NumberOfCompetitors = leagueConfig.NumberOfMatchUps;
            _league.NumberOfMatchUps = leagueConfig.NumberOfMatchUps;
        }

        public void AddCompetitors(IList<Side> sides)
        {
            //if (_league.LeagueCompetitors.Count + sides.Count > _league.NumberOfCompetitors)
            //    throw new Exception("The number of competitors you are adding to this league will take it above the capacity");

            int initialPosition = _league.LeagueCompetitors.Count + 1;

            foreach (var side in sides)
            {
                AddCompetitor(side, initialPosition);
                initialPosition++;
            }
        }

        public void AddCompetitor(Side side, int initialPositionNumber)
        {
            //if (_league.LeagueCompetitors.Count + 1 > _league.NumberOfCompetitors)
            //    throw new Exception("The number of competitors you are adding to this league will take it above the capacity");

            LeagueCompetitor competitor = new LeagueCompetitor() { Side = side, InitialPositionNumber = initialPositionNumber, CurrentPositionNumber = initialPositionNumber };
            competitor.CompetitorRecord = new CompetitorRecord();

            _league.LeagueCompetitors.Add(competitor);
        }

        public void ScheduleMatches()
        {
            _matchScheduler.Schedule();
        }

        public void ResetLeague()
        {
            // TODO: what does this need to do if anything?
        }

        public T GetLeague()
        {
            return _league;
        }
    }

    public class LeagueBuilderDirector<T> where T : League, IAudit
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private LeagueConfig _leagueConfig;


        public LeagueBuilderDirector(LeagueConfig leagueConfig)
        {
            _leagueConfig = leagueConfig;
        }

        public T Construct(LeagueBuilder<T> leagueBuilder)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            leagueBuilder.InitialSetup(_leagueConfig);
            leagueBuilder.AddCompetitors(_leagueConfig.Sides);
            leagueBuilder.ScheduleMatches();
            
            T league = leagueBuilder.GetLeague();

            _leagueConfig.AuditLogger.Log(league, 1, EnumActionType.Created);

            stopwatch.Stop();

            logger.Log(LogLevel.Info, string.Format("It took {0} to create League {1} of type {2} with {3} positions and {4} match ups", stopwatch.Elapsed, _leagueConfig.Name, typeof(T).Name, _leagueConfig.NumberOfPositions, _leagueConfig.NumberOfMatchUps));

            return league;
        }
    }
}
