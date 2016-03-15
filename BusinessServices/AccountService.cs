using DAL;
using Model.Packages;
using Model.ReferenceData;
using Model.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public class AccountService
    {
        private IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateAccount(int packageId, IList<int> sportTypeIds, IList<int> competitionTypeIds)
        {
            Package package = _unitOfWork.GetRepository<Package>().GetById(packageId);

            if (sportTypeIds.Count > package.NumberOfSportTypes)
                throw new Exception("Your package does not allow you to add this many sports");

            if (competitionTypeIds.Count > package.NumberOfCompetitionTypes)
                throw new Exception("Your package does not allow you to add this many competition types");

            IList<SportType> sportTypes = _unitOfWork.GetRepository<SportType>().Find(st => sportTypeIds.Contains(st.Id)).ToList();
            IList<CompetitionType> competitionTypes = _unitOfWork.GetRepository<CompetitionType>().Find(ct => competitionTypeIds.Contains(ct.Id)).ToList();

            // TODO: Create and add administrator
            // TODO: Add company details

            Account account = new Account()
            {
                CompetitionTypes = competitionTypes,
                SportTypes = sportTypes,
                Package = package
            };

            _unitOfWork.Save();
        }

        public IList<User> GetAccountAdministrators(int accountId)
        {
            Account account = _unitOfWork.GetRepository<Account>().GetById(accountId);
            return account.Administrators.ToList();
        }

        public void AddAdministratorToAccount(int accountId, int userId)
        {
            Account account = _unitOfWork.GetRepository<Account>().GetById(accountId);
            User user = _unitOfWork.GetRepository<User>().GetById(userId);

            if (account.Administrators.Count + 1 > 5)
                throw new Exception(string.Format("You cannot have more than {0} Administrators on an account", 5));

            account.Administrators.Add(user);

            _unitOfWork.Save();
        }

        public void RemoveAdministratorFromAccount(int accountId, int userId)
        {
            Account account = _unitOfWork.GetRepository<Account>().GetById(accountId);
            User user = _unitOfWork.GetRepository<User>().GetById(userId);

            if (account.Administrators.Any( u => u == user && u.IsAccountAdministrator))
                throw new Exception("This user is not an Administrator on this account");

            if (account.Administrators.Count - 1 == 0)
                throw new Exception("You cannot have an account without an Administrator");

            account.Administrators.Remove(user);

            _unitOfWork.Save();
        }
    }
}
