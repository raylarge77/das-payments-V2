﻿using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.Payments.AcceptanceTests.Core.Data;
using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.Model.Core.OnProgramme;
using SFA.DAS.Payments.PaymentsDue.Messages.Events;
using SFA.DAS.Payments.RequiredPayments.AcceptanceTests.Data;
using SFA.DAS.Payments.RequiredPayments.AcceptanceTests.Handlers;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SFA.DAS.Payments.RequiredPayments.AcceptanceTests.Steps
{
    [Binding]
    public class RequiredPaymentsSteps : RequiredPaymentsStepsBase
    {
        public RequiredPaymentsSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [When(@"a payments due event is received")]
        public async Task WhenAPaymentsDueEventIsReceived()
        {
            var payments = PaymentsDue.Select(CreatePaymentDueEvent).ToList();
            foreach (var paymentDue in payments)
            {
                await MessageSession.Send(paymentDue).ConfigureAwait(false);
            }
        }

        private ApprenticeshipContractTypePaymentDueEvent CreatePaymentDueEvent(Data.OnProgrammePaymentDue paymentDue)
        {
            var payment = ContractType == 1
                ? (ApprenticeshipContractTypePaymentDueEvent)new ApprenticeshipContractType1PaymentDueEvent()
                : new ApprenticeshipContractType2PaymentDueEvent();

            payment.Learner = TestSession.Learner.ToLearner();
            payment.Ukprn = TestSession.Ukprn;
            payment.SfaContributionPercentage = SfaContributionPercentage;
            payment.Type = paymentDue.Type;
            payment.AmountDue = paymentDue.Amount;
            payment.CollectionPeriod = new CalendarPeriod(CollectionYear, CollectionPeriod);
            payment.DeliveryPeriod = new CalendarPeriod(CollectionYear, paymentDue.Period);
            payment.JobId = TestSession.JobId;
            payment.LearningAim = TestSession.Learner.Course.ToLearningAim();
            payment.PriceEpisodeIdentifier = "p-1"; //TODO: will need to change if scenario specifies different identifier
            return payment;
        }

        [Then(@"the required payments component will generate the following contract type (.*) Completion \(TT(.*)\) payable earnings:")]
        public void ThenTheRequiredPaymentsComponentWillGenerateTheFollowingContractTypeCompletionPayableEarnings(byte contractType, int transactionType, Table expectedEventsTable)
        {
            var expectedPaymentsEvents = expectedEventsTable
                .CreateSet<OnProgrammePaymentDue>();//TODO: fix to use a required payments model
            WaitForIt(() =>
            {
                return expectedPaymentsEvents.Where(x => x.Type == OnProgrammeEarningType.Completion).All(expectedEvent =>
                    ApprenticeshipContractType2Handler.ReceivedEvents.Any(receivedEvent =>
                        expectedEvent.Amount == receivedEvent.AmountDue
                        && TestSession.Learner.LearnRefNumber == receivedEvent?.Learner?.ReferenceNumber
                        && expectedEvent.Type == receivedEvent.OnProgrammeEarningType
                        && TestSession.Ukprn == receivedEvent.Ukprn
                        && expectedEvent.Period == receivedEvent.DeliveryPeriod?.Period
                        && receivedEvent.CollectionPeriod.Name.Contains(CollectionYear)
                    ));
            }, "Failed to find all the required payment events");
        }

        [Then(@"the required payments component will generate the following contract type (.*) Learning \(TT(.*)\) payable earnings:")]
        public void ThenTheRequiredPaymentsComponentWillGenerateTheFollowingContractTypeLearningPayableEarnings(byte contractType, int transactionType, Table expectedEventsTable)
        {
            var expectedPaymentsEvents = expectedEventsTable
                .CreateSet<OnProgrammePaymentDue>();//TODO: fix to use a required payments model
            WaitForIt(() =>
            {
                return expectedPaymentsEvents.Where(x => x.Type == OnProgrammeEarningType.Learning).All(expectedEvent =>
                    ApprenticeshipContractType2Handler.ReceivedEvents.Any(receivedEvent =>
                        expectedEvent.Amount == receivedEvent.AmountDue
                        && TestSession.Learner.LearnRefNumber == receivedEvent?.Learner?.ReferenceNumber
                        && expectedEvent.Type == receivedEvent.OnProgrammeEarningType
                        && TestSession.Ukprn == receivedEvent.Ukprn
                        && expectedEvent.Period == receivedEvent.DeliveryPeriod?.Period
                        && receivedEvent.CollectionPeriod.Name.Contains(CollectionYear)
                    ));
            }, "Failed to find all the required payment events");
        }

        [Then(@"the required payments component will generate the following contract type (.*) Balancing \(TT(.*)\) payable earnings:")]
        public void ThenTheRequiredPaymentsComponentWillGenerateTheFollowingContractTypeBalancingPayableEarnings(byte contractType, int transactionType, Table expectedEventsTable)
        {
            var expectedPaymentsEvents = expectedEventsTable
                .CreateSet<OnProgrammePaymentDue>();//TODO: fix to use a required payments model
            WaitForIt(() =>
            {
                return expectedPaymentsEvents.Where(x => x.Type == OnProgrammeEarningType.Balancing).All(expectedEvent =>
                    ApprenticeshipContractType2Handler.ReceivedEvents.Any(receivedEvent =>
                        expectedEvent.Amount == receivedEvent.AmountDue
                        && TestSession.Learner.LearnRefNumber == receivedEvent?.Learner?.ReferenceNumber
                        && expectedEvent.Type == receivedEvent.OnProgrammeEarningType
                        && TestSession.Ukprn == receivedEvent.Ukprn
                        && expectedEvent.Period == receivedEvent.DeliveryPeriod?.Period
                        && receivedEvent.CollectionPeriod.Name.Contains(CollectionYear)
                    ));
            }, "Failed to find all the required payment events");
        }

        [Then(@"the required payments component will not generate any contract type (.*) Learning \(TT(.*)\) payable earnings")]
        public void ThenTheRequiredPaymentsComponentWillNotGenerateAnyContractTypeLearningTTPayableEarnings(int p0, int p1)
        {
            WaitForIt(() =>
            {
                return PaymentsDue.Where(x => x.Type == OnProgrammeEarningType.Learning).All(paymentDue =>
                    !ApprenticeshipContractType2Handler.ReceivedEvents.Any(receivedEvent =>
                        paymentDue.Amount == receivedEvent.AmountDue
                        && TestSession.Learner.LearnRefNumber == receivedEvent?.Learner?.ReferenceNumber
                        && paymentDue.Type == receivedEvent.OnProgrammeEarningType
                        && TestSession.Ukprn == receivedEvent.Ukprn
                        && paymentDue.Period == receivedEvent.DeliveryPeriod?.Period
                        && receivedEvent.CollectionPeriod.Name.Contains(CollectionYear)
                    ));
            }, "Found some unexpected required payment events");
        }

        [Then(@"the required payments component will not generate any contract type (.*) Completion \(TT(.*)\) payable earnings")]
        public void ThenTheRequiredPaymentsComponentWillNotGenerateAnyContractTypeCompletionTTPayableEarnings(int p0, int p1)
        {
            WaitForIt(() =>
            {
                return PaymentsDue.Where(x => x.Type == OnProgrammeEarningType.Completion).All(paymentDue =>
                    !ApprenticeshipContractType2Handler.ReceivedEvents.Any(receivedEvent =>
                            paymentDue.Amount == receivedEvent.AmountDue
                            && TestSession.Learner.LearnRefNumber == receivedEvent?.Learner?.ReferenceNumber
                            && paymentDue.Type == receivedEvent.OnProgrammeEarningType
                            && TestSession.Ukprn == receivedEvent.Ukprn
                            && paymentDue.Period == receivedEvent.DeliveryPeriod?.Period
                    && receivedEvent.CollectionPeriod.Name.Contains(CollectionYear)
                    ));
            }, "Found some unexpected required payment events");
        }

        [Then(@"the required payments component will not generate any contract type (.*) Balancing \(TT(.*)\) payable earnings")]
        public void ThenTheRequiredPaymentsComponentWillNotGenerateAnyContractTypeBalancingTTPayableEarnings(int p0, int p1)
        {
            WaitForIt(() =>
            {
                return PaymentsDue.Where(x => x.Type == OnProgrammeEarningType.Balancing).All(paymentDue =>
                    !ApprenticeshipContractType2Handler.ReceivedEvents.Any(receivedEvent =>
                            paymentDue.Amount == receivedEvent.AmountDue
                            && TestSession.Learner.LearnRefNumber == receivedEvent?.Learner?.ReferenceNumber
                            && paymentDue.Type == receivedEvent.OnProgrammeEarningType
                            && TestSession.Ukprn == receivedEvent.Ukprn
                            && paymentDue.Period == receivedEvent.DeliveryPeriod?.Period
                    && receivedEvent.CollectionPeriod.Name.Contains(CollectionYear)
                    ));
            }, "Found some unexpected required payment events");
        }
    }
}