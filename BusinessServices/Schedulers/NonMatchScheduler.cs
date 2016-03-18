using BusinessServices.Interfaces;
using Model.Leagues;
using Model.Schedule;
using Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Schedulers
{
    public class NonMatchScheduler : IMatchScheduler
    {
        public void Schedule()
        {

        }

        public void ReSchedule()
        {

        }

        public int TotalNumberOfMatches { get { return 0; } }
    }
}
