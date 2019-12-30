﻿using System.Collections.Generic;
using Autofac.Extras.Moq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.Model.Core.Entities;
using SFA.DAS.Payments.Monitoring.Metrics.Domain.Submission;
using SFA.DAS.Payments.Monitoring.Metrics.Model;

namespace SFA.DAS.Payments.Monitoring.Metrics.Domain.UnitTests.Submission.Summary
{
    [TestFixture]
    public class SubmissionSummaryEarningsTests
    {
        private List<TransactionTypeAmounts> dcEarnings;
        private List<TransactionTypeAmounts> dasEarnings;

        [SetUp]
        public void SetUp()
        {
            dcEarnings = TestsHelper.DefaultDcEarnings;
            dasEarnings = TestsHelper.DefaultDasEarnings;
        }

        protected SubmissionSummary GetSubmissionSummary => TestsHelper.DefaultSubmissionSummary;

        [Test]
        public void Calculates_Correct_Metrics_For_DcEarnings()
        {
            var submissionSummary =  GetSubmissionSummary;
            submissionSummary.AddEarnings(dcEarnings, dasEarnings);
            var metrics = submissionSummary.GetMetrics();
            metrics.DcEarnings.ContractType1.Should().Be(16300);
            metrics.DcEarnings.ContractType2.Should().Be(16300);
            metrics.DcEarnings.Total.Should().Be(32600);
            metrics.DcEarnings.Total.Should().Be(32600);
        }

        [Test]
        public void Calculates_Correct_Totals_For_DasEarnings()
        {
            var submissionSummary = GetSubmissionSummary;
            submissionSummary.AddEarnings(dcEarnings, dasEarnings);
            var metrics = submissionSummary.GetMetrics();
            metrics.DasEarnings.ContractType1.Should().Be(16300);
            metrics.DasEarnings.ContractType2.Should().Be(16300);
            metrics.DasEarnings.Total.Should().Be(32600);
            metrics.DasEarnings.Total.Should().Be(32600);
        }

        [Test]
        public void Calculates_Correct_Differences_For_DasEarnings()
        {
            var submissionSummary = GetSubmissionSummary;
            submissionSummary.AddEarnings(dcEarnings, dasEarnings);
            var metrics = submissionSummary.GetMetrics();
            metrics.DasEarnings.DifferenceContractType1.Should().Be(0);
            metrics.DasEarnings.DifferenceContractType2.Should().Be(0);
        }

        [Test]
        public void Calculates_Correct_Percentages_For_DasEarnings_Matching_DcEarnings()
        {
            var submissionSummary = GetSubmissionSummary;
            submissionSummary.AddEarnings(dcEarnings, dasEarnings);
            var metrics = submissionSummary.GetMetrics();
            metrics.DasEarnings.PercentageContractType1.Should().Be(100);
            metrics.DasEarnings.PercentageContractType2.Should().Be(100);
        }

        [Test]
        public void Calculates_Correct_Percentages_For_DasEarnings_With_No_Dc_Earnings()
        {
            var submissionSummary = GetSubmissionSummary;
            dcEarnings.Clear();
            submissionSummary.AddEarnings(dcEarnings, dasEarnings);
            var metrics = submissionSummary.GetMetrics();
            metrics.DasEarnings.PercentageContractType1.Should().Be(0);
            metrics.DasEarnings.PercentageContractType2.Should().Be(0);
        }

        [Test]
        public void Calculates_Correct_Percentages_For_DasEarnings_With_Greater_Dc_Earnings()
        {
            var submissionSummary = GetSubmissionSummary;
            dasEarnings = new List<TransactionTypeAmounts>
            {
                new TransactionTypeAmounts
                {
                    ContractType = ContractType.Act1,
                    TransactionType1 = 6000,
                    TransactionType2 = 0,
                    TransactionType3 = 1500,
                    TransactionType4 = 50,
                    TransactionType5 = 50,
                    TransactionType6 = 50,
                    TransactionType7 = 50,
                    TransactionType8 = 50,
                    TransactionType9 = 50,
                    TransactionType10 = 50,
                    TransactionType11 = 50,
                    TransactionType12 = 50,
                    TransactionType13 = 50,
                    TransactionType14 = 50,
                    TransactionType15 = 50,
                    TransactionType16 = 50,
                },
                new TransactionTypeAmounts
                {
                    ContractType = ContractType.Act2,
                    TransactionType1 = 3000,
                    TransactionType2 = 0,
                    TransactionType3 = 750,
                    TransactionType4 = 25,
                    TransactionType5 = 25,
                    TransactionType6 = 25,
                    TransactionType7 = 25,
                    TransactionType8 = 25,
                    TransactionType9 = 25,
                    TransactionType10 = 25,
                    TransactionType11 = 25,
                    TransactionType12 = 25,
                    TransactionType13 = 25,
                    TransactionType14 = 25,
                    TransactionType15 = 25,
                    TransactionType16 = 25,
                }
            };
            submissionSummary.AddEarnings(dcEarnings, dasEarnings);
            var metrics = submissionSummary.GetMetrics();
            metrics.DasEarnings.PercentageContractType1.Should().Be(50);
            metrics.DasEarnings.PercentageContractType2.Should().Be(25);
        }
    }
}