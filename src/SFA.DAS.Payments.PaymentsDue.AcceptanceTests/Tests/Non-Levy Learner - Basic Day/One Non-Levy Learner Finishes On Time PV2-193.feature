﻿Feature: One Non-Levy Learner Finishes On Time PV2-193

Background:
	Given the current collection period is R03
	And the payments are for the current collection year
	And a learner is undertaking a training with a training provider
	And the SFA contribution percentage is 90%
	And planned course duration is 12 months
	And the following course information:
	| AimSeqNumber | ProgrammeType | FrameworkCode | PathwayCode | StandardCode | FundingLineType                                                       | LearnAimRef | TotalNegotiatedPrice | CompletionStatus |
	| 1            | 2             | 403           | 1           |              | 16-18 Apprenticeship (From May 2017) Non-Levy Contract (non-procured) | ZPROG001    | 15000                | completed	    |

And the following contract type 2 On Programme earnings are provided:
	| PriceEpisodeIdentifier | Delivery Period	| TransactionType | Amount |
	| p2                     | 1				| Learning (TT1)  | 1000   |
	| p2                     | 2				| Completion (TT2)| 3000   |

@NonDas_BasicDay
Scenario: 1_non_levy_learner_finishes_OnTime
	When an earnings event is received
	Then the payments due component will generate the following contract type 2 payments due:
	| PriceEpisodeIdentifier | Delivery Period	| TransactionType   | Amount	|
	| p2                     | 1 				| Learning (TT1)	| 1000		|
	| p2                     | 2 				| Completion (TT2)	| 3000		|