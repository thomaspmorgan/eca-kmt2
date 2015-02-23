CREATE TABLE [dbo].[Actor] (
    [ActorId]           INT                IDENTITY (1, 1) NOT NULL,
    [ActorTypeId]       INT                NOT NULL,
    [ActorName]         NVARCHAR (MAX)     NOT NULL,
    [Status]            NVARCHAR (MAX)     NULL,
    [Action]            NVARCHAR (MAX)     NULL,
    [PersonId]          INT                NULL,
    [OrganizationId]    INT                NULL,
    [EventId]           INT                NULL,
    [ItineraryStopId]   INT                NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.Actor] PRIMARY KEY CLUSTERED ([ActorId] ASC),
    CONSTRAINT [FK_dbo.Actor_dbo.ActorType_ActorTypeId] FOREIGN KEY ([ActorTypeId]) REFERENCES [dbo].[ActorType] ([ActorTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Actor_dbo.Event_EventId] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([EventId]),
    CONSTRAINT [FK_dbo.Actor_dbo.ItineraryStop_ItineraryStopId] FOREIGN KEY ([ItineraryStopId]) REFERENCES [dbo].[ItineraryStop] ([ItineraryStopId]),
    CONSTRAINT [FK_dbo.Actor_dbo.Organization_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_dbo.Actor_dbo.Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId])
);


GO
CREATE NONCLUSTERED INDEX [IX_PersonId]
    ON [dbo].[Actor]([PersonId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OrganizationId]
    ON [dbo].[Actor]([OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EventId]
    ON [dbo].[Actor]([EventId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ItineraryStopId]
    ON [dbo].[Actor]([ItineraryStopId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ActorTypeId]
    ON [dbo].[Actor]([ActorTypeId] ASC);

