CREATE TABLE [dbo].[PersonParticipantSevisCommStatus]
(    
	[Id] INT NOT NULL IDENTITY,
	[ParticipantId] INT NOT NULL , 
    [SevisCommStatusId] INT NOT NULL, 
    [AddedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_PersonParticipantSevisCommStatus] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_PersonParticipantSevisCommStatus_ToPersonParticipant] FOREIGN KEY ([ParticipantId]) REFERENCES [ParticipantPerson]([ParticipantId]), 
    CONSTRAINT [FK_PersonParticipantSevisCommStatus_ToSevisCommStatus] FOREIGN KEY ([SevisCommStatusId]) REFERENCES [SevisCommStatus]([SevisCommStatusId])
)

GO

CREATE INDEX [IX_PersonParticipantSevisCommStatus_Participant] ON [dbo].[PersonParticipantSevisCommStatus] ([ParticipantId])

GO

CREATE INDEX [IX_PersonParticipantSevisCommStatus_SevisCommStatus] ON [dbo].[PersonParticipantSevisCommStatus] ([SevisCommStatusId])
