CREATE TABLE [dbo].[DependentType] (
    [DependentTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (150) NOT NULL,
    [SevisDependentTypeCode] NVARCHAR(2) NULL, 
    CONSTRAINT [PK_dbo.DependentType] PRIMARY KEY CLUSTERED ([DependentTypeId] ASC)
);
