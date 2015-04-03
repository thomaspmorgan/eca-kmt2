CREATE TABLE [dbo].[ProjectCategory]
(
	[ProjectId] INT NOT NULL , 
    [CategoryId] INT NOT NULL, 
	CONSTRAINT [PK_dbo.ProjectCategory] PRIMARY KEY CLUSTERED ([ProjectId] ASC, [CategoryId] ASC),
    CONSTRAINT [FK_dbo.ProjectCategory_dbo.ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_dbo.ProjectCategory_dbo.CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([CategoryId])
	);

	
GO
CREATE NONCLUSTERED INDEX [IX_ProjectId]
    ON [dbo].[ProjectCategory]([ProjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CategoryId]
    ON [dbo].[ProjectCategory]([CategoryId] ASC);