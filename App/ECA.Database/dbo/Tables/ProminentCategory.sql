CREATE TABLE [dbo].[ProminentCategory] (
    [ProminentCategoryId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]                NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_dbo.ProminentCategory] PRIMARY KEY CLUSTERED ([ProminentCategoryId] ASC),
    CONSTRAINT [FK_dbo.ProminentCategory_dbo.Person_Person_PersonId] FOREIGN KEY ([Person_PersonId]) REFERENCES [dbo].[Person] ([PersonId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Person_PersonId]
    ON [dbo].[ProminentCategory]([Person_PersonId] ASC);

