CREATE TABLE [cam].[Principal]
(
	[PrincipalId] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [PrincipalTypeId] INT NULL, 
    CONSTRAINT [FK_Principal_ToPrincipal] FOREIGN KEY ([PrincipalTypeId]) REFERENCES [cam].[PrincipalType]([PrincipalTypeId])
)
