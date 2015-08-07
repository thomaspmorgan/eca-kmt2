CREATE TABLE [dbo].[ProminentCategory] (
    [ProminentCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Person_PersonId] [int] NULL,
    CONSTRAINT [PK_dbo.ProminentCategoryId] PRIMARY KEY CLUSTERED ([ProminentCategoryId] ASC), 
    CONSTRAINT [FK_dbo.ProminentCategory_dbo.Person_PersonId] FOREIGN KEY ([Person_PersonID]) REFERENCES [dbo].[Person]([PersonID])
);

