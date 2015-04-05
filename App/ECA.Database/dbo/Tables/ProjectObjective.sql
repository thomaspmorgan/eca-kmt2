CREATE TABLE [dbo].[ProjectObjective]
(
	[ProjectId] INT NOT NULL , 
    [ObjectiveId] INT NOT NULL, 
	CONSTRAINT [PK_dbo.ProjectObjective] PRIMARY KEY CLUSTERED ([ProjectId] ASC, [ObjectiveId] ASC),
    CONSTRAINT [FK_dbo.ProjectObjective_dbo.ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_dbo.ProjectObjective_dbo.ObjectiveId] FOREIGN KEY ([ObjectiveId]) REFERENCES [dbo].[Objective] ([ObjectiveId])
	);

	
GO
CREATE NONCLUSTERED INDEX [IX_ProjectId]
    ON [dbo].[ProjectObjective]([ProjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ObjectiveId]
    ON [dbo].[ProjectObjective]([ObjectiveId] ASC);
