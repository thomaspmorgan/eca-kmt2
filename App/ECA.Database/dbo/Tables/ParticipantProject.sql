CREATE TABLE [dbo].[ParticipantProject] (
    [ParticipantId] INT NOT NULL,
    [ProjectId]     INT NOT NULL,
    CONSTRAINT [PK_dbo.ParticipantProject] PRIMARY KEY CLUSTERED ([ParticipantId] ASC, [ProjectId] ASC),
    CONSTRAINT [FK_dbo.ParticipantProject_dbo.Participant_ParticipantId] FOREIGN KEY ([ParticipantId]) REFERENCES [dbo].[Participant] ([ParticipantId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ParticipantProject_dbo.Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ParticipantId]
    ON [dbo].[ParticipantProject]([ParticipantId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectId]
    ON [dbo].[ParticipantProject]([ProjectId] ASC);

