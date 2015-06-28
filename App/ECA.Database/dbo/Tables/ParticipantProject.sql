CREATE TABLE [dbo].[ParticipantProject] (
    [ParticipantId] INT NOT NULL,
    [ProjectId]     INT NOT NULL,
    CONSTRAINT [PK_dbo.ParticipantProject] PRIMARY KEY CLUSTERED ([ParticipantId] ASC, [ProjectId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ParticipantId]
    ON [dbo].[ParticipantProject]([ParticipantId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectId]
    ON [dbo].[ParticipantProject]([ProjectId] ASC);

