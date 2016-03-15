using Model.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Leagues
{
    [Table("ChallengeLeague")]
    public class ChallengeLeague : League, IAudit, ICompetition
    {
        public bool CanDraw { get; set; }

        #region IAudit
        [NotMapped]
        public EnumSubjectType SubjectType { get { return EnumSubjectType.League;  }}
        #endregion
        
    }
}
