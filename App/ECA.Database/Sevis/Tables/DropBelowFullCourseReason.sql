CREATE TABLE [sevis].[DropBelowFullCourseReason]
(
	[DropBelowFullCourseReasonId] INT IDENTITY(1,1) NOT NULL, 
    [ReasonCode] CHAR(2) NOT NULL, 
    [Description] NVARCHAR(250) NOT NULL, 
    [F_1_Ind] BIT NOT NULL, 
    [M_1_Ind] BIT NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.DropBelowFullCourseReason] PRIMARY KEY ([DropBelowFullCourseReasonId])
)

GO

CREATE INDEX [IX_ReasonCode] ON [sevis].[DropBelowFullCourseReason] ([ReasonCode])
