CREATE TABLE [dbo].[Event] (
    [EventId]                INT                IDENTITY (1, 1) NOT NULL,
    [Title]                  NVARCHAR (MAX)     NOT NULL,
    [EventTypeId]            INT                NOT NULL,
    [EventDate]              DATETIMEOFFSET (7) NOT NULL,
    [LocationId]             INT                NOT NULL,
    [Description]            NVARCHAR (MAX)     NULL,
    [TargetAudience]         NVARCHAR (MAX)     NULL,
    [EsimatedAudienceSize]   INT                NOT NULL,
    [EsimatedNumberOfAlumni] INT                NOT NULL,
    [History_CreatedBy]      INT                NOT NULL,
    [History_CreatedOn]      DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]      INT                NOT NULL,
    [History_RevisedOn]      DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.Event] PRIMARY KEY CLUSTERED ([EventId] ASC),
    CONSTRAINT [FK_dbo.Event_dbo.EventType_EventTypeId] FOREIGN KEY ([EventTypeId]) REFERENCES [dbo].[EventType] ([EventTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Event_dbo.Location_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_EventTypeId]
    ON [dbo].[Event]([EventTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LocationId]
    ON [dbo].[Event]([LocationId] ASC);

