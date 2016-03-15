using Model.Leagues;

namespace BusinessServices.Interfaces
{
    public interface IMatchScheduler
    {
        void Schedule();
        void ReSchedule();
        int TotalNumberOfMatches { get; }
    }
}
