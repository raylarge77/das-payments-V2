﻿using System.Collections.Generic;
using SFA.DAS.Payments.Messages.Core.Events;
using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.Model.Core.Incentives;
using SFA.DAS.Payments.Model.Core.OnProgramme;

namespace SFA.DAS.Payments.DataLocks.Messages
{
    public abstract class DataLockEvent : PaymentsEvent, IContractType1EarningEvent
    {
        public List<PriceEpisode> PriceEpisodes { get; set; }
        public short CollectionYear { get; set; }
        public string AgreementId { get; set; }
        public decimal SfaContributionPercentage { get; set; }
        public List<OnProgrammeEarning> OnProgrammeEarnings { get; set; }
        public List<IncentiveEarning> IncentiveEarnings { get; set; }
    }
}