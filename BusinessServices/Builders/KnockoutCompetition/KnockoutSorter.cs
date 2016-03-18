using BusinessServices.Interfaces;
using Model.Knockouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Builders.KnockoutCompetition
{
    public class KnockoutSorter : IKnockoutSorter
    {
        private Knockout _knockout { get; set; }

        public KnockoutSorter(Knockout knockout)
        {
            _knockout = knockout;
        }

        public void Reset(Knockout knockout)
        {

        }
    }
}
