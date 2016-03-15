using BusinessServices.Interfaces;
using Model;
using Model.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Builders
{
    public class PointsLeagueSorter : LeagueSorterBase, ILeagueSorter
    {
        private PointsLeague _pointsLeague { get; set; }

        public PointsLeagueSorter(PointsLeague pointsLeague)
        {
            _pointsLeague = pointsLeague;
        }

        public void UpdatePosition()
        {

        }
    }
}
