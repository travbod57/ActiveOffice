using BusinessServices.Builders;
using BusinessServices.Builders.KnockoutCompetition;
using BusinessServices.Interfaces;
using DAL;
using Model;
using Model.Actors;
using Model.Knockouts;
using Model.Packages;
using Model.ReferenceData;
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
    }
}
