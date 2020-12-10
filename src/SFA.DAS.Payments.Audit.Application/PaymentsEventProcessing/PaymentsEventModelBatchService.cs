﻿using SFA.DAS.Payments.Application.Batch;
using SFA.DAS.Payments.Application.Infrastructure.Logging;
using SFA.DAS.Payments.Core.Configuration;
using SFA.DAS.Payments.Model.Core.Audit;

namespace SFA.DAS.Payments.Audit.Application.PaymentsEventProcessing
{
    public interface IPaymentsEventModelBatchService<T> : IBatchProcessingService<T> where T : IPaymentsEventModel
    {
    }

    public class PaymentsEventModelBatchService<T> : BatchProcessingService<T>, IPaymentsEventModelBatchService<T> where T : IPaymentsEventModel
    {
        public PaymentsEventModelBatchService(IConfigurationHelper configurationHelper, IBatchScopeFactory batchScopeFactory, IPaymentLogger logger)
        : base(configurationHelper, batchScopeFactory, logger)
        {
        }
    }
}