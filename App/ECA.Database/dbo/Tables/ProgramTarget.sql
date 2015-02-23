CREATE TABLE [dbo].[ProgramTarget] (
    [ProgramId]  INT NOT NULL,
    [LocationId] INT NOT NULL,
    CONSTRAINT [PK_dbo.ProgramTarget] PRIMARY KEY CLUSTERED ([ProgramId] ASC, [LocationId] ASC),
    CONSTRAINT [FK_dbo.ProgramTarget_dbo.Location_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProgramTarget_dbo.Program_ProgramId] FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[Program] ([ProgramId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProgramId]
    ON [dbo].[ProgramTarget]([ProgramId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LocationId]
    ON [dbo].[ProgramTarget]([LocationId] ASC);

