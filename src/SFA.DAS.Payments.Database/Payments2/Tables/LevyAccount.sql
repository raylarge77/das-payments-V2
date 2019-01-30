﻿CREATE TABLE [Payments2].[LevyAccount]
(
	[AccountId] BIGINT NOT NULL , 
    [SequenceId] BIGINT NOT NULL, 
    [AccountHashId] VARCHAR(125) NOT NULL, 
    [AccountName] VARCHAR(255) NOT NULL, 
    [Balance] DECIMAL(18, 4) NOT NULL, 
    [IsLevyPayer] BIT NOT NULL, 
    [TransferAllowance] DECIMAL(18, 4) NOT NULL, 
    PRIMARY KEY ([AccountId], [SequenceId])
)
