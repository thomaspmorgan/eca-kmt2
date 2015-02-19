CREATE TABLE [dbo].[MoneyFlowType] (
    [MoneyFlowTypeId]   INT                IDENTITY (1, 1) NOT NULL,
    [MoneyFlowTypeName] NVARCHAR (80)      NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.MoneyFlowType] PRIMARY KEY CLUSTERED ([MoneyFlowTypeId] ASC)
);

