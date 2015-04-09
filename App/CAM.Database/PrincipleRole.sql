CREATE TABLE [cam].[PrincipleRole]
(
	[PrincipalId] INT NOT NULL , 
    [RoleId] INT NOT NULL, 
    [AssignedBy] INT NOT NULL, 
    [AssignedOn] DATETIMEOFFSET NULL, 
    PRIMARY KEY ([PrincipalId], [RoleId]), 
    CONSTRAINT [FK_PrincipleRole_ToPrincipal] FOREIGN KEY ([PrincipalId]) REFERENCES [cam].[Principal]([PrincipalId]), 
    CONSTRAINT [FK_PrincipleRole_ToRole] FOREIGN KEY ([RoleId]) REFERENCES [cam].[Role]([RoleId])
)
