using Model.Competitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Interfaces
{
    public interface IFootballRecord
    {
        Competitor Competitor { get; set; }
        int Played { get; set; }
        int Points { get; set; }
        int Wins { get; set; }
        int Draws { get; set; }
        int Losses { get; set; }
        int For { get; set; }
        int Against { get; set; }
        int Difference { get; set; }
    }
}
