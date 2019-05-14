﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Payments.AcceptanceTests.Core.Automation;
using SFA.DAS.Payments.AcceptanceTests.Core.Data;
using SFA.DAS.Payments.AcceptanceTests.EndToEnd.Data;
using SFA.DAS.Payments.AcceptanceTests.EndToEnd.Handlers;
using SFA.DAS.Payments.DataLocks.Messages.Events;
using SFA.DAS.Payments.EarningEvents.Messages.Events;
using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.Model.Core.Incentives;
using SFA.DAS.Payments.Model.Core.OnProgramme;
using SFA.DAS.Payments.Tests.Core;
using SFA.DAS.Payments.Tests.Core.Builders;

namespace SFA.DAS.Payments.AcceptanceTests.EndToEnd.EventMatchers
{
    public class EarningFailedDataLockMatcher : BaseMatcher<EarningFailedDataLockMatching>
    {
        private readonly IList<DataLockError> expectedDataLockErrorsSpec;
        private readonly TestSession testSession;
        private readonly CollectionPeriod collectionPeriod;
        private readonly Provider provider;

        public EarningFailedDataLockMatcher(Provider provider, TestSession testSession, CollectionPeriod collectionPeriod, IList<DataLockError> expectedDataLockErrorsSpec)
        {
            this.expectedDataLockErrorsSpec = expectedDataLockErrorsSpec;
            this.provider = provider;
            this.testSession = testSession;
            this.collectionPeriod = collectionPeriod;
        }

        protected override IList<EarningFailedDataLockMatching> GetActualEvents()
        {
            return EarningFailedDataLockMatchingHandler.ReceivedEvents
                .Where(ev =>
                    ev.Ukprn == provider.Ukprn &&
                    ev.JobId == provider.JobId &&
                    ev.CollectionPeriod.Period == collectionPeriod.Period &&
                    ev.CollectionPeriod.AcademicYear == collectionPeriod.AcademicYear)
                .ToList();
        }

        protected override IList<EarningFailedDataLockMatching> GetExpectedEvents()
        {
            var earningFailedDataLockEvents = new List<EarningFailedDataLockMatching>();

            var learnerIds = expectedDataLockErrorsSpec.Select(e => e.LearnerId).Distinct().ToList();

            foreach (var learnerId in learnerIds)
            {
                var learner = testSession.GetLearner(provider.Ukprn, learnerId);
                var learnerEarnings = expectedDataLockErrorsSpec.Where(x => x.LearnerId == learnerId).ToList();
                var groupedEarningPerTransactionTypes = learnerEarnings.GroupBy(x => x.TransactionType);

                var earningFailedDataLockEvent = new EarningFailedDataLockMatching
                {
                    CollectionPeriod = collectionPeriod,
                    Ukprn = provider.Ukprn,
                    Learner = new Model.Core.Learner
                    {
                        Uln = learner.Uln
                    },
                    LearningAim = new LearningAim
                    {
                        ProgrammeType = learnerEarnings.First().ProgrammeType,
                        StandardCode = learnerEarnings.First().StandardCode
                    },
                    OnProgrammeEarnings = new List<OnProgrammeEarning>()
                };

                foreach (var earningPerTransactionTypes in groupedEarningPerTransactionTypes)
                {
                    var earningPerPeriods = earningPerTransactionTypes.GroupBy(x => x.DeliveryPeriod);

                    var earningPeriods = new List<EarningPeriod>();
                    foreach (var earningPerPeriod in earningPerPeriods)
                    {
                        earningPeriods.Add(new EarningPeriod
                        {
                            Period = new CollectionPeriodBuilder().WithDate(earningPerPeriod.Key.ToDate()).Build().Period,
                            DataLockFailures = earningPerPeriod.Select(x => new DataLockFailure
                            {
                                ApprenticeshipId = learnerEarnings.First().ApprenticeshipId,
                                DataLockError = x.ErrorCode,
                                ApprenticeshipPriceEpisodeIds = new List<long>()
                            }).ToList()
                        });
                    }

                    earningFailedDataLockEvent.OnProgrammeEarnings.Add(new OnProgrammeEarning
                    {
                        Type = (OnProgrammeEarningType)earningPerTransactionTypes.Key,
                        Periods = earningPeriods.AsReadOnly()
                    });
                }

                earningFailedDataLockEvents.Add(earningFailedDataLockEvent);
            }

            return earningFailedDataLockEvents;
        }

        protected override bool Match(EarningFailedDataLockMatching expectedEvent, EarningFailedDataLockMatching actualEvent)
        {
            if (expectedEvent.CollectionPeriod.Period != actualEvent.CollectionPeriod.Period ||
                expectedEvent.CollectionPeriod.AcademicYear != actualEvent.CollectionPeriod.AcademicYear ||
                expectedEvent.Learner.Uln != actualEvent.Learner.Uln ||
                expectedEvent.LearningAim.ProgrammeType != actualEvent.LearningAim.ProgrammeType ||
                expectedEvent.LearningAim.StandardCode != actualEvent.LearningAim.StandardCode)
                return false;

            if (!MatchOnProgrammeEarnings(expectedEvent as DataLockEvent, actualEvent as DataLockEvent))
                return false;

            return true;
        }

        private bool MatchOnProgrammeEarnings(DataLockEvent expectedEvent, DataLockEvent actualEvent)
        {
            if (expectedEvent == null)
                return true;

            var expectedEventOnProgrammeEarnings = expectedEvent.OnProgrammeEarnings ?? new List<OnProgrammeEarning>();
            var actualEventOnProgrammeEarnings = actualEvent.OnProgrammeEarnings ?? new List<OnProgrammeEarning>();

            foreach (var expectedEarning in expectedEventOnProgrammeEarnings)
            {
                var actualEarning = actualEventOnProgrammeEarnings.FirstOrDefault(a => a.Type == expectedEarning.Type);
                if (actualEarning == null)
                    return false;

                if (!MatchEarningPeriods(actualEarning.Periods, expectedEarning.Periods))
                    return false;
            }

            return true;
        }

        private static bool MatchEarningPeriods(ReadOnlyCollection<EarningPeriod> actualEarningPeriods, ReadOnlyCollection<EarningPeriod> expectedEarningPeriods)
        {
            if (actualEarningPeriods.Count != expectedEarningPeriods.Count)
                return false;

            foreach (var expectedEarningPeriod in expectedEarningPeriods)
            {
                var actualEarningPeriod = actualEarningPeriods.FirstOrDefault(x => x.Period == expectedEarningPeriod.Period);
                if (actualEarningPeriod == null)
                    return false;

                if (!MatchDataLockFailures(actualEarningPeriod.DataLockFailures, expectedEarningPeriod.DataLockFailures))
                    return false;
            }

            return true;
        }

        private static bool MatchDataLockFailures(List<DataLockFailure> actualDataLockFailures, List<DataLockFailure> expectedDataLockFailures)
        {
            if (actualDataLockFailures.Count != expectedDataLockFailures.Count)
                return false;

            foreach (var expectedDataLockFailure in expectedDataLockFailures)
            {
                var actualDataLockFailure = actualDataLockFailures.FirstOrDefault(x => x.DataLockError == expectedDataLockFailure.DataLockError);

                if (actualDataLockFailure?.ApprenticeshipId == null ||
                    actualDataLockFailure.ApprenticeshipId.Value != expectedDataLockFailure.ApprenticeshipId ||
                    actualDataLockFailure.ApprenticeshipPriceEpisodeIds == null ||
                    !actualDataLockFailure.ApprenticeshipPriceEpisodeIds.Any())
                    return false;

            }

            return true;
        }


    }


}
