using BusinessServices.Interfaces;
using Model;
using Model.Actors;
using Model.Competitors;
using Model.Interfaces;
using Model.Leagues;
using Model.Sports;
using System;
using System.Collections.Generic;
using Core.Extensions;
using Model.Record;
using System.Diagnostics;

namespace BusinessServices.Builders
{

    public class LeagueBuilder<T> where T : League
    {
        private T _league;

        public ILeagueSorter _leagueSorter { get; set; }
        public IMatchScheduler _matchScheduler { get; set; }

        public LeagueBuilder(T league, ILeagueSorter leagueSorter, IMatchScheduler matchScheduler)
        {
            _league = league;
            _leagueSorter = leagueSorter;
            _matchScheduler = matchScheduler;
        }

        public void InitialSetup(string name, DateTime startDate, DateTime endDate, int numberOfPositions, int numberOfMatchUps, IList<SportColumn> sportColumns)
        {
            _league.Name = name;
            _league.StartDate = startDate;
            _league.EndDate = endDate;
            _league.NumberOfPositions = numberOfPositions;
            _league.NumberOfMatchUps = numberOfMatchUps;

            _league.SportColumns.AddRange(sportColumns);
        }

        public void AddCompetitors(IList<Side> sides)
        {
            if (_league.LeagueCompetitors.Count + sides.Count > _league.NumberOfPositions)
                throw new Exception("The number of competitors you are adding to this league will take it above the capacity");

            int initialPosition = _league.LeagueCompetitors.Count + 1;

            foreach (var side in sides)
            {
                AddCompetitor(side, initialPosition);
                initialPosition++;
            }
        }

        public void AddCompetitor(Side side, int initialPositionNumber)
        {
            if (_league.LeagueCompetitors.Count + 1 > _league.NumberOfPositions)
                throw new Exception("The number of competitors you are adding to this league will take it above the capacity");

            LeagueCompetitor competitor = new LeagueCompetitor() { Side = side, InitialPositionNumber = initialPositionNumber, CurrentPositionNumber = initialPositionNumber };

            foreach (var column in _league.SportColumns)
            {
                competitor.CompetitorRecords.Add(new CompetitorRecord() { SportColumn = column, Value = 0 });
            }            

            _league.LeagueCompetitors.Add(competitor);
        }

        public void ScheduleMatches()
        {
            _matchScheduler.Schedule();
        }

        public void ResetLeague()
        {
            _leagueSorter.Reset(_league);
        }

        public T GetLeague()
        {
            return _league;
        }
    }

    public class LeagueBuilderDirector<T> where T : League, IAudit
    {
        private string _name { get; set; }
        private DateTime _startDate { get; set; }
        private DateTime _endDate { get; set; }
        private IList<Side> _sides { get; set; }
        private int _numberOfPositions { get; set; }
        private int _numberOfMatchUps { get; set; }
        private IAuditLogger _auditLogger { get; set; }
        private IList<SportColumn> _sportColumns { get; set; }

        public LeagueBuilderDirector(string name, DateTime startDate, DateTime endDate, int numberOfPositions, int numberOfMatchUps, IList<Side> sides, IAuditLogger auditLogger, IList<SportColumn> sportColumns)
        {
            _name = name;
            _startDate = startDate;
            _endDate = endDate;
            _sides = sides;
            _numberOfPositions = numberOfPositions;
            _numberOfMatchUps = numberOfMatchUps;
            _auditLogger = auditLogger;
            _sportColumns = sportColumns;
        }

        public T Construct(LeagueBuilder<T> leagueBuilder)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            leagueBuilder.InitialSetup(_name, _startDate, _endDate, _numberOfPositions, _numberOfMatchUps, _sportColumns);
            leagueBuilder.AddCompetitors(_sides);
            leagueBuilder.ScheduleMatches();

            T league = leagueBuilder.GetLeague();

            _auditLogger.Log(league, 1, EnumActionType.Created);

            stopwatch.Stop();

            // TODO: NLog Info
            // string.Format("It took {0} to create League {1} of type {2} with {3} positions and {4} match ups", stopwatch.Elapsed, _name, typeof(T).Name, _numberOfPositions, _numberOfMatchUps);

            return league;
        }
    }
}
