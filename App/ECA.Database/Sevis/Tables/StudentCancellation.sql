CREATE TABLE [sevis].[StudentCancellation]
(
	[StudentCancellationId] INT IDENTITY(1,1) NOT NULL, 
    [CancellationCode] NVARCHAR(3) NOT NULL, 
    [Reason] NVARCHAR(100) NOT NULL, 
    [History_CreateBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.StudentCancellation] PRIMARY KEY ([StudentCancellationId])
)

GO

CREATE INDEX [IX_CancellationCode] ON [sevis].[StudentCancellation] ([CancellationCode])
