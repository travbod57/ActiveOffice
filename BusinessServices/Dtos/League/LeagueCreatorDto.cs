using BusinessServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Dtos.League
{
    public class LeagueCreatorDto
    {
        public bool CanSidePlayMoreThanOncePerMatchDay { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public Occurrance Occurrance { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
