﻿using Autofac;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using SFA.DAS.Payments.Application.Infrastructure.Logging;
using SFA.DAS.Payments.ServiceFabric.Core;
using System.Collections.Generic;
using System.Fabric;

namespace SFA.DAS.Payments.ProviderPayments.ProviderPaymentsProxyService
{
    public class ProviderPaymentsProxyService : StatelessService
    {
        private readonly ILifetimeScope lifetimeScope;
        private readonly IPaymentLogger paymentLogger;

        public ProviderPaymentsProxyService(StatelessServiceContext context, ILifetimeScope lifetimeScope, IPaymentLogger paymentLogger) : base(context)
        {
            this.lifetimeScope = lifetimeScope;
            this.paymentLogger = paymentLogger;
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            paymentLogger.LogInfo("Creating Service Instance Listeners For Provider Payments Proxy Service");

            return new List<ServiceInstanceListener>
            {
                new ServiceInstanceListener(context =>lifetimeScope.Resolve<IStatelessEndpointCommunicationListener>())
            };
        }

    }
}
