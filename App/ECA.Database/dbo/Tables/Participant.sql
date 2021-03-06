﻿CREATE TABLE [dbo].[Participant] (
    [ParticipantId]     INT                IDENTITY (1, 1) NOT NULL,
    [OrganizationId]    INT                NULL,
    [PersonId]          INT                NULL,
    [ParticipantTypeId] INT                NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    [ParticipantStatusId] INT NULL, 
    [StatusDate] DATETIMEOFFSET NULL, 
    [ProjectId] INT NOT NULL, 
    [IVLP_ParticipantId] NVARCHAR(32) NULL, 
    CONSTRAINT [PK_dbo.Participant] PRIMARY KEY CLUSTERED ([ParticipantId] ASC),
    CONSTRAINT [FK_dbo.Participant_dbo.Organization_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_dbo.Participant_dbo.ParticipantType_ParticipantTypeId] FOREIGN KEY ([ParticipantTypeId]) REFERENCES [dbo].[ParticipantType] ([ParticipantTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Participant_dbo.Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_dbo.Participant_dbo.Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project]([ProjectId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_Participant_ToParticipantStatus] FOREIGN KEY ([ParticipantStatusId]) REFERENCES [ParticipantStatus]([ParticipantStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_OrganizationId]
    ON [dbo].[Participant]([OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PersonId]
    ON [dbo].[Participant]([PersonId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ParticipantTypeId]
    ON [dbo].[Participant]([ParticipantTypeId] ASC);


GO

CREATE INDEX [IX_ProjectId] ON [dbo].[Participant] ([ProjectId])

GO

CREATE INDEX [IX_Participant_ParticipantStatusId] ON [dbo].[Participant] ([ParticipantStatusId])
