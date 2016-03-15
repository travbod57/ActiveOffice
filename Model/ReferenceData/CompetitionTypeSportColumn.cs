using Model.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ReferenceData
{
    /// <summary>
    /// Holds information regarding which columns are applicable for any given competition given a certain sport
    /// Primarily used for displaying the columns in league tables
    /// </summary>
    public class CompetitionTypeSportColumn : DBEntity
    {
        public virtual CompetitionType CompetitionType { get; set; }
        public virtual SportType SportType { get; set; }
        public virtual SportColumn SportColumn { get; set; }
        public int ColumnOrder { get; set; }
        public bool IsOptional { get; set; }
    }
}
