﻿using Autofac;
using SFA.DAS.Payments.DataLocks.Application.Repositories;

namespace SFA.DAS.Payments.DataLocks.Application.Infrastructure.ioc
{
    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApprenticeshipRepository>().AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterType<CurrentPriceEpisodeForJobStore>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ReceivedDataLockEventStore>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}