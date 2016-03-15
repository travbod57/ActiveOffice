using Model.ReferenceData;
using Model.UserManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Audit : DBEntity
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; } // who did it

        public EnumSubjectType SubjectType { get; set; } // what is it about, league, team etc.
        public int SubjectId { get; set; } // Id of subject that did it
        public string SubjectName { get; set; } // Name of subject, can hopefully avoid a join on retrieving logs

        public EnumActionType ActionType { get; set; } // what was the action

        public DateTime DateTimeStamp { get; set; }
    }
}
