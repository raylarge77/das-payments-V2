﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.Payments.Core;
using SFA.DAS.Payments.FundingSource.Messages.Events;

namespace SFA.DAS.Payments.FundingSource.AcceptanceTests.Handlers
{
    public class CoInvestedFundingSourceHandler : IHandleMessages<CoInvestedFundingSourcePaymentEvent>
    {
        public static List<CoInvestedFundingSourcePaymentEvent> ReceivedEvents { get; } = new List<CoInvestedFundingSourcePaymentEvent>();

        public async Task Handle(CoInvestedFundingSourcePaymentEvent message, IMessageHandlerContext context)
        {
            Console.WriteLine(message.ToJson());
            ReceivedEvents.Add(message);
        }
    }

}