﻿using System;
using AutoMapper;
using SFA.DAS.Payments.Application.Infrastructure.Logging;
using SFA.DAS.Payments.EarningEvents.Messages.Events;
using SFA.DAS.Payments.Model.Core.OnProgramme;
using SFA.DAS.Payments.RequiredPayments.Domain;
using SFA.DAS.Payments.RequiredPayments.Domain.Entities;

namespace SFA.DAS.Payments.RequiredPayments.Application.Processors
{
    public class ApprenticeshipContractType2EarningEventProcessor 
        : ApprenticeshipContractTypeEarningEventProcessor<ApprenticeshipContractTypeEarningsEvent>, IApprenticeshipContractType2EarningsEventProcessor
    {
        public ApprenticeshipContractType2EarningEventProcessor(
            IMapper mapper,
            IRequiredPaymentProcessor requiredPaymentProcessor,
            IHoldingBackCompletionPaymentService holdingBackCompletionPaymentService,
            IPaymentHistoryRepository paymentHistoryRepository,
            IApprenticeshipKeyProvider apprenticeshipKeyProvider,
            INegativeEarningService negativeEarningService,
            IPaymentLogger paymentLogger
        ) : base(
            mapper,
            requiredPaymentProcessor,
            holdingBackCompletionPaymentService,
            paymentHistoryRepository,
            apprenticeshipKeyProvider,
            negativeEarningService,
            paymentLogger
        )
        {
        }

        protected override EarningType GetEarningType(int type)
        {
            if (Enum.IsDefined(typeof(OnProgrammeEarningType), type))
            {
                return EarningType.CoInvested;
            }

            return EarningType.Incentive;
        }
    }
}