CREATE TABLE [dbo].[ProgramTheme] (
    [ProgramId] INT NOT NULL,
    [ThemeId]   INT NOT NULL,
    CONSTRAINT [PK_dbo.ProgramTheme] PRIMARY KEY CLUSTERED ([ProgramId] ASC, [ThemeId] ASC),
    CONSTRAINT [FK_dbo.ProgramTheme_dbo.Program_ProgramId] FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[Program] ([ProgramId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProgramTheme_dbo.Theme_ThemeId] FOREIGN KEY ([ThemeId]) REFERENCES [dbo].[Theme] ([ThemeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProgramId]
    ON [dbo].[ProgramTheme]([ProgramId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ThemeId]
    ON [dbo].[ProgramTheme]([ThemeId] ASC);

