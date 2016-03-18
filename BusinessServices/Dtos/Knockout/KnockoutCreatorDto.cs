using Model.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Dtos.Knockout
{
    public class KnockoutCreatorDto
    {
        public int NumberOfRounds { get; set; }
        public bool IsSeeded { get; set; }
        public bool IncludeThirdPlacePlayoff { get; set; }
        public List<Side> Sides { get; set; }
    }
}
