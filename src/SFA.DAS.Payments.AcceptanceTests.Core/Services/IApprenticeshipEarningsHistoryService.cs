﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Payments.AcceptanceTests.Core.Data;

namespace SFA.DAS.Payments.AcceptanceTests.Core.Services
{
    public interface IApprenticeshipEarningsHistoryService
    {
        Task AddHistoryAsync(IEnumerable<Learner> learners);

        void DeleteHistory(long ukprn);

        Task DeleteHistoryAsync(long ukprn);
    }
}
