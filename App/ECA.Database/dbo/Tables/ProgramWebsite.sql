CREATE TABLE [dbo].[ProgramWebsite]
(
	[ProgramId] INT NOT NULL , 
    [WebsiteId] INT NOT NULL, 
	CONSTRAINT [PK_dbo.ProgramWebsite] PRIMARY KEY CLUSTERED ([ProgramId] ASC, [WebsiteId] ASC),
    CONSTRAINT [FK_dbo.ProgramWebsite_dbo.ProgramId] FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[Program] ([ProgramId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProgramWebsite_dbo.WebsiteId] FOREIGN KEY ([WebsiteId]) REFERENCES [dbo].[Website] ([WebsiteId]) ON DELETE CASCADE
)

GO
CREATE NONCLUSTERED INDEX [IX_ProgramId]
    ON [dbo].[ProgramWebsite]([ProgramId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WebsiteId]
    ON [dbo].[ProgramWebsite]([WebsiteId] ASC);