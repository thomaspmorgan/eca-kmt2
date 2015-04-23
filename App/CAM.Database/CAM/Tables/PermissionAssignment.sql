CREATE TABLE [CAM].[PermissionAssignment]
(
	[PrincipalId] INT NOT NULL , 
    [ResourceId] INT NOT NULL, 
    [PermissionId] INT NOT NULL, 
    [AssignedOn] DATETIMEOFFSET NOT NULL, 
    [AssignedBy] INT NOT NULL, 
    [IsAllowed] BIT NOT NULL DEFAULT 1, 
    PRIMARY KEY ([PrincipalId], [ResourceId], [PermissionId]), 
    CONSTRAINT [FK_PermissionAssignment_ToPrincipal] FOREIGN KEY ([PrincipalId]) REFERENCES [CAM].[Principal]([PrincipalId]), 
    CONSTRAINT [FK_PermissionAssignment_ToResource] FOREIGN KEY ([ResourceId]) REFERENCES [CAM].[Resource]([ResourceId]), 
    CONSTRAINT [FK_PermissionAssignment_ToPermission] FOREIGN KEY ([PermissionId]) REFERENCES [CAM].[Permission]([PermissionId])
)
