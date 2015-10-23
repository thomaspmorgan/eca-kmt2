CREATE TABLE [sevis].[StudentSecondaryMajorMinor]
(
	[ProgramSubjectId] INT IDENTITY(1,1) NOT NULL, 
    [ProgramSubjectCode] CHAR(7) NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.SecondaryMajorMinor] PRIMARY KEY ([ProgramSubjectId])
)

GO

CREATE INDEX [IX_ProgramSubjectCode] ON [sevis].[StudentSecondaryMajorMinor] ([ProgramSubjectCode])
