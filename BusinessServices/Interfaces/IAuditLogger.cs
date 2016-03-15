using Model;
using Model.Interfaces;

namespace BusinessServices.Interfaces
{
    public interface IAuditLogger
    {
        void Log(IAudit entity, int userId, EnumActionType actionType);
    }
}
