﻿using System;
using System.Linq;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using SFA.DAS.Payments.RequiredPayments.Messages.Events;
using SFA.DAS.Payments.RequiredPayments.RequiredPaymentsService.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using SFA.DAS.Payments.Application.Infrastructure.Logging;
using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.PaymentsDue.Messages.Events;
using SFA.DAS.Payments.RequiredPayments.Application;
using SFA.DAS.Payments.RequiredPayments.Application.Handlers;
using SFA.DAS.Payments.RequiredPayments.Domain;
using SFA.DAS.Payments.RequiredPayments.Model.Entities;

namespace SFA.DAS.Payments.RequiredPayments.RequiredPaymentsService
{
    [StatePersistence(StatePersistence.Volatile)]
    public class RequiredPaymentsService : Actor, IRequiredPaymentsService
    {
        private PaymentDueEventHanlder _paymentDueEventHanlder;
        private ReliableCollectionCache<PaymentEntity[]> _paymentHistoryCache;

        private readonly IPaymentLogger _paymentLogger;
        private readonly IExecutionContextFactory _executionContextFactory;
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;
        private readonly string _apprenticeshipKey;
        private readonly IApprenticeshipKeyService _apprenticeshipKeyService;
        private readonly ILifetimeScope _lifetimeScope;

        public RequiredPaymentsService(ActorService actorService,
            ActorId actorId,
            IPaymentLogger paymentLogger,
            IExecutionContextFactory executionContextFactory,
            IPaymentHistoryRepository paymentHistoryRepository, 
            IApprenticeshipKeyService apprenticeshipKeyService,
            ILifetimeScope lifetimeScope) : base(actorService, actorId)
        {
            _paymentLogger = paymentLogger;
            _executionContextFactory = executionContextFactory ?? throw new ArgumentNullException(nameof(executionContextFactory));
            _paymentHistoryRepository = paymentHistoryRepository;
            _apprenticeshipKeyService = apprenticeshipKeyService;
            _lifetimeScope = lifetimeScope;
            _apprenticeshipKey = actorId.GetStringId();
        }

        public async Task<RequiredPaymentEvent> HandlePaymentDueEvent(PaymentDueEvent paymentDueEvent, CancellationToken cancellationToken)
        {
            var executionContext = _executionContextFactory.GetExecutionContext();
            executionContext.JobId = paymentDueEvent.JobId;

            _paymentLogger.LogVerbose($"Handling PaymentDue for {_apprenticeshipKey}");

            if (!await IsInitialised().ConfigureAwait(false))
                await Initialise().ConfigureAwait(false);

            var requiredPaymentEvents = await _paymentDueEventHanlder.HandlePaymentDue(paymentDueEvent, cancellationToken).ConfigureAwait(false);

            return requiredPaymentEvents;
        }

        protected override async Task OnActivateAsync()
        {
            if (!await IsInitialised().ConfigureAwait(false))
                await Initialise().ConfigureAwait(false);

            _paymentHistoryCache = new ReliableCollectionCache<PaymentEntity[]>(StateManager);
            _paymentDueEventHanlder = new PaymentDueEventHanlder(_lifetimeScope.Resolve<IPaymentDueProcessor>(), _paymentHistoryCache, _lifetimeScope.Resolve<IMapper>(), _apprenticeshipKeyService);
            await base.OnActivateAsync().ConfigureAwait(false);
        }

        public async Task Initialise()
        {
            _paymentLogger.LogInfo($"Initialising actor for apprenticeship {_apprenticeshipKey}");

            var paymentHistory = await _paymentHistoryRepository.GetPaymentHistory(_apprenticeshipKey).ConfigureAwait(false);

            if (paymentHistory != null)
            {
                var groupedEntities = paymentHistory
                    .GroupBy(payment => _apprenticeshipKeyService.GeneratePaymentKey(payment.PriceEpisodeIdentifier, payment.LearnAimReference, payment.TransactionType, NamedCalendarPeriod.FromName(payment.DeliveryPeriod)))
                    .ToDictionary(c => c.Key, c => c.ToArray());

                foreach (var p in groupedEntities)
                {
                    await _paymentHistoryCache.Add(p.Key, p.Value, CancellationToken.None).ConfigureAwait(false);
                }
            }

            _paymentLogger.LogInfo($"Initialised actor for apprenticeship {_apprenticeshipKey} with {paymentHistory?.Count()} historical payments");

            await StateManager.AddStateAsync("initialised", true).ConfigureAwait(false);
        }

        // TODO: update payment history when new payments created

        private async Task<bool> IsInitialised()
        {
            return await StateManager.ContainsStateAsync("initialised").ConfigureAwait(false);
        }
    }
}