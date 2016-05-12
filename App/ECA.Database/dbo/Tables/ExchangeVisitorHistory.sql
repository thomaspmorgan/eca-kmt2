CREATE TABLE [dbo].[ExchangeVisitorHistory]
(
	[ParticipantId] INT NOT NULL PRIMARY KEY, 
	[RevisedOn] DATETIMEOFFSET NOT NULL, 
	[LastSuccessfulModel] NVARCHAR(MAX) NULL,
	[PendingModel] NVARCHAR(MAX) NULL,

	CONSTRAINT [FK_ExchangeVisitorHistory_ToParticipant] FOREIGN KEY ([ParticipantId]) REFERENCES [Participant]([ParticipantId]), 
)
