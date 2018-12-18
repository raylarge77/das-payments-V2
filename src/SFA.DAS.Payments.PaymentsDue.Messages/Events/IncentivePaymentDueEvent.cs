﻿using SFA.DAS.Payments.Model.Core.Entities;
using SFA.DAS.Payments.Model.Core.Incentives;

namespace SFA.DAS.Payments.PaymentsDue.Messages.Events
{
    public class FunctionalSkillPaymentDueEvent : PaymentDueEvent
    {
        public FunctionalSkillType Type { get; set; }
    }

    public class IncentivePaymentDueEvent : PaymentDueEvent
    {
        public IncentiveType Type { get; set; }
        public ContractType ContractType { get; set; }
    }
}