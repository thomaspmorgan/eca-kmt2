CREATE TABLE [dbo].[PersonType] (
    [PersonTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (150) NOT NULL,
    [SevisDependentTypeCode] NVARCHAR(2) NULL, 
    [IsDependentPersonType] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_dbo.PersonType] PRIMARY KEY CLUSTERED ([PersonTypeId] ASC)
);
