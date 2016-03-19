using BusinessServices.Builders;
using BusinessServices.Builders.KnockoutCompetition;
using BusinessServices.Dtos;
using BusinessServices.Interfaces;
using BusinessServices.Managers.KnockoutCompetition;
using BusinessServices.Sports;
using DAL;
using Model;
using Model.Actors;
using Model.Competitors;
using Model.Knockouts;
using Model.Packages;
using Model.ReferenceData;
using Model.Schedule;
using Model.Sports;
using Model.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public class KnockoutService
    {
        private IUnitOfWork _unitOfWork;

        public KnockoutService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateKnockout(string knockoutName, int numberOfRounds, bool isSeeded, bool includeThirdPlacePlayoff, IMatchScheduler matchScheduler)
        {
            List<Side> sides = _unitOfWork.GetRepository<Side>().All().ToList();
            IAuditLogger auditLogger = new AuditLogger(_unitOfWork);

            CompetitionType competitionType = _unitOfWork.GetRepository<CompetitionType>().Find(ct => ct.Name == "Knockout").SingleOrDefault();
            SportType sportType = _unitOfWork.GetRepository<SportType>().Find(st => st.Name == "Football").SingleOrDefault();

            IList<SportColumn> sportColumns = _unitOfWork.GetRepository<CompetitionTypeSportColumn>().All().Where(ctpc => ctpc.CompetitionType == competitionType && ctpc.SportType == sportType).Select(x => x.SportColumn).ToList();

            KnockoutBuilderDirector director = new KnockoutBuilderDirector(knockoutName, DateTime.Now, DateTime.Now.AddDays(10), 6, isSeeded, includeThirdPlacePlayoff, sides, auditLogger, sportColumns);

            Knockout newKnockout = new Knockout();
            KnockoutSorter sorter = new KnockoutSorter(newKnockout);

            KnockoutBuilder builder = new KnockoutBuilder(newKnockout, sorter, matchScheduler);

            director.Construct(builder);

            _unitOfWork.GetRepository<Knockout>().Add(newKnockout);

            _unitOfWork.Save();
        }

        public void ActivateKnockout(int knockoutId)
        {
            Knockout knockout = _unitOfWork.GetRepository<Knockout>().GetById(knockoutId);
            knockout.IsActive = true;

            _unitOfWork.Save();
        }

        public void AwardKnockoutWin(int knockoutMatchId, int winnerCompetitorId, int loserCompetitorId, int winnerScore, int loserScore)
        {

            KnockoutMatch knockoutMatch = _unitOfWork.GetRepository<KnockoutMatch>().GetById(knockoutMatchId);
            Knockout knockout = knockoutMatch.Knockout;

            KnockoutCompetitor winnerKnockoutCompetitor = _unitOfWork.GetRepository<KnockoutCompetitor>().GetById(winnerCompetitorId);
            KnockoutCompetitor loserKnockoutCompetitor = _unitOfWork.GetRepository<KnockoutCompetitor>().GetById(loserCompetitorId);

            ISportManager footballManager = new FootballManager(knockout.CompetitionType);

            KnockoutManager knockoutManager = new KnockoutManager(knockout, footballManager);
            knockoutManager.AwardWin(knockoutMatch, winnerKnockoutCompetitor, loserKnockoutCompetitor, winnerScore, loserScore);

            _unitOfWork.Save();
        }

        #region Season Management
        public void CreateSeason()
        {

        }

        public void ActivateSeason(int seasonId)
        {

        }
        #endregion
    }
}
