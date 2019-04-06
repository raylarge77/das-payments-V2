@ignore

#Scenario: Levy learner, with a framework uplift, finishes early, Balancing payment applied
#    
#    Given The learner is programme only DAS
#    And the apprenticeship funding band maximum is 9000
#    And levy balance > agreed price for all months
#	
#	And the following commitments exist:
#		| ULN       | priority | start date | end date   | agreed price |
#        | learner a | 1        | 06/08/2018 | 09/08/2019 | 8250         |
#	
#	
#    When an ILR file is submitted with the following data:
#		| ULN   | learner type           | agreed price | start date | planned end date | actual end date | completion status | framework code | programme type | pathway code | 
#        | a     | programme only non-DAS | 8250         | 06/08/2018 | 09/08/2019       | 06/05/2019      | Completed         | 403            | 2              | 1            | 
#
#    Then the provider earnings and payments break down as follows:
#        | Type                                    | 08/18 | 09/18 | 10/18 | ... | 04/19 | 05/19 | 06/19 |
#        | Provider Earned Total                   | 670   | 670   | 670   | ... | 670   | 4020  | 0     |
#        | Provider Earned from SFA                | 670   | 670   | 670   | ... | 670   | 4020  | 0     | (1650 + 360) Balancing + (1650 + 360) Completion
#        | Provider Earned from Employer           | 0     | 0     | 0     | ... | 0     | 0     | 0     | 
#        | Provider Paid by SFA                    | 0     | 670   | 670   | ... | 670   | 670   | 4020  |
#        | Payment due from Employer               | 0     | 0     | 0     | ... | 0     | 0     | 0     |
#        | Levy account debited                    | 0     | 550   | 550   | ... | 550   | 550   | 3300  |
#        | SFA Levy employer budget                | 550   | 550   | 550   | ... | 550   | 3300  | 0     | (1650 Balancing + 1650 Completion)
#        | SFA Levy co-funding budget              | 0     | 0     | 0     | ... | 0     | 0     | 0     |
#		| SFA non-Levy co-funding budget          | 0     | 0     | 0     | ... | 0     | 0     | 0     | 
#        | SFA non-Levy additional payments budget | 120   | 120   | 120   | ... | 120   | 720   | 0     | (360 Balancing + 360 Completion)
#
#      And the transaction types for the payments are:
#        | Payment type                 | 09/18 | 10/18 | ... | 04/19 | 05/19 | 06/19 |
#        | On-program                   | 550   | 550   | ... | 550   | 550   | 0     |
#        | Completion                   | 0     | 0     | ... | 0     | 0     | 1650  |
#        | Balancing                    | 0     | 0     | ... | 0     | 0     | 1650  |
#        | Framework uplift on-program  | 120   | 120   | ... | 120   | 120   | 0     |
#        | Framework uplift completion  | 0     | 0     | ... | 0     | 0     | 360   |
#        | Framework uplift balancing   | 0     | 0     | ... | 0     | 0     | 360   |
#        | Provider disadvantage uplift | 0     | 0     | ..  | 0     | 0     | 0     |



#For DC Integration
#To generate 120 framework uplift payment: the apprenticeship funding band maximum is 9000


Feature: Levy learner, with a framework uplift, finishes early, Balancing payment applied PV2-571
	As a provider,
	I want a Levy learner with a framework uplift, where the learner finishes earlier than planned end date, and a balancing payment is applied
	So that I am accurately paid my apprenticeship provision

Scenario: A Levy learner with a framework uplift payments finishes early PV2-571
	Given the employer levy account balance in collection period R10/Current Academic Year is 8750
	And the following commitments exist
		| start date                   | end date                  | agreed price |
		| 06/Aug/Current Academic Year | 09/Aug/Next Academic Year | 8250         |
	And the provider previously submitted the following learner details
		| Start Date                   | Planned Duration | Total Training Price | Total Training Price Effective Date | Total Assessment Price | Total Assessment Price Effective Date | Actual Duration | Completion Status | Contract Type | Aim Sequence Number | Aim Reference | Framework Code | Pathway Code | Programme Type | Funding Line Type                                                 | SFA Contribution Percentage |
		| 06/Aug/Current Academic Year | 12 months        | 8250                 | 06/Aug/Current Academic Year        | 0                      | 06/Aug/Current Academic Year          |                 | continuing        | Act1          | 1                   | ZPROG001      | 593            | 1            | 20             | 16-18 Apprenticeship (From May 2017) Levy Contract (non-procured) | 90%                         |
    And the following earnings had been generated for the learner
        | Delivery Period           | On-Programme | Completion | Balancing | OnProgramme16To18FrameworkUplift |
        | Aug/Current Academic Year | 550          | 0          | 0         | 120                              |
        | Sep/Current Academic Year | 550          | 0          | 0         | 120                              |
        | Oct/Current Academic Year | 550          | 0          | 0         | 120                              |
        | Nov/Current Academic Year | 550          | 0          | 0         | 120                              |
        | Dec/Current Academic Year | 550          | 0          | 0         | 120                              |
        | Jan/Current Academic Year | 550          | 0          | 0         | 120                              |
        | Feb/Current Academic Year | 550          | 0          | 0         | 120                              |
        | Mar/Current Academic Year | 550          | 0          | 0         | 120                              |
        | Apr/Current Academic Year | 550          | 0          | 0         | 120                              |
        | May/Current Academic Year | 550          | 0          | 0         | 120                              |
        | Jun/Current Academic Year | 550          | 0          | 0         | 120                              |
        | Jul/Current Academic Year | 550          | 0          | 0         | 120                              |
    And the following provider payments had been generated
        | Collection Period         | Delivery Period           | Levy Payments | SFA Fully-Funded Payments | Transaction Type                 |
        | R01/Current Academic Year | Aug/Current Academic Year | 550           | 0                         | Learning                         |
        | R02/Current Academic Year | Sep/Current Academic Year | 550           | 0                         | Learning                         |
        | R03/Current Academic Year | Oct/Current Academic Year | 550           | 0                         | Learning                         |
        | R04/Current Academic Year | Nov/Current Academic Year | 550           | 0                         | Learning                         |
        | R05/Current Academic Year | Dec/Current Academic Year | 550           | 0                         | Learning                         |
        | R06/Current Academic Year | Jan/Current Academic Year | 550           | 0                         | Learning                         |
        | R07/Current Academic Year | Feb/Current Academic Year | 550           | 0                         | Learning                         |
        | R08/Current Academic Year | Mar/Current Academic Year | 550           | 0                         | Learning                         |
        | R09/Current Academic Year | Apr/Current Academic Year | 550           | 0                         | Learning                         |
        | R01/Current Academic Year | Aug/Current Academic Year | 0             | 120                       | OnProgramme16To18FrameworkUplift |
        | R02/Current Academic Year | Sep/Current Academic Year | 0             | 120                       | OnProgramme16To18FrameworkUplift |
        | R03/Current Academic Year | Oct/Current Academic Year | 0             | 120                       | OnProgramme16To18FrameworkUplift |
        | R04/Current Academic Year | Nov/Current Academic Year | 0             | 120                       | OnProgramme16To18FrameworkUplift |
        | R05/Current Academic Year | Dec/Current Academic Year | 0             | 120                       | OnProgramme16To18FrameworkUplift |
        | R06/Current Academic Year | Jan/Current Academic Year | 0             | 120                       | OnProgramme16To18FrameworkUplift |
        | R07/Current Academic Year | Feb/Current Academic Year | 0             | 120                       | OnProgramme16To18FrameworkUplift |
        | R08/Current Academic Year | Mar/Current Academic Year | 0             | 120                       | OnProgramme16To18FrameworkUplift |
        | R09/Current Academic Year | Apr/Current Academic Year | 0             | 120                       | OnProgramme16To18FrameworkUplift |
    But the Provider now changes the Learner details as follows
		| Start Date                   | Planned Duration | Total Training Price | Total Training Price Effective Date | Total Assessment Price | Total Assessment Price Effective Date | Actual Duration | Completion Status | Contract Type | Aim Sequence Number | Aim Reference | Framework Code | Pathway Code | Programme Type | Funding Line Type                                                 | SFA Contribution Percentage |
		| 06/Aug/Current Academic Year | 12 months        | 8250                 | 06/Aug/Current Academic Year        | 0                      | 06/Aug/Current Academic Year          | 9 months        | completed         | Act1          | 1                   | ZPROG001      | 593            | 1            | 20             | 16-18 Apprenticeship (From May 2017) Levy Contract (non-procured) | 90%                         |
	When the amended ILR file is re-submitted for the learners in collection period R10/Current Academic Year
	Then the following learner earnings should be generated
        | Delivery Period           | On-Programme | Completion | Balancing | OnProgramme16To18FrameworkUplift | Completion16To18FrameworkUplift | Balancing16To18FrameworkUplift |
        | Aug/Current Academic Year | 550          | 0          | 0         | 120                              | 0                               | 0                              |
        | Sep/Current Academic Year | 550          | 0          | 0         | 120                              | 0                               | 0                              |
        | Oct/Current Academic Year | 550          | 0          | 0         | 120                              | 0                               | 0                              |
        | Nov/Current Academic Year | 550          | 0          | 0         | 120                              | 0                               | 0                              |
        | Dec/Current Academic Year | 550          | 0          | 0         | 120                              | 0                               | 0                              |
        | Jan/Current Academic Year | 550          | 0          | 0         | 120                              | 0                               | 0                              |
        | Feb/Current Academic Year | 550          | 0          | 0         | 120                              | 0                               | 0                              |
        | Mar/Current Academic Year | 550          | 0          | 0         | 120                              | 0                               | 0                              |
        | Apr/Current Academic Year | 550          | 0          | 0         | 120                              | 0                               | 0                              |
        | May/Current Academic Year | 0            | 1650       | 1650      | 0                                | 360                             | 360                            |
        | Jun/Current Academic Year | 0            | 0          | 0         | 0                                | 0                               | 0                              |
        | Jul/Current Academic Year | 0            | 0          | 0         | 0                                | 0                               | 0                              |
    And at month end only the following payments will be calculated
		| Collection Period         | Delivery Period           | On-Programme | Completion | Balancing | Completion16To18FrameworkUplift | Balancing16To18FrameworkUplift |
		| R10/Current Academic Year | May/Current Academic Year | 0            | 1650       | 1650      | 360                             | 360                            |
	And only the following provider payments will be recorded
		| Collection Period         | Delivery Period           | Levy Payments | SFA Fully-Funded Payments | Transaction Type                |
		| R10/Current Academic Year | May/Current Academic Year | 1650          | 0                         | Completion                      |
		| R10/Current Academic Year | May/Current Academic Year | 1650          | 0                         | Balancing                       |
		| R10/Current Academic Year | May/Current Academic Year | 0             | 360                       | Completion16To18FrameworkUplift |
		| R10/Current Academic Year | May/Current Academic Year | 0             | 360                       | Balancing16To18FrameworkUplift  |
	And only the following provider payments will be generated
		| Collection Period         | Delivery Period           | Levy Payments | SFA Fully-Funded Payments | Transaction Type                |
		| R10/Current Academic Year | May/Current Academic Year | 1650          | 0                         | Completion                      |
		| R10/Current Academic Year | May/Current Academic Year | 1650          | 0                         | Balancing                       |
		| R10/Current Academic Year | May/Current Academic Year | 0             | 360                       | Completion16To18FrameworkUplift |
		| R10/Current Academic Year | May/Current Academic Year | 0             | 360                       | Balancing16To18FrameworkUplift  |