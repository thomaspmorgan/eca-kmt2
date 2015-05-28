CREATE TABLE [sevis].[StudentCreation]
(
	[StudentCreationId] INT IDENTITY(1,1) NOT NULL, 
    [CreationCode] CHAR NOT NULL, 
    [Description] NVARCHAR(100) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.StudentCreation] PRIMARY KEY ([StudentCreationId])
)

GO

CREATE INDEX [IX_CreationCode] ON [sevis].[StudentCreation] ([CreationCode])
