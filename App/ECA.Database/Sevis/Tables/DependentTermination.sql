CREATE TABLE [sevis].[DependentTermination]
(
	[DependentTerminationId] INT IDENTITY(1,1) NOT NULL, 
    [TerminationCode] NVARCHAR(2) NOT NULL, 
    [Description] NVARCHAR(100) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.DependentTermination] PRIMARY KEY ([DependentTerminationId])
)

GO

CREATE INDEX [IX_DependentTerminationCode] ON [sevis].[DependentTermination] ([TerminationCode])
