using DAL;
using Model.Actors;
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
    public class ProfileService
    {
        private IUnitOfWork _unitOfWork;

        public ProfileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // TODO : should these profiles be in a view
        public void GetPlayerProfile(int userId)
        {
            Player player = _unitOfWork.GetRepository<Player>().Find(p => p.User.Id == userId).SingleOrDefault();
        }

        // TODO : should these profiles be in a view
        public void GetTeamProfile(int teamId)
        {
            Team team = _unitOfWork.GetRepository<Team>().GetById(teamId);
        }
    }
}
