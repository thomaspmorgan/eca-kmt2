CREATE TABLE [dbo].[NamePart] (
    [NamePartId] INT            IDENTITY (1, 1) NOT NULL,
    [Value]      NVARCHAR (MAX) NOT NULL,
    [NameTypeId] INT            NOT NULL,
    [PersonId]   INT            NOT NULL,
    CONSTRAINT [PK_dbo.NamePart] PRIMARY KEY CLUSTERED ([NamePartId] ASC),
    CONSTRAINT [FK_dbo.NamePart_dbo.Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_PersonId]
    ON [dbo].[NamePart]([PersonId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NameTypeId]
    ON [dbo].[NamePart]([NameTypeId] ASC);

