CREATE TABLE [sevis].[StudentReprint]
(
	[StudentReprintId] INT IDENTITY(1,1) NOT NULL, 
    [ReprintCode] CHAR(2) NOT NULL, 
    [ReprintReason] NVARCHAR(100) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.StudentReprint] PRIMARY KEY ([StudentReprintId])
)

GO

CREATE INDEX [IX_ReprintCode] ON [sevis].[StudentReprint] ([ReprintCode])
