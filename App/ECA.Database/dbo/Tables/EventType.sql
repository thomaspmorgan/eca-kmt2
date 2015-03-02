CREATE TABLE [dbo].[EventType] (
    [EventTypeId]       INT                IDENTITY (1, 1) NOT NULL,
    [EventTypeName]     NVARCHAR (MAX)     NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.EventType] PRIMARY KEY CLUSTERED ([EventTypeId] ASC)
);

