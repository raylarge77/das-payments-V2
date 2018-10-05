﻿Feature: Non-Levy - Basic Day - 2 learners - 1 finishes early and 1 finishes late

Background:
	Given the SFA contribution percentage is 90%
	And following learners are undertaking training with a training provider
	| LearnerId | 
	| L1		|
	| L2		|
	And the payments due component generates the following contract type 2 payments due:	
	| LearnerId | PriceEpisodeIdentifier | Delivery Period	| TransactionType | Amount |
	| L1		| p2                     | 1				| Learning (TT1)  | 1000   |
	| L1		| p2                     | 2				| Completion (TT2)| 3750   |
	| L1		| p2                     | 2				| Balancing (TT3) | 3000   |
	| L2		| p2                     | 1				| Learning (TT1)  | 1000   |
	| L2		| p2                     | 5				| Completion (TT2)| 3000   |

@NonDas_BasicDay
@Finishes_Early

Scenario: 2_non_levy_learners_1finishes_Early
	Given the current collection period is R02
	And the following historical contract type 2 payments exist:
	| LearnerId | PriceEpisodeIdentifier | Delivery Period	| TransactionType   | Amount |
	| L1		| p2                     | 1				| Learning (TT1)	| 1000   |
	
	When a payments due event is received
	Then the required payments component will generate the following contract type 2 payable earnings:
	| LearnerId | PriceEpisodeIdentifier | Delivery Period	| TransactionType   | Amount |
	| L1		| p2                     | 2				| Completion (TT2)	| 3750   |
	| L1		| p2                     | 2				| Balancing (TT3)	| 3000   |

@NonDas_BasicDay
@Finishes_Late

Scenario: 2_non_levy_learners_1finishes_Late
	Given the current collection period is R05
	And the following historical contract type 2 payments exist:
	| LearnerId | PriceEpisodeIdentifier | Delivery Period	| TransactionType   | Amount |
	| L2		| p2                     | 1				| Learning (TT1)	| 1000   |

	When a payments due event is received
	Then the required payments component will generate the following contract type 2 payable earnings:
	| LearnerId | PriceEpisodeIdentifier | Delivery Period	| TransactionType   | Amount |
	| L2		| p2                     | 5				| Completion (TT2)	| 3000   |

@NonDas_BasicDay
@Finishes_Early
@NoHistory

Scenario: 2_non_levy_learners_1finishes_Early - No history
	Given the current collection period is R02
	When a payments due event is received
	Then the required payments component will generate the following contract type 2 payable earnings:
	| LearnerId | PriceEpisodeIdentifier | Delivery Period	| TransactionType   | Amount |
	| L1		| p2                     | 1				| Learning (TT1)	| 1000   |
	| L1		| p2                     | 2				| Completion (TT2)	| 3750   |
	| L1		| p2                     | 2				| Balancing (TT3)	| 3000   |

@NonDas_BasicDay
@Finishes_Late
@NoHistory

Scenario: 2_non_levy_learners_1finishes_Late - No history
	Given the current collection period is R05
	When a payments due event is received
	Then the required payments component will generate the following contract type 2 payable earnings:
	| LearnerId | PriceEpisodeIdentifier | Delivery Period	| TransactionType   | Amount |
	| L2		| p2                     | 1				| Learning (TT1)	| 1000   |
	| L2		| p2                     | 5				| Completion (TT2)	| 3000   |
