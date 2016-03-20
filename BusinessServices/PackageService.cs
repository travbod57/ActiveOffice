using DAL;
using Model;
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
    public interface IPackageService
    {
        IEnumerable<Package> GetPackages();
    }

    public class PackageService : IPackageService
    {
        private IUnitOfWork _unitOfWork;

        public PackageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Package> GetPackages()
        {
            var packages = from p in _unitOfWork.GetRepository<Package>().All()
                           where p.IsActive && (DateTime.Now.Date >= p.ActiveFrom && DateTime.Now.Date <= p.ActiveTo)
                           select p;

            return packages;
        }
    }
}
