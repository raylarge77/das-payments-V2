﻿using System;
using System.Threading.Tasks;
using ESFA.DC.Logging.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using NServiceBus;
using SFA.DAS.Payments.Application.Infrastructure.Logging;
using SFA.DAS.Payments.FundingSource.LevyFundedService.Interfaces;
using SFA.DAS.Payments.RequiredPayments.Messages.Events;

namespace SFA.DAS.Payments.FundingSource.LevyFundedProxyService.Handlers
{
    public class ApprenticeshipContractType1RequiredPaymentEventHandler : IHandleMessages<CalculatedRequiredLevyAmount>
    {
        private readonly IActorProxyFactory proxyFactory;
        private readonly IPaymentLogger paymentLogger;
        private readonly ESFA.DC.Logging.ExecutionContext executionContext;

        public ApprenticeshipContractType1RequiredPaymentEventHandler(IActorProxyFactory proxyFactory,
            IPaymentLogger paymentLogger,
            IExecutionContext executionContext)
        {
            this.proxyFactory = proxyFactory ?? new ActorProxyFactory();
            this.paymentLogger = paymentLogger;
            this.executionContext = (ESFA.DC.Logging.ExecutionContext) executionContext;
        }

        public async Task Handle(CalculatedRequiredLevyAmount message, IMessageHandlerContext context)
        {
            paymentLogger.LogInfo($"Processing ApprenticeshipContractType1RequiredPaymentEvent event. Message Id: {context.MessageId}, Job: {message.JobId}, UKPRN: {message.Ukprn}");
            executionContext.JobId = message.JobId.ToString();

            try
            {
                var actorId = new ActorId(message.EmployerAccountId);
                var actor = proxyFactory.CreateActorProxy<ILevyFundedService>(new Uri("fabric:/SFA.DAS.Payments.FundingSource.ServiceFabric/LevyFundedServiceActorService"), actorId);
                await actor.HandleRequiredPayment(message).ConfigureAwait(false);
                paymentLogger.LogInfo($"Successfully processed LevyFundedProxyService event for Actor Id {actorId}, Job: {message.JobId}, UKPRN: {message.Ukprn}");
            }
            catch (Exception ex)
            {
                paymentLogger.LogError($"Error while handling LevyFundedProxyService event, Job: {message.JobId}, UKPRN: {message.Ukprn}", ex);
                throw;
            }
        }
    }
}