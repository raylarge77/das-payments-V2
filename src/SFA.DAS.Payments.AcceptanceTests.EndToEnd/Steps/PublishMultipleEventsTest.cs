using Autofac;
using AutoFixture;
using NServiceBus;
using NServiceBus.Features;
using NUnit.Framework;
using SFA.DAS.Payments.AcceptanceTests.Core.Infrastructure;
using SFA.DAS.Payments.AcceptanceTests.EndToEnd.Infrastructure;
using SFA.DAS.Payments.Messages.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.AcceptanceTests.EndToEnd.Steps
{
    public class PublishMultipleEventsTest
    {
        private const string DatabaseConnectionString = "";
        private const string AzureStorageConnectionString = "";
        private const string DasServiceBusConnectionString = "";

        private Fixture Fixture { get; set; }
        private Random Random { get; set; }
        private static EndpointConfiguration DasEndpointConfiguration { get; set; }
        private static IMessageSession DasMessageSession { get; set; }
        private static ContainerBuilder Builder { get; set; }

        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
            Random = new Random();
        }

        private async Task SetUpServiceBus()
        {
            var config = new TestsConfiguration();
            var endpointConfig = new EndpointConfiguration("SFA.DAS.CommitmentsV2");
            DasEndpointConfiguration = endpointConfig;

            Builder.RegisterInstance(endpointConfig)
                .Named<EndpointConfiguration>("DasEndpointConfiguration")
                .SingleInstance();
            var conventions = endpointConfig.Conventions();
            conventions.DefiningMessagesAs(type => type.IsMessage());
            conventions
                .DefiningCommandsAs(t => t.IsInNamespace("SFA.DAS.CommitmentsV2.Messages.Events"));

            endpointConfig.UsePersistence<AzureStoragePersistence>()
                .ConnectionString(AzureStorageConnectionString);
            endpointConfig.DisableFeature<TimeoutManager>();

            var transportConfig = endpointConfig.UseTransport<AzureServiceBusTransport>();
            Builder.RegisterInstance(transportConfig)
                .Named<TransportExtensions<AzureServiceBusTransport>>("DasTransportConfig")
                .SingleInstance();

            transportConfig
                .UseForwardingTopology()
                .ConnectionString(DasServiceBusConnectionString)
                .Transactions(TransportTransactionMode.ReceiveOnly)
                .Queues()
                .DefaultMessageTimeToLive(config.DefaultMessageTimeToLive);
            var routing = transportConfig.Routing();

            //Add types of the events you will publish here
            routing.RouteToEndpoint(typeof(CommitmentsV2.Messages.Events.ApprenticeshipCreatedEvent).Assembly, EndpointNames.DataLocksApprovals);
            routing.RouteToEndpoint(typeof(CommitmentsV2.Messages.Events.ApprenticeshipUpdatedApprovedEvent).Assembly, EndpointNames.DataLocksApprovals);
            routing.RouteToEndpoint(typeof(CommitmentsV2.Messages.Events.DataLockTriageApprovedEvent).Assembly, EndpointNames.DataLocksApprovals);
            routing.RouteToEndpoint(typeof(CommitmentsV2.Messages.Events.ApprenticeshipStoppedEvent).Assembly, EndpointNames.DataLocksApprovals);
            routing.RouteToEndpoint(typeof(CommitmentsV2.Messages.Events.ApprenticeshipStopDateChangedEvent).Assembly, EndpointNames.DataLocksApprovals);
            routing.RouteToEndpoint(typeof(CommitmentsV2.Messages.Events.ApprenticeshipPausedEvent).Assembly, EndpointNames.DataLocksApprovals);
            routing.RouteToEndpoint(typeof(CommitmentsV2.Messages.Events.ApprenticeshipResumedEvent).Assembly, EndpointNames.DataLocksApprovals);
            routing.RouteToEndpoint(typeof(CommitmentsV2.Messages.Events.PaymentOrderChangedEvent).Assembly, EndpointNames.DataLocksApprovals);

            var sanitization = transportConfig.Sanitization();
            var strategy = sanitization.UseStrategy<ValidateAndHashIfNeeded>();
            strategy.RuleNameSanitization(
                ruleNameSanitizer: ruleName => ruleName.Split('.').LastOrDefault() ?? ruleName);
            endpointConfig.UseSerialization<NewtonsoftSerializer>();
            endpointConfig.EnableInstallers();

            DasMessageSession = await Endpoint.Start(DasEndpointConfiguration);
        }
    }
}