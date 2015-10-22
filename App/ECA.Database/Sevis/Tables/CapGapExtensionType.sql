CREATE TABLE [sevis].[CapGapExtensionType]
(
	[CapGapExtensionTypeId] INT IDENTITY(1,1) NOT NULL, 
    [ExtensionTypeCode] NVARCHAR(1) NOT NULL, 
    [Description] NVARCHAR(50) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.CapGapExtensionType] PRIMARY KEY ([CapGapExtensionTypeId])
)

GO

CREATE INDEX [IX_ExtensionTypeCode] ON [sevis].[CapGapExtensionType] ([ExtensionTypeCode])
