﻿using System.Collections.Generic;
using SFA.DAS.Payments.Model.Core;

namespace SFA.DAS.Payments.EarningEvents.Messages.Events.OnProgramme
{
    public abstract class OnProgrammeEarningEvent : EarningEvent
    {
        public List<OnProgrammeEarningPriceEpisode> PriceEpisodes { get; set; }
    }
}