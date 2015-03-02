CREATE TABLE [dbo].[ArtifactType] (
    [ArtifactTypeId]    INT                IDENTITY (1, 1) NOT NULL,
    [Name]              INT                NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.ArtifactType] PRIMARY KEY CLUSTERED ([ArtifactTypeId] ASC)
);

