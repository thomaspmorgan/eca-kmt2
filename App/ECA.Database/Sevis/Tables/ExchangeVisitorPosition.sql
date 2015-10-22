CREATE TABLE [sevis].[ExchangeVisitorPosition]
(
	[ExchangeVisitorPositionId] INT IDENTITY(1,1) NOT NULL, 
    [PositionCode] NVARCHAR(4) NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.ExchangeVisitorPosition] PRIMARY KEY ([ExchangeVisitorPositionId])
)

GO

CREATE INDEX [IX_ExchangeVisitorPositionCode] ON [sevis].[ExchangeVisitorPosition] ([PositionCode])

GO

CREATE INDEX [IX_Description] ON [sevis].[Position] ([Description])
