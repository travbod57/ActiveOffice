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
using Model.Knockouts;

namespace BusinessServices.Builders.KnockoutCompetition
{

    public class KnockoutBuilder
    {
        private Knockout _knockOut;

        private IMatchScheduler _matchScheduler { get; set; }
        private IKnockoutSorter _knockoutSorter { get; set; }

        public KnockoutBuilder(Knockout knockout, IKnockoutSorter knockoutSorter, IMatchScheduler matchScheduler)
        {
            _knockOut = knockout;
            _matchScheduler = matchScheduler;
            _knockoutSorter = knockoutSorter;
        }

        public void InitialSetup(string knockoutName, DateTime startDate, DateTime endDate, int numberOfRounds, bool isSeeded, bool includeThirdPlacePlayoff, IList<SportColumn> sportColumns)
        {
            _knockOut.Name = knockoutName;
            _knockOut.StartDate = startDate;
            _knockOut.EndDate = endDate;
            _knockOut.NumberOfRounds = numberOfRounds;
            _knockOut.IsSeeded = isSeeded;
            _knockOut.IncludeThirdPlacePlayoff = includeThirdPlacePlayoff;

            _knockOut.SportColumns.AddRange(sportColumns);
        }

        public void AddCompetitors(IList<Side> sides)
        {
            //if (_knockOut.KnockoutCompetitors.Count + sides.Count > _knockOut.NumberOfCompetitors)
            //    throw new Exception("The number of competitors you are adding to this knockout will take it above the capacity");

            int initialPosition = _knockOut.KnockoutCompetitors.Count + 1;

            foreach (var side in sides)
            {
                AddCompetitor(side, initialPosition);
                initialPosition++;
            }
        }

        public void AddCompetitor(Side side, int initialSeeding)
        {
            //if (_knockOut.KnockoutCompetitors.Count + 1 > _knockOut.NumberOfCompetitors)
            //    throw new Exception("The number of competitors you are adding to this knockout will take it above the capacity");

            KnockoutCompetitor competitor = new KnockoutCompetitor() { Side = side, InitialSeeding = initialSeeding };

            foreach (var column in _knockOut.SportColumns)
            {
                competitor.CompetitorRecords.Add(new CompetitorRecord() { SportColumn = column, Value = 0 });
            }

            _knockOut.KnockoutCompetitors.Add(competitor);
        }

        public void ScheduleMatches()
        {
            _matchScheduler.Schedule();
        }

        public void ResetLeague()
        {
            _knockoutSorter.Reset(_knockOut);
        }

        public Knockout GetKnockout()
        {
            return _knockOut;
        }
    }

    public class KnockoutBuilderDirector
    {
        private string _name { get; set; }
        private DateTime _startDate { get; set; }
        private DateTime _endDate { get; set; }
        private IList<Side> _sides { get; set; }
        private int _numberOfRounds { get; set; }
        private bool _isSeeded { get; set; }
        private bool _includeThirdPlacePlayoff { get; set; }
        private IAuditLogger _auditLogger { get; set; }
        private IList<SportColumn> _sportColumns { get; set; }

        public KnockoutBuilderDirector(string name, DateTime startDate, DateTime endDate, int numberOfRounds, bool isSeeded, bool includeThirdPlacePlayoff, IList<Side> sides, IAuditLogger auditLogger, IList<SportColumn> sportColumns)
        {
            _name = name;
            _startDate = startDate;
            _endDate = endDate;
            _sides = sides;
            _numberOfRounds = numberOfRounds;
            _isSeeded = isSeeded;
            _includeThirdPlacePlayoff = includeThirdPlacePlayoff;
            _auditLogger = auditLogger;
            _sportColumns = sportColumns;
        }

        public Knockout Construct(KnockoutBuilder knockoutBuilder)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            knockoutBuilder.InitialSetup(_name, _startDate, _endDate, _numberOfRounds, _isSeeded, _includeThirdPlacePlayoff, _sportColumns);
            knockoutBuilder.AddCompetitors(_sides);
            knockoutBuilder.ScheduleMatches();

            Knockout knockout = knockoutBuilder.GetKnockout();

            _auditLogger.Log(knockout, 1, EnumActionType.Created);

            stopwatch.Stop();

            // TODO: NLog Info
            // string.Format("It took {0} to create League {1} of type {2} with {3} positions and {4} match ups", stopwatch.Elapsed, _name, typeof(T).Name, _numberOfPositions, _numberOfMatchUps);

            return knockout;
        }
    }
}
