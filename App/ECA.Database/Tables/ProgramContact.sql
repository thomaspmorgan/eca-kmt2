CREATE TABLE [dbo].[ProgramContact] (
    [ProgramId] INT NOT NULL,
    [ContactId] INT NOT NULL,
    CONSTRAINT [PK_dbo.ProgramContact] PRIMARY KEY CLUSTERED ([ProgramId] ASC, [ContactId] ASC),
    CONSTRAINT [FK_dbo.ProgramContact_dbo.Contact_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact] ([ContactId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProgramContact_dbo.Program_ProgramId] FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[Program] ([ProgramId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ContactId]
    ON [dbo].[ProgramContact]([ContactId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProgramId]
    ON [dbo].[ProgramContact]([ProgramId] ASC);

