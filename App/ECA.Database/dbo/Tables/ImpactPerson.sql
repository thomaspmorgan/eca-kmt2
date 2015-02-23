CREATE TABLE [dbo].[ImpactPerson] (
    [ImpactId] INT NOT NULL,
    [PersonId] INT NOT NULL,
    CONSTRAINT [PK_dbo.ImpactPerson] PRIMARY KEY CLUSTERED ([ImpactId] ASC, [PersonId] ASC),
    CONSTRAINT [FK_dbo.ImpactPerson_dbo.Impact_ImpactId] FOREIGN KEY ([ImpactId]) REFERENCES [dbo].[Impact] ([ImpactId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ImpactPerson_dbo.Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ImpactId]
    ON [dbo].[ImpactPerson]([ImpactId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PersonId]
    ON [dbo].[ImpactPerson]([PersonId] ASC);

