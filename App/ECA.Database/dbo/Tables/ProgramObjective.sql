CREATE TABLE [dbo].[ProgramObjective]
(
	[ProgramId] INT NOT NULL , 
    [ObjectiveId] INT NOT NULL, 
	CONSTRAINT [PK_dbo.ProgramObjective] PRIMARY KEY CLUSTERED ([ProgramId] ASC, [ObjectiveId] ASC),
    CONSTRAINT [FK_dbo.ProgramObjective_dbo.ProgramId] FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[Program] ([ProgramId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProgramObjective_dbo.ObjectiveId] FOREIGN KEY ([ObjectiveId]) REFERENCES [dbo].[Objective] ([ObjectiveId]) ON DELETE CASCADE
	);

	
GO
CREATE NONCLUSTERED INDEX [IX_ProgramId]
    ON [dbo].[ProgramObjective]([ProgramId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ObjectiveId]
    ON [dbo].[ProgramObjective]([ObjectiveId] ASC);
