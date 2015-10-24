CREATE TABLE [sevis].[ExchangeVisitorTerminationReason]
(
	[ExchangeVisitorTerminationReasonId] INT IDENTITY(1,1) NOT NULL, 
    [TerminationCode] NVARCHAR(20) NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.ExchangeVisitorTerminationReason] PRIMARY KEY ([ExchangeVisitorTerminationReasonId])
)

GO

CREATE INDEX [IX_TerminationCode] ON [sevis].[ExchangeVisitorTerminationReason] ([TerminationCode])
