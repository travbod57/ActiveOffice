using Model.Record;
using Model.UserManagement;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Actors
{
    [Table("Player")]
    public class Player : Side
    {
        public Player()
        {
            Teams = new List<Team>();
        }

        public virtual PlayerRecord PlayerRecord { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
