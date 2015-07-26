CREATE TABLE [dbo].[ProminentCategory] (
    [ProminentCategoryId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]                NVARCHAR (MAX) NOT NULL,
    [Person_PersonId]     INT            NULL,
    CONSTRAINT [PK_dbo.ProminentCategory] PRIMARY KEY CLUSTERED ([ProminentCategoryId] ASC),
    CONSTRAINT [FK_dbo.ProminentCategory_dbo.Person_Person_PersonId] FOREIGN KEY ([Person_PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Person_PersonId]
    ON [dbo].[ProminentCategory]([Person_PersonId] ASC);

