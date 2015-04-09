CREATE TABLE [cam].[PermissionAssignment]
(
	[PrincipalId] INT NOT NULL , 
    [ResourceId] INT NOT NULL, 
    [PermissionId] INT NOT NULL, 
    [AssignedOn] DATETIMEOFFSET NOT NULL, 
    [AssignedBy] INT NOT NULL, 
    PRIMARY KEY ([PrincipalId], [ResourceId], [PermissionId]), 
    CONSTRAINT [FK_PermissionAssignment_ToPrincipal] FOREIGN KEY ([PrincipalId]) REFERENCES [cam].[Principal]([PrincipalId]), 
    CONSTRAINT [FK_PermissionAssignment_ToResource] FOREIGN KEY ([ResourceId]) REFERENCES [cam].[Resource]([ResourceId]), 
    CONSTRAINT [FK_PermissionAssignment_ToPermission] FOREIGN KEY ([PermissionId]) REFERENCES [cam].[Permission]([PermissionId])
)
