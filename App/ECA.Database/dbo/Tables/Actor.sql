CREATE TABLE [dbo].[Actor] (
    [ActorId]           INT                IDENTITY (1, 1) NOT NULL,
    [ActorTypeId]       INT                NOT NULL,
    [ActorName]         NVARCHAR (600)     NOT NULL,
    [Status]            NVARCHAR (100)     NULL,
    [Action]            NVARCHAR (100)     NULL,
    [PersonId]          INT                NULL,
    [OrganizationId]    INT                NULL,
    [ActivityId]           INT                NULL,
    [ItineraryStopId]   INT                NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    [AppointmentId] INT NULL, 
    [ProjectId] INT NULL, 
    CONSTRAINT [PK_dbo.Actor] PRIMARY KEY CLUSTERED ([ActorId] ASC),
    CONSTRAINT [FK_dbo.Actor_dbo.ActorType_ActorTypeId] FOREIGN KEY ([ActorTypeId]) REFERENCES [dbo].[ActorType] ([ActorTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Actor_dbo.Activity_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activity] ([ActivityId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Actor_dbo.ItineraryStop_ItineraryStopId] FOREIGN KEY ([ItineraryStopId]) REFERENCES [dbo].[ItineraryStop] ([ItineraryStopId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Actor_dbo.Organization_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Actor_dbo.Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_dbo.Actor_dbo.ItineraryAppointment_AppointmentId] FOREIGN KEY ([AppointmentId]) REFERENCES [dbo].[ItineraryAppointment]([AppointmentId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_dbo.Actor_dbo.Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project]([ProjectId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_PersonId]
    ON [dbo].[Actor]([PersonId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OrganizationId]
    ON [dbo].[Actor]([OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ActivityId]
    ON [dbo].[Actor]([ActivityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ItineraryStopId]
    ON [dbo].[Actor]([ItineraryStopId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ActorTypeId]
    ON [dbo].[Actor]([ActorTypeId] ASC);


GO

CREATE INDEX [IX_AppointmentId] ON [dbo].[Actor] ([AppointmentId])

GO

CREATE INDEX [IX_ProjectId] ON [dbo].[Actor] ([ProjectId])
