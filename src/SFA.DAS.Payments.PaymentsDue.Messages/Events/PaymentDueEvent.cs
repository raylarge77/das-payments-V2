﻿using System;
using SFA.DAS.Payments.Model.Core;

namespace SFA.DAS.Payments.PaymentsDue.Messages.Events
{
    public abstract class PaymentDueEvent : IPaymentDueEvent
    {
        public DateTimeOffset EventTime { get; set; }
        public long Ukprn { get; set; }

        public string JobId { get; set; }
        
        public Learner Learner {get; set; }

        public LearningAim LearningAim { get; set; }

        public string PriceEpisodeIdentifier { get; set; }

        public decimal AmountDue { get; set; }

        public CalendarPeriod CollectionPeriod { get; set; }

        public CalendarPeriod DeliveryPeriod { get; set; }

        public DateTime SubmissionDate { get; set; }

        protected PaymentDueEvent()
        {
            EventTime = DateTimeOffset.UtcNow;
        }

    }
}
