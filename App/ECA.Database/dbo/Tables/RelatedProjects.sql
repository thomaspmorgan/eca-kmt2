CREATE TABLE [dbo].[RelatedProjects] (
    [ProjectId]        INT NOT NULL,
    [RelatedProjectId] INT NOT NULL,
    CONSTRAINT [PK_dbo.RelatedProjects] PRIMARY KEY CLUSTERED ([ProjectId] ASC, [RelatedProjectId] ASC),
    CONSTRAINT [FK_dbo.RelatedProjects_dbo.Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.RelatedProjects_dbo.Project_RelatedProjectId] FOREIGN KEY ([RelatedProjectId]) REFERENCES [dbo].[Project] ([ProjectId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectId]
    ON [dbo].[RelatedProjects]([ProjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RelatedProjectId]
    ON [dbo].[RelatedProjects]([RelatedProjectId] ASC);

