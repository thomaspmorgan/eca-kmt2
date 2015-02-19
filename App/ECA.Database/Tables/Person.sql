CREATE TABLE [dbo].[Person] (
    [PersonId]                          INT                NOT NULL,
    [Gender]                            INT                NOT NULL,
    [DateOfBirth]                       DATETIMEOFFSET (7) NOT NULL,
    [Ethnicity]                         NVARCHAR (MAX)     NULL,
    [PermissionToContact]               BIT                NOT NULL,
    [EvaluationRetention]               NVARCHAR (MAX)     NULL,
    [History_CreatedBy]                 INT                NOT NULL,
    [History_CreatedOn]                 DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]                 INT                NOT NULL,
    [History_RevisedOn]                 DATETIMEOFFSET (7) NOT NULL,
    [Person_PersonId]                   INT                NULL,
    [ParticipantOrigination_LocationId] INT                NULL,
    [Location_LocationId]               INT                NULL,
    [ItineraryStop_ItineraryStopId]     INT                NULL,
    CONSTRAINT [PK_dbo.Person] PRIMARY KEY CLUSTERED ([PersonId] ASC),
    CONSTRAINT [FK_dbo.Person_dbo.ItineraryStop_ItineraryStop_ItineraryStopId] FOREIGN KEY ([ItineraryStop_ItineraryStopId]) REFERENCES [dbo].[ItineraryStop] ([ItineraryStopId]),
    CONSTRAINT [FK_dbo.Person_dbo.Location_Location_LocationId] FOREIGN KEY ([Location_LocationId]) REFERENCES [dbo].[Location] ([LocationId]),
    CONSTRAINT [FK_dbo.Person_dbo.Location_ParticipantOrigination_LocationId] FOREIGN KEY ([ParticipantOrigination_LocationId]) REFERENCES [dbo].[Location] ([LocationId]),
    CONSTRAINT [FK_dbo.Person_dbo.Location_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Location] ([LocationId]),
    CONSTRAINT [FK_dbo.Person_dbo.Person_Person_PersonId] FOREIGN KEY ([Person_PersonId]) REFERENCES [dbo].[Person] ([PersonId])
);


GO
CREATE NONCLUSTERED INDEX [IX_PersonId]
    ON [dbo].[Person]([PersonId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Person_PersonId]
    ON [dbo].[Person]([Person_PersonId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ParticipantOrigination_LocationId]
    ON [dbo].[Person]([ParticipantOrigination_LocationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Location_LocationId]
    ON [dbo].[Person]([Location_LocationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ItineraryStop_ItineraryStopId]
    ON [dbo].[Person]([ItineraryStop_ItineraryStopId] ASC);

