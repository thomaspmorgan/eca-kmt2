CREATE TABLE [dbo].[PersonType] (
    [PersonTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_dbo.PersonType] PRIMARY KEY CLUSTERED ([PersonTypeId] ASC)
);
