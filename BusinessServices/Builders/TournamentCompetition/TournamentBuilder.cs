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
using Model.Tournaments;
using BusinessServices.Builders.LeagueCompetition;
using BusinessServices.Schedulers;
using BusinessServices.Dtos.League;
using BusinessServices.Enums;
using BusinessServices.Builders.KnockoutCompetition;
using BusinessServices.Dtos.Knockout;
using Model.ReferenceData;

namespace BusinessServices.Builders.TournamentCompetition
{

    public class TournamentBuilder
    {
        private Tournament _tournament;
        private ITournamentSorter _tournamentSorter { get; set; }

        public TournamentBuilder(Tournament tournament, ITournamentSorter tournamentSorter)
        {
            _tournament = tournament;
            _tournamentSorter = tournamentSorter;
        }

        public void InitialSetup(TournamentConfig config)
        {
            _tournament.Name = config.Name;
            _tournament.StartDate = config.StartDate;
            _tournament.EndDate = config.EndDate;
            _tournament.NumberOfRounds = config.NumberOfRounds;
        }

        public void AddCompetitors(IList<Side> sides)
        {
            //if (_knockOut.KnockoutCompetitors.Count + sides.Count > _knockOut.NumberOfCompetitors)
            //    throw new Exception("The number of competitors you are adding to this knockout will take it above the capacity");

            int initialPosition = _tournament.TournamentCompetitors.Count + 1;

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

            TournamentCompetitor competitor = new TournamentCompetitor() { Side = side };

            _tournament.TournamentCompetitors.Add(competitor);
        }

        public void CreatePools(TournamentConfig config)
        {
            LeagueConfig leagueConfig = new LeagueConfig()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                NumberOfPositions = 5,
                NumberOfMatchUps = 2,
                Sides = config.Sides,
                AuditLogger = config.AuditLogger,
                IsPartOfTournament = true
            };

            for (int i = 0; i < config.NumberOfPools; i++)
            {
                leagueConfig.Name = string.Format("Pool {0}", i);

                LeagueBuilderDirector<PointsLeague> director = new LeagueBuilderDirector<PointsLeague>(leagueConfig);

                PointsLeague pointsLeague = new PointsLeague();
                LeagueCreatorDto leagueCreatorDto = new LeagueCreatorDto() {
                    CanSidePlayMoreThanOncePerMatchDay = false,
                    DayOfWeek = DayOfWeek.Friday,
                    NumberOfCompetitors = config.NumberOfPositionsPerPool,
                    Occurrance = Occurrance.Daily,
                    ScheduleType = ScheduleType.Scheduled
                };

                IMatchScheduler matchScheduler = new RandomLeagueMatchScheduler(pointsLeague, leagueCreatorDto);
                LeagueBuilder<PointsLeague> builder = new LeagueBuilder<PointsLeague>(pointsLeague, matchScheduler);

                director.Construct(builder);

                _tournament.Leagues.Add(pointsLeague);
            }
        }

        public void CreateKnockout(TournamentConfig config)
        {
            KnockoutConfig knockoutConfig = new KnockoutConfig()
            {
                Name = string.Format("{0} knockout", config.Name),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                IncludeThirdPlacePlayoff = false,
                IsSeeded = false,
                Sides = new List<Side>(),
                AuditLogger = config.AuditLogger,
                NumberOfRounds = config.NumberOfRounds,
                IsPartOfTournament = true
            };

            KnockoutBuilderDirector director = new KnockoutBuilderDirector(knockoutConfig);

            Knockout knockout = new Knockout() { CompetitionType = new CompetitionType() { Id = 1, Name = "Knockout" } };
            KnockoutSorter sorter = new KnockoutSorter(knockout);

            KnockoutMatchScheduler matchScheduler = new KnockoutMatchScheduler(knockout, knockoutConfig);

            KnockoutBuilder builder = new KnockoutBuilder(knockout, sorter, matchScheduler);
            director.Construct(builder);

            _tournament.Knockout = knockout;
        }

        public void Validate(TournamentConfig config)
        {

        }

        public void ScheduleMatches()
        {

        }

        public void ResetTournament()
        {
            _tournamentSorter.Reset(_tournament);
        }

        public Tournament GetTournament()
        {
            return _tournament;
        }
    }

    public class TournamentBuilderDirector
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private TournamentConfig _config { get; set; }

        public TournamentBuilderDirector(TournamentConfig config)
        {
            config.IsSeeded = false;
            _config = config;
        }

        public Tournament Construct(TournamentBuilder tournamentBuilder)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            tournamentBuilder.InitialSetup(_config);
            tournamentBuilder.Validate(_config);
            tournamentBuilder.AddCompetitors(_config.Sides);
            tournamentBuilder.CreatePools(_config);
            tournamentBuilder.CreateKnockout(_config);

            Tournament tournament = tournamentBuilder.GetTournament();

            _config.AuditLogger.Log(tournament, 1, EnumActionType.Created);

            stopwatch.Stop();

            logger.Log(LogLevel.Info, string.Format("It took {0} to create tournament {1} with {2} rounds and {3} pools each with {4} competitors", stopwatch.Elapsed, _config.Name, _config.NumberOfRounds, _config.NumberOfPools, _config.NumberOfPositionsPerPool));

            return tournament;
        }
    }
}
