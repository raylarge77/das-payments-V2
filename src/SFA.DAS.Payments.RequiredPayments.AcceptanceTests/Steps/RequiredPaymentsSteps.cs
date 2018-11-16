﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.Payments.AcceptanceTests.Core.Data;
using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.Model.Core.OnProgramme;
using SFA.DAS.Payments.PaymentsDue.Messages.Events;
using SFA.DAS.Payments.RequiredPayments.AcceptanceTests.Data;
using SFA.DAS.Payments.RequiredPayments.AcceptanceTests.Handlers;
using SFA.DAS.Payments.RequiredPayments.Messages.Events;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SFA.DAS.Payments.RequiredPayments.AcceptanceTests.Steps
{
    [Binding]
    public class RequiredPaymentsSteps : RequiredPaymentsStepsBase
    {
        private const bool trace = true;

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

        private ApprenticeshipContractTypePaymentDueEvent CreatePaymentDueEvent(OnProgrammePaymentDue paymentDue)
        {
            var payment = ContractType == 1
                ? (ApprenticeshipContractTypePaymentDueEvent)new ApprenticeshipContractType1PaymentDueEvent()
                : new ApprenticeshipContractType2PaymentDueEvent();
            var testSessionLearner =
                TestSession.Learners.FirstOrDefault(l => l.LearnerIdentifier == paymentDue.LearnerId) ??
                TestSession.Learner;
            payment.Learner = testSessionLearner.ToLearner();
            payment.Learner.ReferenceNumber = testSessionLearner.LearnRefNumber;
            payment.Learner.ReferenceNumber = TestSession.GenerateLearnerReference(paymentDue.LearnerId);
            payment.Ukprn = TestSession.Ukprn;
            payment.SfaContributionPercentage = SfaContributionPercentage;
            payment.Type = paymentDue.Type;
            payment.AmountDue = paymentDue.Amount;
            payment.CollectionPeriod = new CalendarPeriod(CollectionYear, CollectionPeriod);
            payment.DeliveryPeriod = new CalendarPeriod(CollectionYear, paymentDue.DeliveryPeriod);
            payment.JobId = TestSession.JobId;
            payment.LearningAim = testSessionLearner.Course.ToLearningAim();
            payment.PriceEpisodeIdentifier = paymentDue.PriceEpisodeIdentifier;
            return payment;
        }

        [Then(@"the required payments component will only generate contract type (.*) required payments")]
        public async Task ThenTheRequiredPaymentsComponentWillOnlyGenerateContractTypeRequiredPayments(int p0, Table table)
        {
            var expectedPaymentsEvents = table
                .CreateSet<OnProgrammePaymentDue>().ToArray();//TODO: fix to use a required payments model
            await WaitForIt(() => MatchRequiredPayment(expectedPaymentsEvents), "Failed to find all the required payment events or unexpected events found");
        }

        [Then(@"the required payments component will not generate any contract type (.*) payable earnings")]
        public async Task ThenTheRequiredPaymentsComponentWillNotGenerateAnyContractTypePayableEarnings(int p0)
        {
            await WaitForIt(() => MatchUnexpectedRequiredPayment(null), "Found some unexpected required payment events");
        }

        [Then(@"the required payments component will not generate any contract type (.*) Learning \(TT(.*)\) payable earnings")]
        public async Task ThenTheRequiredPaymentsComponentWillNotGenerateAnyContractTypeLearningTTPayableEarnings(int p0, int p1)
        {
            await WaitForIt(() => MatchUnexpectedRequiredPayment(OnProgrammeEarningType.Learning), "Found some unexpected required payment events");
        }

        [Then(@"the required payments component will not generate any contract type (.*) Completion \(TT(.*)\) payable earnings")]
        public async Task ThenTheRequiredPaymentsComponentWillNotGenerateAnyContractTypeCompletionTTPayableEarnings(int p0, int p1)
        {
            await WaitForIt(() => MatchUnexpectedRequiredPayment(OnProgrammeEarningType.Completion), "Found some unexpected required payment events");
        }

        [Then(@"the required payments component will not generate any contract type (.*) Balancing \(TT(.*)\) payable earnings")]
        public async Task ThenTheRequiredPaymentsComponentWillNotGenerateAnyContractTypeBalancingTTPayableEarnings(int p0, int p1)
        {
            await WaitForIt(() => MatchUnexpectedRequiredPayment(OnProgrammeEarningType.Balancing), "Found some unexpected required payment events");
        }

        private bool MatchUnexpectedRequiredPayment(OnProgrammeEarningType? type)
        {
            var result = PaymentsDue.Where(x => !type.HasValue || x.Type == type).ToList().All(paymentDue =>
                !ApprenticeshipContractType2Handler.ReceivedEvents.Any(receivedEvent =>
                    TestSession.JobId == receivedEvent.JobId
                    && paymentDue.Amount == receivedEvent.AmountDue
                    && TestSession.Learners.Any(l => l.LearnRefNumber == receivedEvent.Learner?.ReferenceNumber)
                    && paymentDue.Type == receivedEvent.OnProgrammeEarningType
                    && TestSession.Ukprn == receivedEvent.Ukprn
                    && paymentDue.DeliveryPeriod == receivedEvent.DeliveryPeriod?.Period
                    && receivedEvent.CollectionPeriod.Name.Contains(CollectionYear)
                ));
#if DEBUG
            if (!result)
            {
                Debug.WriteLine("Found unexpected events. Trace:");
                TraceMismatch(PaymentsDue.ToArray(), ApprenticeshipContractType2Handler.ReceivedEvents.ToArray());
            }
#endif
            return result;
        }

        private Tuple<bool, string> MatchRequiredPayment(OnProgrammePaymentDue[] expectedPaymentsEvents, OnProgrammeEarningType? type = null)
        {
            OnProgrammePaymentDue[] events;

            if (type.HasValue)
            {
                events = expectedPaymentsEvents.Where(x => x.Type == type.Value).ToArray();

                if (events.Length == 0)
                    throw new ArgumentException("expectedPayments have no items of requested type");
            }
            else
            {
                events = expectedPaymentsEvents;
            }

            var sessionEvents = ApprenticeshipContractType2Handler.ReceivedEvents.Where(e => e.Ukprn == TestSession.Ukprn).ToArray();

            var matchedExpectations = sessionEvents
                .Where(receivedEvent => events.Any(expectedEvent =>
                    expectedEvent.Amount == receivedEvent.AmountDue 
                    && TestSession.JobId == receivedEvent.JobId
                    && TestSession.Learners.FirstOrDefault(l => l.LearnerIdentifier == expectedEvent.LearnerId)?.LearnRefNumber == receivedEvent.Learner?.ReferenceNumber  //TestSession.Learners.Any(l => l.LearnRefNumber == receivedEvent.Learner?.ReferenceNumber)  
                    && expectedEvent.Type == receivedEvent.OnProgrammeEarningType
                    && TestSession.Ukprn == receivedEvent.Ukprn
                    && expectedEvent.DeliveryPeriod == receivedEvent.DeliveryPeriod?.Period
                    && receivedEvent.CollectionPeriod.Name.Contains(CollectionYear)
                )).ToList();

            var allFound = matchedExpectations.Count == expectedPaymentsEvents.Length;
            var nothingExtra = sessionEvents.Length == matchedExpectations.Count;

            var reason = new List<string>();
            if (!allFound)
                reason.Add($"{expectedPaymentsEvents.Length - matchedExpectations.Count} out of {expectedPaymentsEvents.Length} event(s) did not arrive");

            if (!nothingExtra)
                reason.Add($"{sessionEvents.Length - matchedExpectations.Count} unexpected event(s) arrived");

#if DEBUG
            if ((!allFound || !nothingExtra))
            {
                if (!allFound)
                {
                    Debug.WriteLine("Did not find all expected events. Trace:");
                    TraceMismatch(expectedPaymentsEvents, sessionEvents);
                }

                if (!nothingExtra)
                {
                    Debug.WriteLine("Found some unexpected events. Trace:");
                    TraceUnexpected(sessionEvents, matchedExpectations);
                }
            }
#endif
            return Tuple.Create(allFound && nothingExtra, string.Join(" and ", reason));
        }

        private static void TraceUnexpected(ApprenticeshipContractType2RequiredPaymentEvent[] sessionEvents, List<ApprenticeshipContractType2RequiredPaymentEvent> matchedExpectations)
        {
            var unexpected = sessionEvents.Where(e => !matchedExpectations.Contains(e)).ToList();
            for (var i = 0; i < unexpected.Count; i++)
            {
                var e = unexpected[i];
                Debug.WriteLine($"{i + 1}: PE:{e.PriceEpisodeIdentifier}, AmountDue:{e.AmountDue}, LearnRefNumber:{e.Learner.ReferenceNumber}, Type:{e.OnProgrammeEarningType}, DeliveryPeriod:{e.DeliveryPeriod.Name}, CollectionPeriod:{e.CollectionPeriod.Name}");
            }
        }

        private void TraceMismatch(OnProgrammePaymentDue[] expectedPaymentsEvents, ApprenticeshipContractType2RequiredPaymentEvent[] receivedEvents)
        {
            for (var i = 0; i < expectedPaymentsEvents.Length; i++)
            {
                var expectedEvent = expectedPaymentsEvents[i];
                for (var k = 0; k < receivedEvents.Length; k++)
                {
                    var receivedEvent = receivedEvents[k];
                    var mismatchedFields = new List<string>();
                    var learnerReference = TestSession.GenerateLearnerReference(expectedEvent.LearnerId);

                    if (expectedEvent.Amount != receivedEvent.AmountDue) mismatchedFields.Add($" Amount({expectedEvent.Amount}!={receivedEvent.AmountDue})");
                    if (learnerReference != receivedEvent.Learner?.ReferenceNumber) mismatchedFields.Add($"LearnRefNumber({learnerReference}!={receivedEvent.Learner?.ReferenceNumber})");
                    if (expectedEvent.Type != receivedEvent.OnProgrammeEarningType) mismatchedFields.Add($"Type({expectedEvent.Type}!={receivedEvent.OnProgrammeEarningType})");
                    if (expectedEvent.DeliveryPeriod != receivedEvent.DeliveryPeriod?.Period) mismatchedFields.Add($"Period({expectedEvent.DeliveryPeriod}!={receivedEvent.DeliveryPeriod?.Period})");
                    if (!receivedEvent.CollectionPeriod.Name.Contains(CollectionYear)) mismatchedFields.Add($"CollectionPeriod({receivedEvent.CollectionPeriod} does not contain {CollectionYear})");

                    if (mismatchedFields.Count == 0)
                        Debug.WriteLine($"Event {i + 1} of {expectedPaymentsEvents.Length}: match {k + 1}");
                    else
                        Debug.WriteLine($"Event {i + 1} of {expectedPaymentsEvents.Length}: mismatch {k + 1} on {string.Join(",", mismatchedFields)}");
                }
            }
        }
    }
}