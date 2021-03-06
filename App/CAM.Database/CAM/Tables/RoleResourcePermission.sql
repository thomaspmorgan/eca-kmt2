﻿CREATE TABLE [cam].[RoleResourcePermission]
(
	[RoleId] INT NOT NULL , 
    [ResourceId] INT NOT NULL, 
    [PermissionId] INT NOT NULL, 
    [AssignedOn] DATETIMEOFFSET NOT NULL, 
    [AssignedBy] INT NOT NULL, 
    PRIMARY KEY ([RoleId], [ResourceId], [PermissionId]), 
    CONSTRAINT [FK_RoleResourcePermission_ToRole] FOREIGN KEY ([RoleId]) REFERENCES [cam].[Role]([RoleId]), 
    CONSTRAINT [FK_RoleResourcePermission_ToResource] FOREIGN KEY ([ResourceId]) REFERENCES [cam].[Resource]([ResourceId]), 
    CONSTRAINT [FK_RoleResourcePermission_ToPermission] FOREIGN KEY ([PermissionId]) REFERENCES [cam].[Permission]([PermissionId]),
)
