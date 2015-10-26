CREATE TABLE [sevis].[DependentCancellationReason]
(
	[DependentCancellationReasonId] INT IDENTITY(1,1) NOT NULL, 
    [ReasonCode] CHAR(2) NOT NULL, 
    [Description] NVARCHAR(100) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.DependentCancellationReason] PRIMARY KEY ([DependentCancellationReasonId])
)

GO

CREATE INDEX [IX_DependentCancellationReasonCode] ON [sevis].[DependentCancellationReason] ([ReasonCode])
