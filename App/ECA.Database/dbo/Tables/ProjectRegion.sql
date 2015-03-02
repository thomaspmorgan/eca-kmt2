CREATE TABLE [dbo].[ProjectRegion] (
    [ProjectId]  INT NOT NULL,
    [LocationId] INT NOT NULL,
    CONSTRAINT [PK_dbo.ProjectRegion] PRIMARY KEY CLUSTERED ([ProjectId] ASC, [LocationId] ASC),
    CONSTRAINT [FK_dbo.ProjectRegion_dbo.Location_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProjectRegion_dbo.Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectId]
    ON [dbo].[ProjectRegion]([ProjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LocationId]
    ON [dbo].[ProjectRegion]([LocationId] ASC);

