CREATE TABLE [dbo].[ProjectContact] (
    [ProjectId] INT NOT NULL,
    [ContactId] INT NOT NULL,
    CONSTRAINT [PK_dbo.ProjectContact] PRIMARY KEY CLUSTERED ([ProjectId] ASC, [ContactId] ASC),
    CONSTRAINT [FK_dbo.ProjectContact_dbo.Contact_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact] ([ContactId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProjectContact_dbo.Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectId]
    ON [dbo].[ProjectContact]([ProjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ContactId]
    ON [dbo].[ProjectContact]([ContactId] ASC);

