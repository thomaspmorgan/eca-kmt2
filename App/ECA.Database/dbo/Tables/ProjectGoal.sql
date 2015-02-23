CREATE TABLE [dbo].[ProjectGoal] (
    [ProjectId] INT NOT NULL,
    [GoalId]    INT NOT NULL,
    CONSTRAINT [PK_dbo.ProjectGoal] PRIMARY KEY CLUSTERED ([ProjectId] ASC, [GoalId] ASC),
    CONSTRAINT [FK_dbo.ProjectGoal_dbo.Goal_GoalId] FOREIGN KEY ([GoalId]) REFERENCES [dbo].[Goal] ([GoalId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProjectGoal_dbo.Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectId]
    ON [dbo].[ProjectGoal]([ProjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_GoalId]
    ON [dbo].[ProjectGoal]([GoalId] ASC);

