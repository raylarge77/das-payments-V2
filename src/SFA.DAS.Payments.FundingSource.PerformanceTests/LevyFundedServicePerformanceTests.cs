﻿using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using NServiceBus.Features;
using NUnit.Framework;
using SFA.DAS.Payments.AcceptanceTests.Core.Infrastructure;
using SFA.DAS.Payments.FundingSource.Application.Data;
using SFA.DAS.Payments.Messages.Core;
using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.Model.Core.Entities;
using SFA.DAS.Payments.Model.Core.OnProgramme;
using SFA.DAS.Payments.RequiredPayments.Messages.Events;

namespace SFA.DAS.Payments.FundingSource.PerformanceTests
{
    [TestFixture]
    public class LevyFundedServicePerformanceTests
    {
        private static EndpointConfiguration endpointConfiguration;
        private static IEndpointInstance endpointInstance;
        private static Config config;
        private FundingSourceDataContext dataContext;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            //var config = new TestsConfiguration();

            config = new Config();
            BuildConfiguration().Bind(config);

            endpointConfiguration = new EndpointConfiguration(config.AppSettings.TestsEndpointName);
            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningMessagesAs(type => type.IsMessage());


            endpointConfiguration.UsePersistence<AzureStoragePersistence>()
                .ConnectionString(config.ConnectionStrings.StorageConnectionString);
            endpointConfiguration.DisableFeature<TimeoutManager>();


            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            transport
                .ConnectionString(config.ConnectionStrings.ServiceBusConnectionString)
                .Transactions(TransportTransactionMode.ReceiveOnly)
                .RuleNameShortener(ruleName => ruleName.Split('.').LastOrDefault() ?? ruleName);

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(CalculatedRequiredLevyAmount), config.AppSettings.LevyEndPoint);
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendOnly();
            endpointInstance = await Endpoint.Start(endpointConfiguration);
        }

        [SetUp]
        public async Task SetUp()
        {
            var dataContext = new FundingSourceDataContext(config.ConnectionStrings.PaymentsConnectionString);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(TestContext.CurrentContext.TestDirectory);
            builder.AddJsonFile("appSettings.json", false);
            //builder.AddJsonFile("appSettings.debug.json",true);
            return builder.Build();
        }

        [TestCase(100)]
        [TestCase(1000)]
        public async Task Batch_For_Same_Employer(int batchSize)
        {
            var options = new NServiceBus.SendOptions();
            var visibleTime = DateTime.UtcNow.AddSeconds(10);
            Console.WriteLine($"Messages visible at {visibleTime:G}");
            options.DoNotDeliverBefore(visibleTime);
            var messages = Enumerable.Range(0, batchSize).Select(i =>
                new CalculatedRequiredLevyAmount
                {
                    AmountDue = 100,
                    ContractType = ContractType.Act1,
                    CollectionPeriod = new CollectionPeriod {Period = 1, AcademicYear = 1920},
                    DeliveryPeriod = 1,
                    JobId = 9990999,
                    Ukprn = 100003915,
                    AccountId = 999,
                    SfaContributionPercentage = .95M,
                    EarningEventId = Guid.NewGuid(),
                    Learner = new Learner {Uln = 99999},
                    OnProgrammeEarningType = OnProgrammeEarningType.Learning,
                    TransferSenderAccountId = 999
                }).ToList();
            foreach (var calculatedRequiredLevyAmount in messages)
            {
                await endpointInstance.Send(calculatedRequiredLevyAmount, options).ConfigureAwait(false);
            }
            Console.WriteLine($"Sent {batchSize} messages");
            
        }



        [OneTimeTearDown]
        public async Task OneTimeCleanUp()
        {
            if (endpointInstance != null)
                await endpointInstance.Stop();
        }
    }
}