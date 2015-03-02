CREATE TABLE [dbo].[ItineraryStopParticipant] (
    [ItineraryStopId] INT NOT NULL,
    [ParticipantId]   INT NOT NULL,
    CONSTRAINT [PK_dbo.ItineraryStopParticipant] PRIMARY KEY CLUSTERED ([ItineraryStopId] ASC, [ParticipantId] ASC),
    CONSTRAINT [FK_dbo.ItineraryStopParticipant_dbo.ItineraryStop_ItineraryStopId] FOREIGN KEY ([ItineraryStopId]) REFERENCES [dbo].[ItineraryStop] ([ItineraryStopId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ItineraryStopParticipant_dbo.Participant_ParticipantId] FOREIGN KEY ([ParticipantId]) REFERENCES [dbo].[Participant] ([ParticipantId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ItineraryStopId]
    ON [dbo].[ItineraryStopParticipant]([ItineraryStopId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ParticipantId]
    ON [dbo].[ItineraryStopParticipant]([ParticipantId] ASC);

