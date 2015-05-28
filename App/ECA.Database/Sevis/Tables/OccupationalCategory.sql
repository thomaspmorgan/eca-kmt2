CREATE TABLE [sevis].[OccupationalCategory]
(
	[OccupationalCategoryId] INT IDENTITY(1,1) NOT NULL, 
    [OccupationalCategoryCode] CHAR(2) NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.OccupationalCategory] PRIMARY KEY ([OccupationalCategoryId])
)

GO

CREATE INDEX [IX_OccupationalCategoryCode] ON [sevis].[OccupationalCategory] ([OccupationalCategoryCode])
