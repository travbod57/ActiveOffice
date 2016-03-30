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
using Model.Knockouts;
using NLog;

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

        public void InitialSetup(KnockoutConfig knockoutConfig)
        {
            _knockOut.Name = knockoutConfig.Name;
            _knockOut.StartDate = knockoutConfig.StartDate;
            _knockOut.EndDate = knockoutConfig.EndDate;
            _knockOut.NumberOfRounds = knockoutConfig.NumberOfRounds;
            _knockOut.IsSeeded = knockoutConfig.IsSeeded;
            _knockOut.IncludeThirdPlacePlayoff = knockoutConfig.IncludeThirdPlacePlayoff;
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
            competitor.CompetitorRecord = new CompetitorRecord();

            _knockOut.KnockoutCompetitors.Add(competitor);
        }

        public void ScheduleMatches()
        {
            _matchScheduler.Schedule();
        }

        public void ResetKnockout()
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
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private KnockoutConfig _knockoutConfig { get; set; }

        public KnockoutBuilderDirector(KnockoutConfig knockoutConfig)
        {
            _knockoutConfig = knockoutConfig;
        }

        public Knockout Construct(KnockoutBuilder knockoutBuilder)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            knockoutBuilder.InitialSetup(_knockoutConfig);
            knockoutBuilder.AddCompetitors(_knockoutConfig.Sides);
            knockoutBuilder.ScheduleMatches();

            Knockout knockout = knockoutBuilder.GetKnockout();

            _knockoutConfig.AuditLogger.Log(knockout, 1, EnumActionType.Created);

            stopwatch.Stop();

            logger.Log(LogLevel.Info, string.Format("It took {0} to create knockout {1} with {2} rounds", stopwatch.Elapsed, _knockoutConfig.Name, _knockoutConfig.NumberOfRounds));

            return knockout;
        }
    }
}
