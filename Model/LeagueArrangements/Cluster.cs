﻿using Model.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.LeagueArrangements
{
    public class Cluster : DBEntity
    {
        public Cluster()
        {
            Leagues = new List<League>();
        }

        public string Name { get; set; }
        public string Sponsor { get; set; }
        public int NumberOfDivisions { get; set; }
        public bool IsActive { get; set; }
        public virtual Season Season { get; set; }

        public ICollection<League> Leagues { get; set; }
    }
}
