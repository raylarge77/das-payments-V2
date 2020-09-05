﻿using System.Collections.Generic;
using TechTalk.SpecFlow.Assist.Attributes;

namespace SFA.DAS.Payments.AcceptanceTests.Core.Data
{
    public class Learner
    {
        public long Ukprn { get; set; }
        [TableAliases("Learner[ ]?Reference[ ]?Number")]
        public string LearnRefNumber { get; set; }
        public long Uln { get; set; }
        public Course Course { get; set; }

        public string LearnerIdentifier { get; set; }
        public List<Aim> Aims { get; set; } = new List<Aim>();

        public int? EefCode { get; set; }
        public string PostcodePrior { get; set; }
        public bool IsLevyLearner { get; set; }

        public List<EmploymentStatusMonitoring> EmploymentStatusMonitoring { get; set; } = new List<EmploymentStatusMonitoring>();

        public long? OriginalUln { get; set; }

        public bool Restart { get; set; }

        public LearnerEarningsHistory EarningsHistory { get; set; }

        public override string ToString()
        {
            return $"Learn Ref Number: [ {LearnRefNumber} ]\tUln: [ {Uln} ]\t\tLearner Identifier: [ {LearnerIdentifier} ]";
        }
    }
}