using Model.Leagues;

namespace BusinessServices.Builders
{
    public abstract class LeagueSorterBase
    {
        public void Reset(League league)
        {
            foreach (var competitor in league.LeagueCompetitors)
            {
                //competitor.Played = 0;
                //competitor.Wins = 0;
                //competitor.Draws = 0;
                //competitor.Losses = 0;
                //competitor.Points = 0;
                //competitor.CurrentPositionNumber = competitor.InitialPositionNumber;
            }
        }
    }
}
