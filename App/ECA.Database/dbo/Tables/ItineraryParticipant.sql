CREATE TABLE [dbo].[ItineraryParticipant]
(
	[ItineraryId] INT NOT NULL,
	[ParticipantId] INT NOT NULL,

	CONSTRAINT [PK_dbo.ItineraryParticipant] PRIMARY KEY CLUSTERED ([ItineraryId] ASC, [ParticipantId] ASC),
	CONSTRAINT [FK_dbo.ItineraryParticipant_dbo.Itinerary_ItineraryId] FOREIGN KEY ([ItineraryId]) REFERENCES [dbo].[Itinerary]([ItineraryId]),
	CONSTRAINT [FK_dbo.ItineraryParticipant_dbo.Participant_ParticipantId] FOREIGN KEY ([ParticipantId]) REFERENCES [dbo].[Participant]([ParticipantId])
)
