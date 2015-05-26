CREATE TABLE [sevis].[FieldOfStudy]
(
	[FieldOfStudyId] INT IDENTITY (1, 1) NOT NULL, 
    [FieldOfStudyCode] CHAR(7) NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [HistoryCreatedBy] INT NOT NULL, 
    [HistoryCreatedOn] DATETIMEOFFSET NOT NULL, 
    [HistoryRevisedBy] INT NOT NULL, 
    [HistoryRevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.FieldOfStudy] PRIMARY KEY ([FieldOfStudyId])
)

GO



CREATE INDEX [IX_FieldOfStudyCode] ON [sevis].[FieldOfStudy] ([FieldOfStudyCode])
