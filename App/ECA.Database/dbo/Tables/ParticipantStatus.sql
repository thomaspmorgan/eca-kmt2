CREATE TABLE [dbo].[ParticipantStatus] (
    [ParticipantStatusId]           INT                IDENTITY (1, 1) NOT NULL,
    [Status]                        NVARCHAR(50)                NOT NULL,
    [History_CreatedBy]             INT                NOT NULL,
    [History_CreatedOn]             DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]             INT                NOT NULL,
    [History_RevisedOn]             DATETIMEOFFSET (7) NOT NULL,
    [Itinerary_ItineraryId]         INT                NULL,
    [ItineraryStop_ItineraryStopId] INT                NULL,
    [StatusDate]                    DATETIMEOFFSET (7) DEFAULT ('0001-01-01T00:00:00.000+00:00') NULL,
    [ParticipantId]                 INT                DEFAULT ((0)) NULL,
    CONSTRAINT [PK_dbo.ParticipantStatus] PRIMARY KEY CLUSTERED ([ParticipantStatusId] ASC),
    CONSTRAINT [FK_dbo.ParticipantStatus_dbo.Itinerary_Itinerary_ItineraryId] FOREIGN KEY ([Itinerary_ItineraryId]) REFERENCES [dbo].[Itinerary] ([ItineraryId]),
    CONSTRAINT [FK_dbo.ParticipantStatus_dbo.ItineraryStop_ItineraryStop_ItineraryStopId] FOREIGN KEY ([ItineraryStop_ItineraryStopId]) REFERENCES [dbo].[ItineraryStop] ([ItineraryStopId]),
    CONSTRAINT [FK_dbo.ParticipantStatus_dbo.Participant_ParticipantId] FOREIGN KEY ([ParticipantId]) REFERENCES [dbo].[Participant] ([ParticipantId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ParticipantId]
    ON [dbo].[ParticipantStatus]([ParticipantId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Itinerary_ItineraryId]
    ON [dbo].[ParticipantStatus]([Itinerary_ItineraryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ItineraryStop_ItineraryStopId]
    ON [dbo].[ParticipantStatus]([ItineraryStop_ItineraryStopId] ASC);

