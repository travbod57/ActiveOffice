using BusinessServices;
using DAL;
using Model;
using Model.Actors;
using Model.Leagues;
using Ninject;
using Ninject.Modules;
using Ninject.Extensions.Conventions;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ActiveOffice
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Bind(scanner => scanner.From("BusinessServices").SelectAllClasses().BindAllInterfaces());
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InSingletonScope();

            //kernel.Load(Assembly.GetExecutingAssembly());
            string n = Assembly.GetExecutingAssembly().FullName;
            //IUnitOfWork uow = new UnitOfWork();

            SomeClass s = new SomeClass(kernel.Get<IActorsService>());

            var u = kernel.Get<IUnitOfWork>();

            IActorsService actorsService = new ActorsService(u);
            ILeagueService leagueService = new LeagueService(u);

            actorsService.AddTeam("TEAM EXTREME");

            //var players = actorsService.GetPlayers().ToList();
            //var team = actorsService.GetPlayer(1);
            //team.Age = 50;
            //var league = leagueService.GetLeague(1);
            //league.Name = "NOw Son";

            //leagueService.CreatePointsLeague();


            //u.Save();

            //var leagueSides = leagueService.GetLeagueSides(1);
            //var leagueStandings = leagueService.GetLeagueStandings(1);

            //foreach (var item in leagueStandings)
            //{
            //    Console.WriteLine(string.Format("{0}: Pld{1} W{2} D{3} L{4} Pts{5}", item.SideName, item.Played, item.Wins, item.Draws, item.Loss, item.Points));
            //}

            Console.ReadLine();
        }

    }

    public class SomeClass
    {
        private readonly IActorsService _actorsService;
        public SomeClass()
        {

        }

        public SomeClass(IActorsService actorsService)
         {
             _actorsService = actorsService;
         }

    }
}
