CREATE TABLE [sevis].[EducationLevel]
(
	[EducationLevelId] INT IDENTITY(1,1) NOT NULL, 
    [EducationLevelCode] CHAR(2) NOT NULL, 
    [Description] NVARCHAR(100) NOT NULL, 
    [F_1_Ind] CHAR NOT NULL, 
    [M_1_Ind] CHAR NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.EducationLevel] PRIMARY KEY ([EducationLevelId])
)

GO

CREATE INDEX [IX_EducationLevelCode] ON [sevis].[EducationLevel] ([EducationLevelCode])
