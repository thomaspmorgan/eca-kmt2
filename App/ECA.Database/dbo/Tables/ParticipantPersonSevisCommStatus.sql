CREATE TABLE [dbo].[ParticipantPersonSevisCommStatus]
(    
	[Id] INT NOT NULL IDENTITY,
	[ParticipantId] INT NOT NULL , 
    [SevisCommStatusId] INT NOT NULL, 
    [AddedOn] DATETIMEOFFSET NOT NULL, 
    [BatchId] NVARCHAR(14) NULL,
    CONSTRAINT [PK_ParticipantPersonSevisCommStatus] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_ParticipantPersonSevisCommStatus_ToPersonParticipant] FOREIGN KEY ([ParticipantId]) REFERENCES [ParticipantPerson]([ParticipantId]), 
    CONSTRAINT [FK_ParticipantPersonSevisCommStatus_ToSevisCommStatus] FOREIGN KEY ([SevisCommStatusId]) REFERENCES [SevisCommStatus]([SevisCommStatusId])
)

GO

CREATE INDEX [IX_ParticipantPersonSevisCommStatus_Participant] ON [dbo].[ParticipantPersonSevisCommStatus] ([ParticipantId])

GO

CREATE INDEX [IX_ParticipantPersonSevisCommStatus_SevisCommStatus] ON [dbo].[ParticipantPersonSevisCommStatus] ([SevisCommStatusId])

GO

CREATE INDEX [IX_ParticipantPersonSevisCommStatus_BatchId] ON [dbo].[ParticipantPersonSevisCommStatus] ([BatchId])
