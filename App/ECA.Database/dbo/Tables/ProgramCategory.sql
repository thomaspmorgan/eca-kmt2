CREATE TABLE [dbo].[ProgramCategory]
(
	[ProgramId] INT NOT NULL , 
    [CategoryId] INT NOT NULL, 
	CONSTRAINT [PK_dbo.ProgramCategory] PRIMARY KEY CLUSTERED ([ProgramId] ASC, [CategoryId] ASC),
    CONSTRAINT [FK_dbo.ProgramCategory_dbo.ProgramId] FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[Program] ([ProgramId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProgramCategory_dbo.CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([CategoryId]) ON DELETE CASCADE
	);

	
GO
CREATE NONCLUSTERED INDEX [IX_ProgramId]
    ON [dbo].[ProgramCategory]([ProgramId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CategoryId]
    ON [dbo].[ProgramCategory]([CategoryId] ASC);