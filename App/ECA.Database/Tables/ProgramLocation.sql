CREATE TABLE [dbo].[ProgramLocation] (
    [ProgramId]  INT NOT NULL,
    [LocationId] INT NOT NULL,
    CONSTRAINT [PK_dbo.ProgramLocation] PRIMARY KEY CLUSTERED ([ProgramId] ASC, [LocationId] ASC),
    CONSTRAINT [FK_dbo.ProgramLocation_dbo.Location_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProgramLocation_dbo.Program_ProgramId] FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[Program] ([ProgramId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProgramId]
    ON [dbo].[ProgramLocation]([ProgramId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LocationId]
    ON [dbo].[ProgramLocation]([LocationId] ASC);

