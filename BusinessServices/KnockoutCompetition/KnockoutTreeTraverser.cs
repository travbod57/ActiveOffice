using BusinessServices.Dtos.Knockout;
using Model;
using Model.Competitors;
using Model.Knockouts;
using Model.Schedule;
using System.Collections.Generic;

namespace BusinessServices.KnockoutCompetition
{
    public class KnockoutTreeTraverser
    {
        private List<KnockoutCompetitor> _shuffledKnockoutCompetitors;
        private List<RoundInformationDto> _roundInformation;
        private Knockout _knockout;
        private int _competitorIndex;
        private bool _addCompetitorsToKnockout;

        public KnockoutTreeTraverser(List<KnockoutCompetitor> shuffledKnockoutCompetitors, List<RoundInformationDto> roundInformation, Knockout knockout, bool addCompetitorsToKnockout)
        {
            _shuffledKnockoutCompetitors = shuffledKnockoutCompetitors;
            _roundInformation = roundInformation;
            _knockout = knockout;
            _competitorIndex = 0;
            _addCompetitorsToKnockout = addCompetitorsToKnockout;
        }

        /// <summary>
        /// Each match recursively calls method again to add children for as many rounds as needed.
        /// </summary>
        /// <param name="parentMatch"></param>
        /// <param name="round"></param>
        /// <param name="knockoutSide"></param>
        public void CreateMatches(KnockoutMatch parentMatch, int round, EnumKnockoutSide knockoutSide)
        {
            RoundInformationDto roundInformation = _roundInformation[round];

            for (int i = 0; i < (roundInformation.Round == EnumRound.SemiFinal ? 1 : 2); i++)
            {
                KnockoutMatch knockoutMatch = new KnockoutMatch()
                {
                    CompetitorA = round == 0 && _addCompetitorsToKnockout ? _shuffledKnockoutCompetitors[_competitorIndex++] : null,
                    CompetitorB = round == 0 && _addCompetitorsToKnockout ? _shuffledKnockoutCompetitors[_competitorIndex++] : null,
                    Round = roundInformation.Round,
                    KnockoutSide = knockoutSide,
                    NextRoundMatch = parentMatch,
                    MatchState = EnumMatchState.Unscheduled,
                    MatchNumberForRound = ++roundInformation.MatchesForRoundCount
                };

                _knockout.KnockoutMatches.Add(knockoutMatch);

                if (round > 0)
                    CreateMatches(knockoutMatch, round - 1, knockoutSide);
            }
        }
    }
}
