﻿using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.Core.Configuration;
using SFA.DAS.Payments.Core.Validation;
using SFA.DAS.Payments.EarningEvents.Domain.Mapping;
using SFA.DAS.Payments.EarningEvents.Domain.Validation.Learner;
using SFA.DAS.Payments.EarningEvents.Messages.Events;
using SFA.DAS.Payments.EarningEvents.Messages.Internal.Commands;
using SFA.DAS.Payments.Model.Core.Incentives;

namespace SFA.DAS.Payments.EarningEvents.Domain
{
    public interface ILearnerSubmissionProcessor
    {
        (ValidationResult Validation, List<EarningEvent> EarningEvents) GenerateEarnings(ProcessLearnerCommand learnerSubmission);
    }

    public class LearnerSubmissionProcessor : ILearnerSubmissionProcessor
    {
        private readonly ILearnerValidator learnerValidator;
        private readonly IApprenticeshipContractTypeEarningsEventBuilder apprenticeshipContractTypeEarningsEventBuilder;
        private readonly IFunctionalSkillEarningsEventBuilder functionalSkillEarningsEventBuilder;
        private readonly bool doNotGenerateAct1TransactionType4To16Payments;
        private readonly bool doNotGenerateAct2Payments;

        public LearnerSubmissionProcessor(ILearnerValidator learnerValidator,
            IApprenticeshipContractTypeEarningsEventBuilder apprenticeshipContractTypeEarningsEventBuilder,
            IFunctionalSkillEarningsEventBuilder functionalSkillEarningsEventBuilder,
            IConfigurationHelper configurationHelper)
        {
            this.learnerValidator = learnerValidator ?? throw new ArgumentNullException(nameof(learnerValidator));
            this.apprenticeshipContractTypeEarningsEventBuilder = apprenticeshipContractTypeEarningsEventBuilder ?? throw new ArgumentNullException(nameof(apprenticeshipContractTypeEarningsEventBuilder));
            this.functionalSkillEarningsEventBuilder = functionalSkillEarningsEventBuilder ?? throw new ArgumentNullException(nameof(functionalSkillEarningsEventBuilder));
            bool.TryParse(configurationHelper.GetSetting("DoNotGenerateACT1TransactionType4To16Payments"), out doNotGenerateAct1TransactionType4To16Payments);
            bool.TryParse(configurationHelper.GetSetting("DoNotGenerateACT2Payments"), out doNotGenerateAct2Payments);
        }

        public (ValidationResult Validation, List<EarningEvent> EarningEvents) GenerateEarnings(ProcessLearnerCommand learnerSubmission)
        {
            var validationResult = learnerValidator.Validate(learnerSubmission.Learner);

            if (validationResult.Failed)
                return (Validation: validationResult, EarningEvents: null);

            var earningsEvent = new List<EarningEvent>();
            earningsEvent.AddRange(apprenticeshipContractTypeEarningsEventBuilder.Build(learnerSubmission));
            earningsEvent.AddRange(functionalSkillEarningsEventBuilder.Build(learnerSubmission));

            var validEarningsEvent = FilterEarningsEventBasedOnConfig(earningsEvent);
            RemoveFuturePeriods(earningsEvent);

            return (Validation: validationResult, EarningEvents: validEarningsEvent);
        }

        private List<EarningEvent> FilterEarningsEventBasedOnConfig(List<EarningEvent> earningsEvent)
        {
            var earningsToDelete = new List<EarningEvent>();
            foreach (var earningEvent in earningsEvent)
            {
                if (doNotGenerateAct1TransactionType4To16Payments)
                {
                    switch (earningEvent)
                    {
                        case ApprenticeshipContractType1EarningEvent apprenticeshipContractType1EarningEvent:
                            apprenticeshipContractType1EarningEvent.IncentiveEarnings = new List<IncentiveEarning>();
                            break;
                        case Act1FunctionalSkillEarningsEvent _:
                            earningsToDelete.Add(earningEvent);
                            break;
                    }
                }

                if (doNotGenerateAct2Payments &&
                    (earningEvent is ApprenticeshipContractType2EarningEvent ||
                     earningEvent is Act2FunctionalSkillEarningsEvent))
                {
                    earningsToDelete.Add(earningEvent);
                }
            }

            var validEarningsEvent = earningsEvent.Except(earningsToDelete).ToList();
            return validEarningsEvent;
        }

        private void RemoveFuturePeriods(List<EarningEvent> earningsEvents)
        {
            foreach (var earningEvent in earningsEvents)
            {
                if (earningEvent is ApprenticeshipContractType2EarningEvent act2OnProgEarning)
                {
                    foreach (var onProgrammeEarning in act2OnProgEarning.OnProgrammeEarnings)
                    {
                        onProgrammeEarning.Periods = onProgrammeEarning.Periods.Where(p => p.Period <= earningEvent.CollectionPeriod.Period).ToList().AsReadOnly();
                    }

                    act2OnProgEarning.OnProgrammeEarnings = act2OnProgEarning.OnProgrammeEarnings.Where(e => e.Periods.Count > 0).ToList();

                    foreach (var incentiveEarning in act2OnProgEarning.IncentiveEarnings)
                    {
                        incentiveEarning.Periods = incentiveEarning.Periods.Where(p => p.Period <= earningEvent.CollectionPeriod.Period).ToList().AsReadOnly();
                    }

                    act2OnProgEarning.IncentiveEarnings = act2OnProgEarning.IncentiveEarnings.Where(e => e.Periods.Count > 0).ToList();

                    continue;
                }

                if (earningEvent is Act2FunctionalSkillEarningsEvent act2FunctionalSkillsEarning)
                {
                    foreach (var earning in act2FunctionalSkillsEarning.Earnings)
                    {
                        earning.Periods = earning.Periods.Where(p => p.Period <= earningEvent.CollectionPeriod.Period).ToList().AsReadOnly();
                    }

                    act2FunctionalSkillsEarning.Earnings = act2FunctionalSkillsEarning.Earnings.Where(e => e.Periods.Count > 0).ToList().AsReadOnly();
                }
            }
        }
    }
}