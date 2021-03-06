#@ignore issue to be fixed after go live
# DC Integration
#| learning support code | learning support date from | learning support date to |
#| 1                     | 06/08/2018                 | 06/10/2019               |
#| 1                     | 06/08/2018                 | 06/10/2019               |

Feature: Levy learner, takes an English qualification that has a planned end date that exceeds the actual end date of the programme aim and learning support is applicable to all learning PV2-803
		As a provider,
		I want a Levy learner, with Learning Support, where English & Maths exceeds end date of programme aim
		So that I am paid Learning Support by SFA until English & Maths completes

Scenario Outline: Levy learner, Eng aim planned end date exceeds the actual end date of the programme aim and learning support is applicable to all learning PV2-803
	Given the employer levy account balance in collection period <Collection_Period> is <Levy Balance>
	And the following commitments exist
        | start date                | end date                     | agreed price | status |Framework Code | Pathway Code | Programme Type | 
        | 01/Aug/Last Academic Year | 01/Aug/Current Academic Year | 15000        | active |593            | 1            | 20             | 
   And the provider previously submitted the following learner details
		| Start Date                | Planned Duration | Total Training Price | Total Training Price Effective Date | Total Assessment Price | Total Assessment Price Effective Date | Actual Duration | Completion Status | Contract Type | Aim Reference | SFA Contribution Percentage | Funding Line Type         | Framework Code | Pathway Code | Programme Type |
		| 06/Aug/Last Academic Year | 12 months        | 12000                | 06/Aug/Last Academic Year           | 3000                   | 06/Aug/Last Academic Year             |                 | continuing        | Act1          | ZPROG001      | 90%                         | 19-24 Apprenticeship Levy | 593            | 1            | 20             |

	And the following aims
		| Aim Type         | Aim Reference | Start Date                | Planned Duration | Actual Duration | Aim Sequence Number | Framework Code | Pathway Code | Programme Type | Funding Line Type         | Completion Status | Price Episode Id  |
		| Programme        | ZPROG001      | 06/Aug/Last Academic Year | 12 months        |                 | 1                   | 593            | 1            | 20             | 19-24 Apprenticeship Levy | continuing        | 1st price details |
		| Maths or English | 12345         | 06/Aug/Last Academic Year | 14 months        |                 | 2                   | 593            | 1            | 20             | 19-24 Apprenticeship Levy | continuing        | 2nd price details | 
	And price details as follows	
        | Price Episode Id  | Total Training Price | Total Training Price Effective Date | Total Assessment Price | Total Assessment Price Effective Date | Contract Type | Aim Sequence Number | SFA Contribution Percentage |
        | 1st price details | 15000                | 06/Aug/Last Academic Year           | 0                      | 06/Aug/Last Academic Year             | Act1          | 1                   | 90%                         |
        |                   | 0                    | 06/Aug/Last Academic Year           | 0                      | 06/Aug/Last Academic Year             | Act1          | 2                   | 100%                        |
    And the following earnings had been generated for the learner
        | Delivery Period        | On-Programme | Completion | Balancing | LearningSupport | OnProgrammeMathsAndEnglish | Aim Sequence Number | Price Episode Identifier |
        | Aug/Last Academic Year | 1000         | 0          | 0         | 150             | 0                          | 1                   | 1st price details        |
        | Sep/Last Academic Year | 1000         | 0          | 0         | 150             | 0                          | 1                   | 1st price details        |
        | Oct/Last Academic Year | 1000         | 0          | 0         | 150             | 0                          | 1                   | 1st price details        |
        | Nov/Last Academic Year | 1000         | 0          | 0         | 150             | 0                          | 1                   | 1st price details        |
        | Dec/Last Academic Year | 1000         | 0          | 0         | 150             | 0                          | 1                   | 1st price details        |
        | Jan/Last Academic Year | 1000         | 0          | 0         | 150             | 0                          | 1                   | 1st price details        |
        | Feb/Last Academic Year | 1000         | 0          | 0         | 150             | 0                          | 1                   | 1st price details        |
        | Mar/Last Academic Year | 1000         | 0          | 0         | 150             | 0                          | 1                   | 1st price details        |
        | Apr/Last Academic Year | 1000         | 0          | 0         | 150             | 0                          | 1                   | 1st price details        |
        | May/Last Academic Year | 1000         | 0          | 0         | 150             | 0                          | 1                   | 1st price details        |
        | Jun/Last Academic Year | 1000         | 0          | 0         | 150             | 0                          | 1                   | 1st price details        |
        | Jul/Last Academic Year | 1000         | 0          | 0         | 150             | 0                          | 1                   | 1st price details        |
        | Aug/Last Academic Year | 0            | 0          | 0         | 0               | 33.64                      | 2                   |                          |
        | Sep/Last Academic Year | 0            | 0          | 0         | 0               | 33.64                      | 2                   |                          |
        | Oct/Last Academic Year | 0            | 0          | 0         | 0               | 33.64                      | 2                   |                          |
        | Nov/Last Academic Year | 0            | 0          | 0         | 0               | 33.64                      | 2                   |                          |
        | Dec/Last Academic Year | 0            | 0          | 0         | 0               | 33.64                      | 2                   |                          |
        | Jan/Last Academic Year | 0            | 0          | 0         | 0               | 33.64                      | 2                   |                          |
        | Feb/Last Academic Year | 0            | 0          | 0         | 0               | 33.64                      | 2                   |                          |
        | Mar/Last Academic Year | 0            | 0          | 0         | 0               | 33.64                      | 2                   |                          |
        | Apr/Last Academic Year | 0            | 0          | 0         | 0               | 33.64                      | 2                   |                          |
        | May/Last Academic Year | 0            | 0          | 0         | 0               | 33.64                      | 2                   |                          |
        | Jun/Last Academic Year | 0            | 0          | 0         | 0               | 33.64                      | 2                   |                          |
        | Jul/Last Academic Year | 0            | 0          | 0         | 0               | 33.64                      | 2                   |                          |

    And the following provider payments had been generated
        | Collection Period      | Delivery Period        | Levy Payments | SFA Fully-Funded Payments | Transaction Type           |
        | R01/Last Academic Year | Aug/Last Academic Year | 1000          | 0                         | Learning                   |
        | R02/Last Academic Year | Sep/Last Academic Year | 1000          | 0                         | Learning                   |
        | R03/Last Academic Year | Oct/Last Academic Year | 1000          | 0                         | Learning                   |
        | R04/Last Academic Year | Nov/Last Academic Year | 1000          | 0                         | Learning                   |
        | R05/Last Academic Year | Dec/Last Academic Year | 1000          | 0                         | Learning                   |
        | R06/Last Academic Year | Jan/Last Academic Year | 1000          | 0                         | Learning                   |
        | R07/Last Academic Year | Feb/Last Academic Year | 1000          | 0                         | Learning                   |
        | R08/Last Academic Year | Mar/Last Academic Year | 1000          | 0                         | Learning                   |
        | R09/Last Academic Year | Apr/Last Academic Year | 1000          | 0                         | Learning                   |
        | R10/Last Academic Year | May/Last Academic Year | 1000          | 0                         | Learning                   |
        | R11/Last Academic Year | Jun/Last Academic Year | 1000          | 0                         | Learning                   |
        | R12/Last Academic Year | Jul/Last Academic Year | 1000          | 0                         | Learning                   |
        | R01/Last Academic Year | Aug/Last Academic Year | 0             | 150                       | LearningSupport            |
        | R02/Last Academic Year | Sep/Last Academic Year | 0             | 150                       | LearningSupport            |
        | R03/Last Academic Year | Oct/Last Academic Year | 0             | 150                       | LearningSupport            |
        | R04/Last Academic Year | Nov/Last Academic Year | 0             | 150                       | LearningSupport            |
        | R05/Last Academic Year | Dec/Last Academic Year | 0             | 150                       | LearningSupport            |
        | R06/Last Academic Year | Jan/Last Academic Year | 0             | 150                       | LearningSupport            |
        | R07/Last Academic Year | Feb/Last Academic Year | 0             | 150                       | LearningSupport            |
        | R08/Last Academic Year | Mar/Last Academic Year | 0             | 150                       | LearningSupport            |
        | R09/Last Academic Year | Apr/Last Academic Year | 0             | 150                       | LearningSupport            |
        | R10/Last Academic Year | May/Last Academic Year | 0             | 150                       | LearningSupport            |
        | R11/Last Academic Year | Jun/Last Academic Year | 0             | 150                       | LearningSupport            |
        | R12/Last Academic Year | Jul/Last Academic Year | 0             | 150                       | LearningSupport            |
        | R01/Last Academic Year | Aug/Last Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
        | R02/Last Academic Year | Sep/Last Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
        | R03/Last Academic Year | Oct/Last Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
        | R04/Last Academic Year | Nov/Last Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
        | R05/Last Academic Year | Dec/Last Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
        | R06/Last Academic Year | Jan/Last Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
        | R07/Last Academic Year | Feb/Last Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
        | R08/Last Academic Year | Mar/Last Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
        | R09/Last Academic Year | Apr/Last Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
        | R10/Last Academic Year | May/Last Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
        | R11/Last Academic Year | Jun/Last Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
        | R12/Last Academic Year | Jul/Last Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
    But aims details are changed as follows
		| Aim Type         | Aim Reference | Start Date                | Planned Duration | Actual Duration | Aim Sequence Number | Framework Code | Pathway Code | Programme Type | Funding Line Type         | Completion Status | Price Episode Id  |
		| Programme        | ZPROG001      | 06/Aug/Last Academic Year | 12 months        | 12 months       | 1                   | 593            | 1            | 20             | 19-24 Apprenticeship Levy | completed         | 1st price details |
		| Maths or English | 12345         | 06/Aug/Last Academic Year | 14 months        | 14 months       | 2                   | 593            | 1            | 20             | 19-24 Apprenticeship Levy | completed         | 2nd price details |
	And price details as follows	
        | Price Episode Id  | Total Training Price | Total Training Price Effective Date | Total Assessment Price | Total Assessment Price Effective Date | Contract Type | Aim Sequence Number | SFA Contribution Percentage |
        | 1st price details | 15000                | 06/Aug/Last Academic Year           | 0                      | 06/Aug/Last Academic Year             | Act1          | 1                   | 90%                         |
        |                   | 0                    | 06/Aug/Last Academic Year           | 0                      | 06/Aug/Last Academic Year             | Act1          | 2                   | 100%                        |
    When the amended ILR file is re-submitted for the learners in collection period <Collection_Period>
    Then the following learner earnings should be generated
        | Delivery Period           | On-Programme | Completion | Balancing | LearningSupport | OnProgrammeMathsAndEnglish | Aim Sequence Number | Price Episode Identifier | Contract Type |
        | Aug/Current Academic Year | 0            | 3000       | 0         | 0               | 0                          | 1                   | 1st price details        | Act1          |
        | Sep/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 1                   | 1st price details        | Act1          |
        | Oct/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 1                   | 1st price details        | Act1          |
        | Nov/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 1                   | 1st price details        | Act1          |
        | Dec/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 1                   | 1st price details        | Act1          |
        | Jan/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 1                   | 1st price details        | Act1          |
        | Feb/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 1                   | 1st price details        | Act1          |
        | Mar/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 1                   | 1st price details        | Act1          |
        | Apr/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 1                   | 1st price details        | Act1          |
        | May/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 1                   | 1st price details        | Act1          |
        | Jun/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 1                   | 1st price details        | Act1          |
        | Jul/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 1                   | 1st price details        | Act1          |
        | Aug/Current Academic Year | 0            | 0          | 0         | 150             | 33.64                      | 2                   |                          | Act1          |
        | Sep/Current Academic Year | 0            | 0          | 0         | 150             | 33.64                      | 2                   |                          | Act1          |
        | Oct/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 2                   |                          | Act1          |
        | Nov/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 2                   |                          | Act1          |
        | Dec/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 2                   |                          | Act1          |
        | Jan/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 2                   |                          | Act1          |
        | Feb/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 2                   |                          | Act1          |
        | Mar/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 2                   |                          | Act1          |
        | Apr/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 2                   |                          | Act1          |
        | May/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 2                   |                          | Act1          |
        | Jun/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 2                   |                          | Act1          |
        | Jul/Current Academic Year | 0            | 0          | 0         | 0               | 0                          | 2                   |                          | Act1          |

    And at month end only the following payments will be calculated
        | Collection Period         | Delivery Period           | On-Programme | Completion | Balancing | LearningSupport | OnProgrammeMathsAndEnglish |
        | R01/Current Academic Year | Aug/Current Academic Year | 0            | 3000       | 0         | 150             | 33.64                      |
        | R02/Current Academic Year | Sep/Current Academic Year | 0            | 0          | 0         | 150             | 33.64                      |
        | R03/Current Academic Year | Oct/Current Academic Year | 0            | 0          | 0         | 0               | 0                          |
        | R04/Current Academic Year | Nov/Current Academic Year | 0            | 0          | 0         | 0               | 0                          |
        | R05/Current Academic Year | Dec/Current Academic Year | 0            | 0          | 0         | 0               | 0                          |
        | R06/Current Academic Year | Jan/Current Academic Year | 0            | 0          | 0         | 0               | 0                          |
        | R07/Current Academic Year | Feb/Current Academic Year | 0            | 0          | 0         | 0               | 0                          |
        | R08/Current Academic Year | Mar/Current Academic Year | 0            | 0          | 0         | 0               | 0                          |
        | R09/Current Academic Year | Apr/Current Academic Year | 0            | 0          | 0         | 0               | 0                          |
        | R10/Current Academic Year | May/Current Academic Year | 0            | 0          | 0         | 0               | 0                          |
        | R11/Current Academic Year | Jun/Current Academic Year | 0            | 0          | 0         | 0               | 0                          |
        | R12/Current Academic Year | Jul/Current Academic Year | 0            | 0          | 0         | 0               | 0                          |
    And only the following provider payments will be recorded
        | Collection Period         | Delivery Period           | Levy Payments | SFA Fully-Funded Payments | Transaction Type           |
        | R01/Current Academic Year | Aug/Current Academic Year | 3000          | 0                         | Completion                 |
        | R01/Current Academic Year | Aug/Current Academic Year | 0             | 150                       | LearningSupport            |
        | R02/Current Academic Year | Sep/Current Academic Year | 0             | 150                       | LearningSupport            |
        | R01/Current Academic Year | Aug/Current Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
        | R02/Current Academic Year | Sep/Current Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
	And only the following provider payments will be generated
        | Collection Period         | Delivery Period           | Levy Payments | SFA Fully-Funded Payments | Transaction Type           |
        | R01/Current Academic Year | Aug/Current Academic Year | 3000          | 0                         | Completion                 |
        | R01/Current Academic Year | Aug/Current Academic Year | 0             | 150                       | LearningSupport            |
        | R02/Current Academic Year | Sep/Current Academic Year | 0             | 150                       | LearningSupport            |
        | R01/Current Academic Year | Aug/Current Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
        | R02/Current Academic Year | Sep/Current Academic Year | 0             | 33.64                     | OnProgrammeMathsAndEnglish |
Examples: 
        | Collection_Period         | Levy Balance |
        | R01/Current Academic Year | 3500         |
        | R02/Current Academic Year | 500          |
        | R03/Current Academic Year | 500          |