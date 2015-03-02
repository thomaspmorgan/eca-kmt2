CREATE TABLE [dbo].[MoneyFlowSourceRecipientType] (
    [MoneyFlowSourceRecipientTypeId] INT                IDENTITY (1, 1) NOT NULL,
    [TypeName]                       NVARCHAR (20)      NOT NULL,
    [History_CreatedBy]              INT                NOT NULL,
    [History_CreatedOn]              DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]              INT                NOT NULL,
    [History_RevisedOn]              DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.MoneyFlowSourceRecipientType] PRIMARY KEY CLUSTERED ([MoneyFlowSourceRecipientTypeId] ASC)
);

