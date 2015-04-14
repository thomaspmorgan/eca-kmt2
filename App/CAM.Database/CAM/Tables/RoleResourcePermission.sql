CREATE TABLE [CAM].[RoleResourcePermission]
(
	[RoleId] INT NOT NULL , 
    [ResourceId] INT NOT NULL, 
    [PermissionId] INT NOT NULL, 
    [AssignedOn] INT NOT NULL, 
    [AssignedBy] INT NOT NULL, 
    PRIMARY KEY ([RoleId], [ResourceId], [PermissionId]), 
    CONSTRAINT [FK_RoleResourcePermission_ToRole] FOREIGN KEY ([RoleId]) REFERENCES [CAM].[Role]([RoleId]), 
    CONSTRAINT [FK_RoleResourcePermission_ToResource] FOREIGN KEY ([ResourceId]) REFERENCES [CAM].[Resource]([ResourceId]), 
    CONSTRAINT [FK_RoleResourcePermission_ToPermission] FOREIGN KEY ([PermissionId]) REFERENCES [CAM].[Permission]([PermissionId]),
)
