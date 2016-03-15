using Model.Actors;
using Model.ReferenceData;
using Model.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Record
{
    public class SportSideRecord : DBEntity
    {
        public Side Side { get; set; }
        public SportType SportType { get; set; }
        public Season Season { get; set; }
        public SportColumn SportColumn { get; set; }
        public int Value { get; set; }
    }
}
