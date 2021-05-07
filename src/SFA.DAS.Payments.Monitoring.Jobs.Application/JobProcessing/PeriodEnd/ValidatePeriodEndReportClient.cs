﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Payments.Monitoring.Metrics.Model.PeriodEnd;

namespace SFA.DAS.Payments.Monitoring.Jobs.Application.JobProcessing.PeriodEnd
{
    public interface IValidatePeriodEndReportClient
    {
        Task<bool> RequestReports(long jobId, short academicYear, byte collectionPeriod);
    }

    public class ValidatePeriodEndReportClient : IValidatePeriodEndReportClient
    {
        private readonly string authCode;
        private readonly Uri functionAddressUri;

        public ValidatePeriodEndReportClient(string authCode, string functionAddress)
        {
            this.authCode = authCode;
            functionAddressUri = new Uri(functionAddress);
        }

        public async Task<bool> RequestReports(long jobId, short academicYear, byte collectionPeriod)
        {
            var result = await new HttpClient().GetAsync(BuildUriFromParameters(jobId, academicYear, collectionPeriod));

            if (!result.IsSuccessStatusCode) return false;

            var content = await result.Content.ReadAsStringAsync();
            var periodEndSummaryModel = JsonConvert.DeserializeObject<PeriodEndSummaryModel>(content);
            return periodEndSummaryModel.IsWithinTolerance.GetValueOrDefault();
        }

        private string BuildUriFromParameters(long jobId, short academicYear, byte collectionPeriod)
        {
            return string.IsNullOrWhiteSpace(authCode)
                ? $"{new Uri(functionAddressUri, "/api/RequestReports")}?jobId={jobId}&collectionPeriod={collectionPeriod}&AcademicYear={academicYear}"
                : $"{new Uri(functionAddressUri, "/api/RequestReports")}?code={authCode}&jobId={jobId}&collectionPeriod={collectionPeriod}&AcademicYear={academicYear}";
        }
    }
}