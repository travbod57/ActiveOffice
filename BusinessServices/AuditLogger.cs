using BusinessServices.Interfaces;
using DAL;
using Model;
using Model.Interfaces;
using System;

namespace BusinessServices
{
    public class AuditLogger : IAuditLogger
    {
        public IUnitOfWork _unitOfWork { get; set; }

        public AuditLogger(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Log(IAudit entity, int userId, EnumActionType actionType)
        {
            Audit audit = new Audit();
            audit.DateTimeStamp = DateTime.Now;
            audit.UserId = userId;
            audit.ActionType = actionType;
            audit.SubjectType = entity.SubjectType;
            audit.SubjectId = entity.Id;

            _unitOfWork.GetRepository<Audit>().Add(audit);
        }
    }
}
