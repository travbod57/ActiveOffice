using Model.Competitors;
using System.Collections.Generic;

namespace BusinessServices.KnockoutCompetition
{
    public class KnockoutSection
    {
        public KnockoutSection()
        {
            competitors = new List<KnockoutCompetitor>();
        }

        public List<KnockoutCompetitor> competitors { get; set; }
    }
}
