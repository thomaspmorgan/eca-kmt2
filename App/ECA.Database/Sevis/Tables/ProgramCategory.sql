CREATE TABLE [sevis].[ProgramCategory]
(
	[ProgramCategoryId] INT IDENTITY(1,1) NOT NULL, 
    [CategoryCode] NVARCHAR(3) NOT NULL, 
    [Description] NVARCHAR(100) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.ProgramCategory] PRIMARY KEY ([ProgramCategoryId])
)

GO

CREATE INDEX [IX_CategoryCode] ON [sevis].[ProgramCategory] ([CategoryCode])
