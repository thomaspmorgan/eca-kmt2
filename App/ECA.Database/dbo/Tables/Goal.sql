﻿CREATE TABLE [dbo].[Goal] (
    [GoalId]            INT                IDENTITY (1, 1) NOT NULL,
    [GoalName]          NVARCHAR (300)     NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_dbo.Goal] PRIMARY KEY CLUSTERED ([GoalId] ASC)
);

