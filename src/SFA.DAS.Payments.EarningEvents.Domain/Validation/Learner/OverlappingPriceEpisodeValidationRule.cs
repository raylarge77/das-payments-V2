﻿using System.Linq;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;

namespace SFA.DAS.Payments.EarningEvents.Domain.Validation.Learner
{
    public class OverlappingPriceEpisodeValidationRule: ILearnerValidationRule
    {
        public ValidationRuleResult IsValid(FM36Learner learner)
        {
            foreach (var priceEpisode in learner.PriceEpisodes)
            {
                var overlappingPriceEpisode = learner.PriceEpisodes
                    .Where(pe => pe != priceEpisode)
                    .FirstOrDefault(pe =>
                        priceEpisode.PriceEpisodeValues.PriceEpisodePlannedEndDate > pe.PriceEpisodeValues?.EpisodeStartDate &&
                        priceEpisode.PriceEpisodeValues.EpisodeStartDate < pe.PriceEpisodeValues?.PriceEpisodePlannedEndDate);
                if (overlappingPriceEpisode != null)
                    return ValidationRuleResult.Failed($"Found overlapping price episodes.  Price Episode {priceEpisode.PriceEpisodeIdentifier} overlapped with price episode {overlappingPriceEpisode.PriceEpisodeIdentifier}.");
            }
            return  ValidationRuleResult.Ok();
        }
    }
}