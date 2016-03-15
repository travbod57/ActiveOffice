using Model.Competitors;
using Model.Sports;

namespace Model.Record
{
    public class CompetitorRecord : DBEntity
    {
        public virtual Competitor Competitor { get; set; }
        public virtual SportColumn SportColumn { get; set; }
        public int Value { get; set; }
    }
}
