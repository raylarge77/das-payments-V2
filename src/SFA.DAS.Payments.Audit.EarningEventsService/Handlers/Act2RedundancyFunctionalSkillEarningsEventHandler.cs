﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Payments.Application.Infrastructure.Logging;
using SFA.DAS.Payments.Application.Messaging;
using SFA.DAS.Payments.Audit.Application.PaymentsEventProcessing.EarningEvent;
using SFA.DAS.Payments.EarningEvents.Messages.Events;

namespace SFA.DAS.Payments.Audit.EarningEventsService.Handlers
{
    public class Act2RedundancyFunctionalSkillEarningsEventHandler : IHandleMessageBatches<Act2RedundancyFunctionalSkillEarningsEvent>
    {
        private readonly IPaymentLogger logger;
        private readonly IEarningEventStorageService storageService;

        public Act2RedundancyFunctionalSkillEarningsEventHandler(IPaymentLogger logger, IEarningEventStorageService storageService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        public async Task Handle(IList<Act2RedundancyFunctionalSkillEarningsEvent> messages, CancellationToken cancellationToken)
        {
            logger.LogDebug($"Handling ACT2 Redundancy Functional Skill Earnings Event for Job(s): { string.Join(",", messages.Select(x => x.JobId).Distinct().ToArray()) }");
            var earningEvents = new List<EarningEvent>();
            earningEvents.AddRange(messages);
            await storageService.StoreEarnings(earningEvents, cancellationToken).ConfigureAwait(false);
            logger.LogDebug($"Finished Handling ACT2 Redundancy Functional Skill Earnings Event for Job(s): { string.Join(",", messages.Select(x => x.JobId).Distinct().ToArray()) }");
        }
    }
}