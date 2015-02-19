CREATE TABLE [dbo].[ProjectLocation] (
    [ProjectId]  INT NOT NULL,
    [LocationId] INT NOT NULL,
    CONSTRAINT [PK_dbo.ProjectLocation] PRIMARY KEY CLUSTERED ([ProjectId] ASC, [LocationId] ASC),
    CONSTRAINT [FK_dbo.ProjectLocation_dbo.Location_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProjectLocation_dbo.Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectId]
    ON [dbo].[ProjectLocation]([ProjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LocationId]
    ON [dbo].[ProjectLocation]([LocationId] ASC);

