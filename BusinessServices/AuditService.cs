using DAL;
using Model;
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
    public class AuditService
    {
        private IUnitOfWork _unitOfWork;

        public AuditService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<string> GetPlayerLog(int userId)
        {
            var playerLog = from a in _unitOfWork.GetRepository<Audit>().All()
                            where a.UserId == userId
                            orderby a.DateTimeStamp descending
                            select string.Format("{0} {1} on {2} at {3}", a.User.UserName, a.ActionType, a.DateTimeStamp.Date, a.DateTimeStamp.TimeOfDay);
            
            return playerLog;
        }

        public IEnumerable<string> GetTeamLog(int teamId)
        {
            var teamLog = from a in _unitOfWork.GetRepository<Audit>().All()
                          join t in _unitOfWork.GetRepository<Team>().All() on a.SubjectId equals t.Id
                          where a.SubjectId == teamId && a.SubjectType == EnumSubjectType.Team
                          orderby a.DateTimeStamp descending
                          select string.Format("{0} {1} {2} on {2} at {3}", a.User.UserName, a.ActionType, t.Name, a.DateTimeStamp.Date, a.DateTimeStamp.TimeOfDay);

            return teamLog;

        }

        //public IEnumerable<string> GetCompetitionLog(int competitionId)
        //{
        //    // get based on subject type and subject type id
        //}
    }
}
