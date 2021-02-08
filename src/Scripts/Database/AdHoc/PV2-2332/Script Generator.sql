----------------------------------------------------------------
----	New strategy is to reverse the existing payments *without the transfers*
----		and then repay with the transfers
----------------------------------------------------------------

-- Add 1537 of the 1538 records

IF OBJECT_ID('tempdb..#Direct') IS NOT NULL DROP TABLE #Direct

SELECT P.LearnerUln, P.Ukprn, P.TransferSenderAccountId, P.AccountId, P.Amount, NEWID() [RequiredPaymentId], R.Amount [PaymentDue],
	P.ApprenticeshipId [CommitmentId], P.LearnerReferenceNumber, P.IlrSubmissionDateTime, P.LearningAimStandardCode, P.LearningAimProgrammeType,
	P.LearningAimFrameworkCode, P.LearningAimPathwayCode, P.ContractType, P.DeliveryPeriod, P.TransactionType, P.SfaContributionPercentage,
	P.LearningAimFundingLineType, P.LearningAimReference, P.LearningStartDate, NEWID() [EventId], P.FundingSource, P.PriceEpisodeIdentifier,
	P.EarningsStartDate, P.EarningsPlannedEndDate [PlannedEndDate], P.EarningsActualEndDate [ActualEndDate],
	P.EarningsCompletionStatus, P.EarningsCompletionAmount, P.EarningsInstalmentAmount, P.EarningsNumberOfInstalments,
	-1 [AimSeqNumber]
INTO #Direct
FROM [Payments2].[Payment] P
LEFT JOIN Payments2.FundingSourceEvent F
	ON P.FundingSourceEventId = F.EventId
LEFT JOIN Payments2.RequiredPaymentEvent R
	ON F.RequiredPaymentEventId = R.EventId
LEFT JOIN Payments2.EarningEvent E
	ON E.EventId = R.EarningEventId
WHERE 1 = 1
AND P.collectionperiod != 14 --This collectionPeriod had all the refunds correctly transfered
AND P.AcademicYear = 1920
AND FundingSource = 5
AND P.TransferSenderAccountId IS NOT NULL
AND P.apprenticeshipid IS NOT NULL
AND P.ApprenticeshipPriceEpisodeId IS NULL
AND R.EventId IS NOT NULL
AND p.LearnerUln NOT IN --This Uln's were accidentally fixed during R05 Script run so we don't want to fix them again
(
5262162869, 4260168567, 8368020882, 6767527148, 7843199968, 2974815684,
3419693485, 7109313998, 6742002629, 2655867679, 7247213921, 5740009862,
6713073874, 8923179549, 1035841257, 6648865201, 9745542538, 6553416035,
2827627689, 2750985047, 7031989980, 6422667190, 2042270996, 1002434717,
8998765720, 9278142432, 3140779134, 5318367851, 5849338488, 7721674424,
6091520066, 5023370088, 6668188078, 4736833253, 2836326009, 1415037255,
5872337328, 4417698154
)



-- Add the last remaining record
IF OBJECT_ID('tempdb..#CannotAfford') IS NOT NULL DROP TABLE #CannotAfford

Select P.LearnerUln, P.Ukprn, P.TransferSenderAccountId, P.AccountId, P.Amount, RP.EventId [RequiredPaymentId], RP.Amount [PaymentDue],
	P.ApprenticeshipId [CommitmentId], P.LearnerReferenceNumber, P.IlrSubmissionDateTime, P.LearningAimStandardCode, P.LearningAimProgrammeType,
	P.LearningAimFrameworkCode, P.LearningAimPathwayCode, P.ContractType, P.DeliveryPeriod, P.TransactionType, P.SfaContributionPercentage,
	P.LearningAimFundingLineType, P.LearningAimReference, P.LearningStartDate, P.EventId, P.FundingSource, P.PriceEpisodeIdentifier,
	P.EarningsStartDate, P.EarningsPlannedEndDate [PlannedEndDate], P.EarningsActualEndDate [ActualEndDate],
	P.EarningsCompletionStatus, P.EarningsCompletionAmount, P.EarningsInstalmentAmount, P.EarningsNumberOfInstalments,
	-1 [AimSeqNumber]
INTO #CannotAfford
from Payments2.Payment p
left join Payments2.RequiredPaymentEvent rp 
	on p.ukprn = rp.ukprn
    and p.accountId= rp.accountid
    and p.academicyear = rp.academicyear
    and p.collectionperiod = rp.collectionperiod
    and p.deliveryperiod = rp.deliveryperiod
    and p.learneruln = rp.learneruln
    and p.learningaimreference = rp.learningaimreference
    and p.learningaimprogrammetype = rp.learningaimprogrammetype
    and p.learningaimstandardcode = rp.learningaimstandardcode
    and p.learningaimframeworkcode = rp.learningaimframeworkcode
    and p.learningaimpathwaycode = rp.learningaimpathwaycode
    and p.transactiontype = rp.transactiontype
    and p.contracttype = rp.contracttype
LEFT JOIN Payments2.EarningEvent E
	ON RP.EarningEventId = E.EventId
where
    p.Id = 86570921


IF OBJECT_ID('tempdb..#Records') IS NOT NULL DROP TABLE #Records
; WITH Records AS (
	SELECT * 
	FROM #Direct
	UNION
	SELECT *
	FROM #CannotAfford
)
SELECT *
INTO #Records
FROM Records



DECLARE @collectionPeriodName VARCHAR(8) = '2021-R07'
DECLARE @collectionPeriodMonth NVARCHAR(2) = '02'
DECLARE @collectionPeriodYear NVARCHAR(4) = '2021'


SELECT 
	-- Insert the required payment
	'INSERT INTO PaymentsDue.RequiredPayments (Id, CommitmentId, AccountId, Uln, LearnRefNumber, Ukprn, IlrSubmissionDateTime,
	PriceEpisodeIdentifier, StandardCode, ProgrammeType, FrameworkCode, PathwayCode, ApprenticeshipContractType, DeliveryMonth,
	DeliveryYear, CollectionPeriodName, CollectionPeriodMonth, CollectionPeriodYear, TransactionType, AmountDue, SfaContributionPercentage,
	FundingLineType, UseLevyBalance, LearnAimRef, LearningStartDate, CommitmentVersionId, AccountVersionId, AimSeqNumber) 
	VALUES 
		(' + 
	'''' + CAST(RequiredPaymentId AS NVARCHAR(36)) + ''', ' + 
	CAST(CommitmentId AS NVARCHAR) + ', ' + 
	CAST(AccountId AS NVARCHAR) + ', ' + 
	CAST(LearnerUln AS NVARCHAR) + ', ' + 
	'''' + LearnerReferenceNumber + ''', ' + 
	CAST(Ukprn AS NVARCHAR) + ', ' + 
	'''' + CONVERT(varchar, IlrSubmissionDateTime, 111) + ''', ' + 
	'''' + PriceEpisodeIdentifier + ''', ' +
	CAST(LearningAimStandardCode AS NVARCHAR) + ', ' + 
	CAST(LearningAimProgrammeType AS NVARCHAR) + ', ' + 
	CAST(LearningAimFrameworkCode AS NVARCHAR) + ', ' + 
	CAST(LearningAimPathwayCode AS NVARCHAR) + ', ' + 
	CAST(ContractType AS NVARCHAR) + ', ' + 
	CASE WHEN DeliveryPeriod < 6 THEN CAST((DeliveryPeriod + 7) AS NVARCHAR) ELSE CAST((DeliveryPeriod - 5) AS NVARCHAR) END  + ', ' + 
	CASE WHEN DeliveryPeriod < 6 THEN '2019' ELSE '2020' END  + ', ' + 
	'''' + @collectionPeriodName  + ''', ' + 
	@collectionPeriodMonth + ', ' + 
	@collectionPeriodYear + ', ' + 
	CAST(TransactionType AS NVARCHAR) + ', ' + 
	CAST(Amount AS NVARCHAR) + ', ' + 
	CAST(SfaContributionPercentage AS NVARCHAR) + ', ' + 
	'''' + LearningAimFundingLineType  + ''', ' + 
	'0, ' + 
	'''' + LearningAimReference + ''', ' + 
	'''' + CONVERT(varchar, LearningStartDate, 111)  + ''',' + 
	''''',' +
	''''',' +
	CAST([AimSeqNumber] AS NVARCHAR) +
	');  
	
	' + 


	-- Insert the payment. This should only happen if the required payment was inserted without error
	'INSERT INTO Payments.Payments (PaymentId, RequiredPaymentId, DeliveryMonth, DeliveryYear, CollectionPeriodName, CollectionPeriodMonth, 
		CollectionPeriodYear, FundingSource, TransactionType, Amount) 
	VALUES 
		('	+ 
		'''' + CAST(EventId AS NVARCHAR(36)) + ''', ' + 
		'''' + CAST(RequiredPaymentId AS NVARCHAR(36)) + ''', ' + 
		CASE WHEN DeliveryPeriod < 6 THEN CAST(DeliveryPeriod + 7 AS NVARCHAR) ELSE CAST(DeliveryPeriod - 5 AS NVARCHAR) END  + ', ' + 
		CASE WHEN DeliveryPeriod < 6 THEN '2019' ELSE '2020' END  + ', ' + 
		'''' + @collectionPeriodName  + ''', ' + 
		@collectionPeriodMonth + ', ' + 
		@collectionPeriodYear + ', ' + 
		CAST(FundingSource AS NVARCHAR) + ', ' + 
		CAST(TransactionType AS NVARCHAR) + ', ' + 
		CAST(Amount AS NVARCHAR) +
	');  
	
	' +



	-- Insert the earning. This should only happen if the required payment was inserted without error
	'INSERT INTO PaymentsDue.Earnings (RequiredPaymentId, StartDate, PlannedEndDate, ActualEndDate, CompletionStatus, CompletionAmount,
		MonthlyInstallment, TotalInstallments) 
	VALUES 
		(' + 
	'''' + CAST(RequiredPaymentId AS NVARCHAR(36)) + ''', ' + 
	'''' + CONVERT(varchar, EarningsStartDate, 111) + ''', ' + 
	CASE WHEN PlannedEndDate IS NULL THEN 'null, ' ELSE '''' + CONVERT(varchar, PlannedEndDate, 111) + ''', ' END + 
	CASE WHEN ActualEndDate IS NULL THEN 'null, ' ELSE '''' + CONVERT(varchar, ActualEndDate, 111) + ''', ' END + 
	COALESCE(CAST(EarningsCompletionStatus AS NVARCHAR), 'null') + ', ' + 
	COALESCE(CAST(EarningsCompletionAmount AS NVARCHAR), 'null') + ', ' + 
	COALESCE(CAST(EarningsInstalmentAmount AS NVARCHAR), 'null') + ', ' + 
	COALESCE(CAST(EarningsNumberOfInstalments AS NVARCHAR), 'null') + 
	
	');  
	
	' +

	
	-- Insert the transfer payment always
	'INSERT INTO TransferPayments.AccountTransfers (SendingAccountId, ReceivingAccountId, RequiredPaymentId, CommitmentId, 
		Amount, TransferType, CollectionPeriodName, CollectionPeriodMonth, CollectionPeriodYear) 
	VALUES 
		(' + 
		CAST(TransferSenderAccountId AS NVARCHAR) + ', ' + 
		CAST(AccountId AS NVARCHAR) + ', ' + 
		'''' + CAST(RequiredPaymentId AS NVARCHAR(36)) + ''', ' + 
		CAST(CommitmentId AS NVARCHAR) + ', ' + 
		CAST(Amount AS NVARCHAR) + ', ' + 
		'1, ' + 
		'''' + @collectionPeriodName  + ''', ' + 
		@collectionPeriodMonth + ', ' + 
		@collectionPeriodYear +
	')'


FROM #Records
ORDER BY Ukprn, LearnerUln, DeliveryPeriod, TransactionType


SELECT
-- Insert the required payment
	'DELETE PaymentsDue.RequiredPayments 
	WHERE Id = ' + 
		'''' + CAST(RequiredPaymentId AS NVARCHAR(36)) + '''' + 

	-- Insert the payment. This should only happen if the required payment was inserted without error
	'DELETE Payments.Payments WHERE RequiredPaymentId = ' +
		'''' + CAST(RequiredPaymentId AS NVARCHAR(36)) + '''' + 
	
	-- Insert the earning. This should only happen if the required payment was inserted without error
	'DELETE PaymentsDue.Earnings WHERE RequiredPaymentId = ' +
		'''' + CAST(RequiredPaymentId AS NVARCHAR(36)) + '''' + 
		
	-- Insert the transfer payment always
	'DELETE TransferPayments.AccountTransfers WHERE RequiredPaymentId =  ' +
		'''' + CAST(RequiredPaymentId AS NVARCHAR(36)) + '''' 
		
FROM #Records




DROP TABLE #Records
DROP TABLE #Direct



-------------------------------------------------------------------------------------------------
--		Regenerate the list with new required payment ids
--			Generate insert scripts for the 3 payment tables - but not the transfers
-------------------------------------------------------------------------------------------------



SELECT P.LearnerUln, P.Ukprn, P.TransferSenderAccountId, P.AccountId, P.Amount, NEWID() [RequiredPaymentId], R.Amount [PaymentDue],
	P.ApprenticeshipId [CommitmentId], P.LearnerReferenceNumber, P.IlrSubmissionDateTime, P.LearningAimStandardCode, P.LearningAimProgrammeType,
	P.LearningAimFrameworkCode, P.LearningAimPathwayCode, P.ContractType, P.DeliveryPeriod, P.TransactionType, P.SfaContributionPercentage,
	P.LearningAimFundingLineType, P.LearningAimReference, P.LearningStartDate, NEWID() [EventId], P.FundingSource, P.PriceEpisodeIdentifier,
	P.EarningsStartDate, P.EarningsPlannedEndDate [PlannedEndDate], P.EarningsActualEndDate [ActualEndDate],
	P.EarningsCompletionStatus, P.EarningsCompletionAmount, P.EarningsInstalmentAmount, P.EarningsNumberOfInstalments,
	-1 [AimSeqNumber]
INTO #DirectNumber2
FROM [Payments2].[Payment] P
LEFT JOIN Payments2.FundingSourceEvent F
	ON P.FundingSourceEventId = F.EventId
LEFT JOIN Payments2.RequiredPaymentEvent R
	ON F.RequiredPaymentEventId = R.EventId
LEFT JOIN Payments2.EarningEvent E
	ON E.EventId = R.EarningEventId
WHERE 1 = 1
AND P.collectionperiod != 14 --This collectionPeriod had all the refunds correctly transfered
AND P.AcademicYear = 1920
AND FundingSource = 5
AND P.TransferSenderAccountId IS NOT NULL
AND P.apprenticeshipid IS NOT NULL
AND P.ApprenticeshipPriceEpisodeId IS NULL
AND R.EventId IS NOT NULL
AND p.LearnerUln NOT IN 
(
5262162869, 4260168567, 8368020882, 6767527148, 7843199968, 2974815684,
3419693485, 7109313998, 6742002629, 2655867679, 7247213921, 5740009862,
6713073874, 8923179549, 1035841257, 6648865201, 9745542538, 6553416035,
2827627689, 2750985047, 7031989980, 6422667190, 2042270996, 1002434717,
8998765720, 9278142432, 3140779134, 5318367851, 5849338488, 7721674424,
6091520066, 5023370088, 6668188078, 4736833253, 2836326009, 1415037255,
5872337328, 4417698154
)


; WITH Records AS (
	SELECT * 
	FROM #DirectNumber2
)
SELECT *
INTO #RecordsNumber2
FROM Records


SELECT
	-- Insert the required payment
	'INSERT INTO PaymentsDue.RequiredPayments (Id, CommitmentId, AccountId, Uln, LearnRefNumber, Ukprn, IlrSubmissionDateTime,
	PriceEpisodeIdentifier, StandardCode, ProgrammeType, FrameworkCode, PathwayCode, ApprenticeshipContractType, DeliveryMonth,
	DeliveryYear, CollectionPeriodName, CollectionPeriodMonth, CollectionPeriodYear, TransactionType, AmountDue, SfaContributionPercentage,
	FundingLineType, UseLevyBalance, LearnAimRef, LearningStartDate, CommitmentVersionId, AccountVersionId, AimSeqNumber) 
		VALUES 
	(' + 
	'''' + CAST(RequiredPaymentId AS NVARCHAR(36)) + ''', ' + 
	CAST(CommitmentId AS NVARCHAR) + ', ' + 
	CAST(AccountId AS NVARCHAR) + ', ' + 
	CAST(LearnerUln AS NVARCHAR) + ', ' + 
	'''' + LearnerReferenceNumber + ''', ' + 
	CAST(Ukprn AS NVARCHAR) + ', ' + 
	'''' + CONVERT(varchar, IlrSubmissionDateTime, 111) + ''', ' + 
	'''' + PriceEpisodeIdentifier + ''', ' +
	CAST(LearningAimStandardCode AS NVARCHAR) + ', ' + 
	CAST(LearningAimProgrammeType AS NVARCHAR) + ', ' + 
	CAST(LearningAimFrameworkCode AS NVARCHAR) + ', ' + 
	CAST(LearningAimPathwayCode AS NVARCHAR) + ', ' + 
	CAST(ContractType AS NVARCHAR) + ', ' + 
	CASE WHEN DeliveryPeriod < 6 THEN CAST((DeliveryPeriod + 7) AS NVARCHAR) ELSE CAST((DeliveryPeriod - 5) AS NVARCHAR) END  + ', ' + 
	CASE WHEN DeliveryPeriod < 6 THEN '2019' ELSE '2020' END  + ', ' + 
	'''' + @collectionPeriodName  + ''', ' + 
	@collectionPeriodMonth + ', ' + 
	@collectionPeriodYear + ', ' + 
	CAST(TransactionType AS NVARCHAR) + ', ' + 
	CAST(-Amount AS NVARCHAR) + ', ' + 
	CAST(SfaContributionPercentage AS NVARCHAR) + ', ' + 
	'''' + LearningAimFundingLineType  + ''', ' + 
	'0, ' + 
	'''' + LearningAimReference + ''', ' + 
	'''' + CONVERT(varchar, LearningStartDate, 111)  + ''',' + 
	''''',' +
	''''',' +
	CAST([AimSeqNumber] AS NVARCHAR) +
	');  

	' + 



	'INSERT INTO Payments.Payments (PaymentId, RequiredPaymentId, DeliveryMonth, DeliveryYear, CollectionPeriodName, CollectionPeriodMonth, 
		CollectionPeriodYear, FundingSource, TransactionType, Amount) 
	VALUES 
		('	+ 
		'''' + CAST(EventId AS NVARCHAR(36)) + ''', ' + 
		'''' + CAST(RequiredPaymentId AS NVARCHAR(36)) + ''', ' + 
		CASE WHEN DeliveryPeriod < 6 THEN CAST(DeliveryPeriod + 7 AS NVARCHAR) ELSE CAST(DeliveryPeriod - 5 AS NVARCHAR) END  + ', ' + 
		CASE WHEN DeliveryPeriod < 6 THEN '2019' ELSE '2020' END  + ', ' + 
		'''' + @collectionPeriodName  + ''', ' + 
		@collectionPeriodMonth + ', ' + 
		@collectionPeriodYear + ', ' + 
		CAST(FundingSource AS NVARCHAR) + ', ' + 
		CAST(TransactionType AS NVARCHAR) + ', ' + 
		CAST(-Amount AS NVARCHAR) +
	');  
	
	' +


	'INSERT INTO PaymentsDue.Earnings (RequiredPaymentId, StartDate, PlannedEndDate, ActualEndDate, CompletionStatus, CompletionAmount,
		MonthlyInstallment, TotalInstallments) 
	VALUES 
		(' + 
	'''' + CAST(RequiredPaymentId AS NVARCHAR(36)) + ''', ' + 
	'''' + CONVERT(varchar, EarningsStartDate, 111) + ''', ' + 
	CASE WHEN PlannedEndDate IS NULL THEN 'null, ' ELSE '''' + CONVERT(varchar, PlannedEndDate, 111) + ''', ' END + 
	CASE WHEN ActualEndDate IS NULL THEN 'null, ' ELSE '''' + CONVERT(varchar, ActualEndDate, 111) + ''', ' END + 
	COALESCE(CAST(EarningsCompletionStatus AS NVARCHAR), 'null') + ', ' + 
	COALESCE(CAST(EarningsCompletionAmount AS NVARCHAR), 'null') + ', ' + 
	COALESCE(CAST(EarningsInstalmentAmount AS NVARCHAR), 'null') + ', ' + 
	COALESCE(CAST(EarningsNumberOfInstalments AS NVARCHAR), 'null') + 
	
	');  ' 
 FROM #RecordsNumber2   
 ORDER BY Ukprn, LearnerUln, DeliveryPeriod, TransactionType


 SELECT
-- Insert the required payment
	'DELETE PaymentsDue.RequiredPayments 
	WHERE Id = ' + 
		'''' + CAST(RequiredPaymentId AS NVARCHAR(36)) + '''' + 

	-- Insert the payment. This should only happen if the required payment was inserted without error
	'DELETE Payments.Payments WHERE RequiredPaymentId = ' +
		'''' + CAST(RequiredPaymentId AS NVARCHAR(36)) + '''' + 
	
	-- Insert the earning. This should only happen if the required payment was inserted without error
	'DELETE PaymentsDue.Earnings WHERE RequiredPaymentId = ' +
		'''' + CAST(RequiredPaymentId AS NVARCHAR(36)) + '''' + 
		
	-- Insert the transfer payment always
	'DELETE TransferPayments.AccountTransfers WHERE RequiredPaymentId =  ' +
		'''' + CAST(RequiredPaymentId AS NVARCHAR(36)) + '''' 
		
FROM #RecordsNumber2   
 ORDER BY Ukprn, LearnerUln, DeliveryPeriod, TransactionType


 DROP TABLE #RecordsNumber2
 DROP TABLE #DirectNumber2