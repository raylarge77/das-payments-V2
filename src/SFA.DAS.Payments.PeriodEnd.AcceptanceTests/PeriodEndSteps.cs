﻿using System.Linq;
using System.Threading.Tasks;
using Autofac;
using SFA.DAS.Payments.AcceptanceTests.Core;
using SFA.DAS.Payments.AcceptanceTests.Core.Automation;
using SFA.DAS.Payments.AcceptanceTests.Core.Data;
using SFA.DAS.Payments.PeriodEnd.AcceptanceTests.Handlers;
using SFA.DAS.Payments.PeriodEnd.Model;
using TechTalk.SpecFlow;

namespace SFA.DAS.Payments.PeriodEnd.AcceptanceTests
{
    [Binding]
    public class PeriodEndSteps : StepsBase
    {
        public PeriodEndSteps(ScenarioContext context) : base(context)
        {

        }

        [Given(@"the payments are for the current collection year")]
        public void GivenThePaymentsAreForTheCurrentCollectionYear()
        {
            SetCurrentCollectionYear();
        }

        [Given(@"the current collection period is R(.*)")]
        [Given(@"the current processing period is (.*)")]
        public void GivenTheCurrentProcessingPeriodIs(byte period)
        {
            CollectionPeriod = period;
        }

        [When(@"the period end service is notified the the period end has started")]
        public async Task WhenThePeriodEndServiceIsNotifiedTheThePeriodEndHasStarted()
        {
            var dcHelper = Scope.Resolve<IDcHelper>();
            await dcHelper.SendPeriodEndTask(AcademicYear, CollectionPeriod, TestSession.JobId, PeriodEndTaskType.PeriodEndStart.ToString()).ConfigureAwait(false);
        }

        [When(@"the period end service is notified the the period end has stopped")]
        public async Task WhenThePeriodEndServiceIsNotifiedTheThePeriodEndHasStopped()
        {
            var dcHelper = Scope.Resolve<IDcHelper>();
            await dcHelper.SendPeriodEndTask(AcademicYear, CollectionPeriod, TestSession.JobId, PeriodEndTaskType.PeriodEndStop.ToString()).ConfigureAwait(false);
        }

        [When(@"the period end service is notified the the period end is running")]
        public async Task WhenThePeriodEndServiceIsNotifiedTheThePeriodEndIsRunning()
        {
            var dcHelper = Scope.Resolve<IDcHelper>();
            await dcHelper.SendPeriodEndTask(AcademicYear, CollectionPeriod, TestSession.JobId, PeriodEndTaskType.PeriodEndRun.ToString()).ConfigureAwait(false);
        }

        [When(@"the period end service is notified that period end reports have been requested")]
        public async Task WhenThePeriodEndServiceIsNotifiedThatPeriodEndReportsHaveBeenRequested()
        {
            var dcHelper = Scope.Resolve<IDcHelper>();
            await dcHelper.SendPeriodEndTask(AcademicYear, CollectionPeriod, TestSession.JobId, PeriodEndTaskType.PeriodEndReports.ToString()).ConfigureAwait(false);
        }



        [When(@"the period end service is notified that a period end request validate submission window job has been requested")]
        public async Task WhenThePeriodEndServiceIsNotifiedThatAPeriodEndRequestValidateSubmissionWindowJobHasBeenRequested()
        {
            var dcHelper = Scope.Resolve<IDcHelper>();
            await dcHelper.SendPeriodEndTask(AcademicYear, CollectionPeriod, TestSession.JobId, PeriodEndTaskType.PeriodEndSubmissionWindowValidation.ToString()).ConfigureAwait(false);
        }

        [When(@"the period end service is notified the the period end is running twice")]
        public async Task WhenThePeriodEndServiceIsNotifiedTheThePeriodEndIsRunningTwice()
        {
            var dcHelper = Scope.Resolve<IDcHelper>();
            await dcHelper.SendPeriodEndTask(AcademicYear, CollectionPeriod, TestSession.JobId, PeriodEndTaskType.PeriodEndRun.ToString()).ConfigureAwait(false);
            await dcHelper.SendPeriodEndTask(AcademicYear, CollectionPeriod, TestSession.DuplicateJobId, PeriodEndTaskType.PeriodEndRun.ToString()).ConfigureAwait(false);
        }

        [Then(@"the period end service should publish a period end request validate submission window event")]
        public async Task ThenThePeriodEndServiceShouldPublishAPeriodEndRequestValidateSubmissionWindowEvent()
        {
            await WaitForIt(() =>
            {
                return PeriodEndRequestValidateSubmissionWindow.ReceivedEvents.Any(ev => ev.JobId == TestSession.JobId);
            }, $"Failed to find the period end request validate submission window event for job : { TestSession.JobId}");
        }


        [Then(@"the period end service should publish a period end started event")]
        public async Task ThenThePeriodEndServiceShouldPublishAPeriodEndStartedEvent()
        {
            await WaitForIt(() =>
                {
                    return PeriodEndStartedEventHandler.ReceivedEvents.Any(ev => ev.JobId == TestSession.JobId);
                }, $"Failed to find the period end started event for job : { TestSession.JobId}");
        }

        [Then(@"the period end service should publish a period end running event")]
        public async Task ThenThePeriodEndServiceShouldPublishAPeriodEndRunningEvent()
        {
            await WaitForIt(() =>
            {
                return PeriodEndRunningEventHandler.ReceivedEvents.Any(ev => ev.JobId == TestSession.JobId);
            }, $"Failed to find the period end running event for job : { TestSession.JobId}");
        }


        [Then(@"the period end service should publish a period end stopped event")]
        public async Task ThenThePeriodEndServiceShouldPublishAPeriodEndStoppedEvent()
        {
            await WaitForIt(() =>
            {
                return PeriodEndStoppedEventHandler.ReceivedEvents.Any(ev => ev.JobId == TestSession.JobId);
            }, $"Failed to find the period end stopped event for job : { TestSession.JobId}");
        }


     
        
        [Then(@"the period end service should publish a period end request reports event")]
        public async Task ThenThePeriodEndServiceShouldPublishAPeriodEndRequestReportsEvent()
        {
            await WaitForIt(() =>
            {
                return PeriodEndRequestReportsEventHandlers.ReceivedEvents.Any(ev => ev.JobId == TestSession.JobId);
            }, $"Failed to find the period end request reports event for job : { TestSession.JobId}");
        }

        [Then(@"the period end (.*) job is persisted to the database")]
        public async Task ThenThePeriodEndJobIsPersistedToTheDatabase(string periodEndJobType)
        {
            short jobType;
            switch (periodEndJobType.ToLower())
            {
                case "stopped":
                    jobType = 6;
                    break;
                case "started":
                    jobType = 2;
                    break;
                case "running":
                    jobType = 5;
                    break;
                default:
                    jobType = 5;
                    break;
            }

            await WaitForIt(() => Container.Resolve<TestPaymentsDataContext>().JobExists(TestSession.JobId, jobType),
                $"Failed to find the period end {periodEndJobType} job for dc job id : { TestSession.JobId}");
        }

        [Then("not publish one for the duplicate notification")]
        public async Task ThenNotPublishOneForTheDuplicateNotification()
        {
            var failText = "Found Unexpected Period End Running Event Published";
            await WaitForUnexpected(() =>
            {
                return !PeriodEndRunningEventHandler.ReceivedEvents.Any(ev => ev.JobId == TestSession.DuplicateJobId)
                    ? (true, string.Empty)
                    : (false, failText);
            }, failText);
        }
    }
}