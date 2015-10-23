CREATE TABLE [sevis].[EndProgramReason]
(
	[EndProgramReasonId] INT IDENTITY(1,1) NOT NULL, 
    [ReasonCode] NVARCHAR(10) NOT NULL, 
    [Description] NVARCHAR(200) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.EndProgramReason] PRIMARY KEY ([EndProgramReasonId])
)

GO

CREATE INDEX [IX_ExtensionTypeCode] ON [sevis].[EndProgramReason] ([ReasonCode])
