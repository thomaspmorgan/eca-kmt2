CREATE TABLE [CAM].[SevisAccount]
(
	[Id] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY, 
    [PrincipalId] INT NOT NULL, 
    [OrgId] NVARCHAR(15) NULL, 
    [Username] NVARCHAR(10) NULL

	CONSTRAINT [FK_cam.SevisAccount_cam.Principal_PrincipalId] FOREIGN KEY ([PrincipalId]) REFERENCES [cam].[Principal]([PrincipalId])
)
