CREATE TABLE [cam].[PrincipalRole]
(
	[PrincipalId] INT NOT NULL , 
    [RoleId] INT NOT NULL, 
    [AssignedBy] INT NOT NULL, 
    [AssignedOn] DATETIMEOFFSET NOT NULL , 
    PRIMARY KEY ([PrincipalId], [RoleId]), 
    CONSTRAINT [FK_PrincipalRole_ToPrincipal] FOREIGN KEY ([PrincipalId]) REFERENCES [cam].[Principal]([PrincipalId]), 
    CONSTRAINT [FK_PrincipalRole_ToRole] FOREIGN KEY ([RoleId]) REFERENCES [cam].[Role]([RoleId])
)
