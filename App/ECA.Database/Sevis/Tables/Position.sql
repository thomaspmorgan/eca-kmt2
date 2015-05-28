CREATE TABLE [sevis].[Position]
(
	[PositionId] INT IDENTITY(1,1) NOT NULL, 
    [PositionCode] NVARCHAR(4) NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.Position] PRIMARY KEY ([PositionId])
)

GO

CREATE INDEX [IX_PositionCode] ON [sevis].[Position] ([PositionCode])
