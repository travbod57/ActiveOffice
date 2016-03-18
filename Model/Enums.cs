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

    public enum EnumRound
    {
        FirstRound,
        SecondRound,
        ThirdRound,
        FourthRound,
        FifthRound,
        QuarterFinal,
        SemiFinal,
        ThirdPlacePlayOff,
        Final,
        RunnerUp,
        Winner
    }

    public enum EnumKnockoutSide
    {
        Center,
        Left,
        Right
    }
}
