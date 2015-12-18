CREATE TABLE [dbo].[ItineraryGroupParticipant]
(
	[ItineraryGroupId] INT NOT NULL,
	[ParticipantId] INT NOT NULL,

	CONSTRAINT [PK_dbo.ItineraryGroupParticipant] PRIMARY KEY CLUSTERED ([ItineraryGroupId] ASC, [ParticipantId] ASC),
	CONSTRAINT [FK_dbo.ItineraryGroupParticipant_dbo.ItineraryGroup_ItineraryGroupId] FOREIGN KEY ([ItineraryGroupId]) REFERENCES [dbo].[ItineraryGroup]([ItineraryGroupId]),
	CONSTRAINT [FK_dbo.ItineraryGroupParticipant_dbo.Participant_ParticipantId] FOREIGN KEY ([ParticipantId]) REFERENCES [dbo].[Participant]([ParticipantId])
)
