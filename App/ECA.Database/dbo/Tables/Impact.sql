CREATE TABLE [dbo].[Impact] (
    [ImpactId]          INT                IDENTITY (1, 1) NOT NULL,
    [ProgramId]         INT                NULL,
    [ProjectId]         INT                NULL,
    [Description]       NVARCHAR (MAX)     NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.Impact] PRIMARY KEY CLUSTERED ([ImpactId] ASC),
    CONSTRAINT [FK_dbo.Impact_dbo.Program_ProgramId] FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[Program] ([ProgramId]),
    CONSTRAINT [FK_dbo.Impact_dbo.Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProgramId]
    ON [dbo].[Impact]([ProgramId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectId]
    ON [dbo].[Impact]([ProjectId] ASC);

