using Model.Interfaces;
using Model.ReferenceData;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Leagues
{
    [Table("PointsLeague")]
    public class PointsLeague : League, IAudit, ICompetition
    {
        public int PointsForWin { get; set; }
        public int PointsForDraw { get; set; }
        public int PointsForLoss { get; set; }

        #region IAudit
        [NotMapped]
        public EnumSubjectType SubjectType { get { return EnumSubjectType.League;  }}
        #endregion
    }
}
