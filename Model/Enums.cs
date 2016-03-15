using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum EnumActionType
    {
       Created,
       Renewed,
       ScheduledMatches,
       RescheduledMatches,
       Joined,
       Left
    }

    public enum EnumSubjectType
    {
        League,
        Knockout,
        Tournament,
        Team
    }

    public enum EnumCompetitionType
    {
        PointsLeague,
        Knockout,
        ChallengeLeague,
        Tournament
    }
}
