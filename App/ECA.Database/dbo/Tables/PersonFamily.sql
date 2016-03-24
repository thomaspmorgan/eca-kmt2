CREATE TABLE [dbo].[PersonFamily] (
    [PersonId]        INT NOT NULL,
    [RelatedPersonId] INT NOT NULL,
    [SevisId] VARCHAR(15) NULL, 
    CONSTRAINT [PK_dbo.PersonFamily] PRIMARY KEY CLUSTERED ([PersonId] ASC, [RelatedPersonId] ASC),
    CONSTRAINT [FK_dbo.PersonFamily_dbo.Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.PersonFamily_dbo.Person_RelatedPersonId] FOREIGN KEY ([RelatedPersonId]) REFERENCES [dbo].[Person] ([PersonId])
);


GO
CREATE NONCLUSTERED INDEX [IX_PersonId]
    ON [dbo].[PersonFamily]([PersonId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RelatedPersonId]
    ON [dbo].[PersonFamily]([RelatedPersonId] ASC);

