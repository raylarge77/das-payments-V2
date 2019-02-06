﻿using Autofac;
using SFA.DAS.Payments.FundingSource.Application.Interfaces;
using SFA.DAS.Payments.FundingSource.Application.Services;
using SFA.DAS.Payments.FundingSource.Domain.Interface;
using SFA.DAS.Payments.FundingSource.Domain.Services;
using System.Collections.Generic;
using AutoMapper;
using SFA.DAS.Payments.FundingSource.Application.Repositories;
using SFA.DAS.Payments.RequiredPayments.Messages.Events;
using SFA.DAS.Payments.ServiceFabric.Core;
using SFA.DAS.Payments.ServiceFabric.Core.Infrastructure.Cache;

namespace SFA.DAS.Payments.FundingSource.Application.Infrastructure.Ioc
{
    public class FundingSourceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ValidateRequiredPaymentEvent>().AsImplementedInterfaces();
            builder.RegisterType<CoInvestedFundingSourcePaymentEventMapper>().AsImplementedInterfaces();
            builder.RegisterType<SfaFullyFundedPaymentProcessor>().AsImplementedInterfaces();
            builder.RegisterType<SfaFullyFundedFundingSourcePaymentEventMapper>().AsImplementedInterfaces();
            builder.RegisterType<IncentiveRequiredPaymentProcessor>().AsImplementedInterfaces();
            builder.RegisterType<LevyAccountRepository>().AsImplementedInterfaces();
            builder.RegisterType<PaymentProcessor>().AsImplementedInterfaces();
            builder.RegisterType<LevyPaymentProcessor>().As<ILevyPaymentProcessor>();
            builder.RegisterType<CoInvestedPaymentProcessor>().As<ICoInvestedPaymentProcessor>();
            builder.RegisterType<EmployerCoInvestedPaymentProcessor>().As<IEmployerCoInvestedPaymentProcessor>();
            builder.RegisterType<SfaCoInvestedPaymentProcessor>().As<ISfaCoInvestedPaymentProcessor>();
            builder.RegisterType<LevyBalanceService>().AsImplementedInterfaces().SingleInstance();

            builder.Register(c => new ContractType2RequiredPaymentEventFundingSourceService
            (
                new List<ICoInvestedPaymentProcessorOld>()
                {
                    new SfaCoInvestedPaymentProcessor(c.Resolve<IValidateRequiredPaymentEvent>()),
                    new EmployerCoInvestedPaymentProcessor(c.Resolve<IValidateRequiredPaymentEvent>())
                },
                c.Resolve<ICoInvestedFundingSourcePaymentEventMapper>()
            )).As<IContractType2RequiredPaymentEventFundingSourceService>();

            builder.Register(c =>
            {
                var stateManagerProvider = c.Resolve<IActorStateManagerProvider>();
                return new ContractType1RequiredPaymentEventFundingSourceService
                (
                    c.Resolve<IPaymentProcessor>(),
                    c.Resolve<IMapper>(),
                    new ReliableCollectionCache<ApprenticeshipContractType1RequiredPaymentEvent>(stateManagerProvider),
                    new ReliableCollectionCache<List<string>>(stateManagerProvider),
                    c.Resolve<ILevyAccountRepository>(),
                    c.Resolve<ILevyBalanceService>()
                );
            }).As<IContractType1RequiredPaymentEventFundingSourceService>();
        }
    }
}