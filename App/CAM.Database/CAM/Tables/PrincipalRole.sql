CREATE TABLE [CAM].[PrincipalRole]
(
	[PrincipalId] INT NOT NULL , 
    [RoleId] INT NOT NULL, 
    [AssignedBy] INT NOT NULL, 
    [AssignedOn] DATETIMEOFFSET NULL, 
    PRIMARY KEY ([PrincipalId], [RoleId]), 
    CONSTRAINT [FK_PrincipalRole_ToPrincipal] FOREIGN KEY ([PrincipalId]) REFERENCES [CAM].[Principal]([PrincipalId]), 
    CONSTRAINT [FK_PrincipalRole_ToRole] FOREIGN KEY ([RoleId]) REFERENCES [CAM].[Role]([RoleId])
)
