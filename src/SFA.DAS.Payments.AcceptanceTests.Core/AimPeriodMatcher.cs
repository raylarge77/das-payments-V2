﻿using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Core.Data;
using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.Tests.Core;
using SFA.DAS.Payments.Tests.Core.Builders;

namespace SFA.DAS.Payments.AcceptanceTests.Core
{
    public static class AimPeriodMatcher
    {
        public static bool IsStartDateValidForCollectionPeriod(
            string startDate,
            CollectionPeriod collectionPeriod,
            TimeSpan? plannedDurationAsTimeSpan,
            TimeSpan? actualDurationAsTimeSpan,
            CompletionStatus completionStatus,
            string aimReference)
        {
            var aimStartDate = startDate.ToDate();
            var aimStartPeriod = new CollectionPeriodBuilder().WithDate(aimStartDate).Build();
            var aimDuration = actualDurationAsTimeSpan ?? plannedDurationAsTimeSpan;
            var collectionPeriodReferenceDate = DateFromCollectionPeriod(collectionPeriod);

            if (GetInactiveStatuses().Contains(completionStatus))
            {
                if (
                    (aimStartPeriod.AcademicYear < collectionPeriod.AcademicYear &&
                     DurationGreaterThanCollectionPeriod(aimStartDate, aimDuration, collectionPeriodReferenceDate)) ||
                    (aimStartPeriod.AcademicYear == collectionPeriod.AcademicYear &&
                     aimStartDate <= collectionPeriodReferenceDate && DurationGreaterThanCollectionPeriod(aimStartDate, aimDuration, collectionPeriodReferenceDate)))
                {
                    return true;
                }

                return false;
            }

            if (GetActiveStatuses().Contains(completionStatus) &&
                (aimStartPeriod.AcademicYear < collectionPeriod.AcademicYear &&
                 DurationGreaterThanCollectionPeriod(aimStartDate, aimDuration, collectionPeriodReferenceDate)))
            {
                return true;
            }

            if (completionStatus == CompletionStatus.Completed &&
                (aimStartPeriod.AcademicYear == collectionPeriod.AcademicYear &&
                 aimStartDate <= collectionPeriodReferenceDate &&
                 aimStartDate + aimDuration < collectionPeriodReferenceDate))
            {
                return true;
            }

            if (GetActiveStatuses().Contains(completionStatus) &&
                (aimStartPeriod.AcademicYear < collectionPeriod.AcademicYear &&
                 aimStartDate <= collectionPeriodReferenceDate &&
                 DurationGreaterThanCollectionPeriod(aimStartDate, aimDuration, collectionPeriodReferenceDate)))
            {
                return true;
            }

            return aimStartPeriod.AcademicYear == collectionPeriod.AcademicYear &&
                   aimStartDate <= collectionPeriodReferenceDate &&
                   DurationGreaterThanCollectionPeriod(aimStartDate, aimDuration, collectionPeriodReferenceDate);
        }

        private static DateTime DateFromCollectionPeriod(CollectionPeriod collectionPeriod)
        {
            var year = Convert.ToInt16(collectionPeriod.AcademicYear.ToString().Substring(0, 2)) + 2000;

            return collectionPeriod.Period < 6
                ? new DateTime(year, collectionPeriod.Period + 7, 1).AddMonths(1).AddDays(-1)
                : new DateTime(year + 1, collectionPeriod.Period - 5, 1).AddMonths(1).AddDays(-1);
        }

        private static bool DurationGreaterThanCollectionPeriod(DateTime startDate, TimeSpan? duration,
            DateTime collectionPeriod)
        {
            var aimEndDate = startDate + duration;

            return collectionPeriod.FirstDayOfMonth() <= aimEndDate;
        }

        private static IEnumerable<CompletionStatus> GetInactiveStatuses()
        {
            yield return CompletionStatus.BreakInLearning;
            yield return CompletionStatus.Withdrawn;
        }

        private static IEnumerable<CompletionStatus> GetActiveStatuses()
        {
            yield return CompletionStatus.Continuing;
            yield return CompletionStatus.Completed;
        }

        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime LastDayOnMonth(this DateTime date)
        {
            return date.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }
    }
}