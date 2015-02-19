CREATE TABLE [dbo].[ProjectTheme] (
    [ProjectId] INT NOT NULL,
    [ThemeId]   INT NOT NULL,
    CONSTRAINT [PK_dbo.ProjectTheme] PRIMARY KEY CLUSTERED ([ProjectId] ASC, [ThemeId] ASC),
    CONSTRAINT [FK_dbo.ProjectTheme_dbo.Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProjectTheme_dbo.Theme_ThemeId] FOREIGN KEY ([ThemeId]) REFERENCES [dbo].[Theme] ([ThemeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectId]
    ON [dbo].[ProjectTheme]([ProjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ThemeId]
    ON [dbo].[ProjectTheme]([ThemeId] ASC);

