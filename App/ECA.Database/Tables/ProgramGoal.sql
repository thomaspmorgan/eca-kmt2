CREATE TABLE [dbo].[ProgramGoal] (
    [ProgramId] INT NOT NULL,
    [GoalId]    INT NOT NULL,
    CONSTRAINT [PK_dbo.ProgramGoal] PRIMARY KEY CLUSTERED ([ProgramId] ASC, [GoalId] ASC),
    CONSTRAINT [FK_dbo.ProgramGoal_dbo.Goal_GoalId] FOREIGN KEY ([GoalId]) REFERENCES [dbo].[Goal] ([GoalId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProgramGoal_dbo.Program_ProgramId] FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[Program] ([ProgramId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_GoalId]
    ON [dbo].[ProgramGoal]([GoalId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProgramId]
    ON [dbo].[ProgramGoal]([ProgramId] ASC);

