CREATE TABLE [dbo].[Artifact] (
    [ArtifactId]        INT                IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (MAX)     NOT NULL,
    [Path]              NVARCHAR (MAX)     NULL,
    [ArtifactTypeId]    INT                NOT NULL,
    [Data]              VARBINARY (MAX)    NULL,
    [ActivityId]           INT                NULL,
    [ProjectId]         INT                NULL,
    [ProgramId]         INT                NULL,
    [PublicationId]     INT                NULL,
    [ItineraryStopId]   INT                NULL,
    [ImpactId]          INT                NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.Artifact] PRIMARY KEY CLUSTERED ([ArtifactId] ASC),
    CONSTRAINT [FK_dbo.Artifact_dbo.ArtifactType_ArtifactTypeId] FOREIGN KEY ([ArtifactTypeId]) REFERENCES [dbo].[ArtifactType] ([ArtifactTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Artifact_dbo.Activity_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activity] ([ActivityId]),
    CONSTRAINT [FK_dbo.Artifact_dbo.Impact_ImpactId] FOREIGN KEY ([ImpactId]) REFERENCES [dbo].[Impact] ([ImpactId]),
    CONSTRAINT [FK_dbo.Artifact_dbo.ItineraryStop_ItineraryStopId] FOREIGN KEY ([ItineraryStopId]) REFERENCES [dbo].[ItineraryStop] ([ItineraryStopId]),
    CONSTRAINT [FK_dbo.Artifact_dbo.Program_ProgramId] FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[Program] ([ProgramId]),
    CONSTRAINT [FK_dbo.Artifact_dbo.Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_dbo.Artifact_dbo.Publication_PublicationId] FOREIGN KEY ([PublicationId]) REFERENCES [dbo].[Publication] ([PublicationId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ArtifactTypeId]
    ON [dbo].[Artifact]([ArtifactTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ActivityId]
    ON [dbo].[Artifact]([ActivityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectId]
    ON [dbo].[Artifact]([ProjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProgramId]
    ON [dbo].[Artifact]([ProgramId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PublicationId]
    ON [dbo].[Artifact]([PublicationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ItineraryStopId]
    ON [dbo].[Artifact]([ItineraryStopId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ImpactId]
    ON [dbo].[Artifact]([ImpactId] ASC);

