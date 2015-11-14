CREATE TABLE [cam].[Permission]
(
	[PermissionId] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [PermissionName] NVARCHAR(50) NOT NULL, 
    [CreatedOn] DATETIMEOFFSET NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [RevisedOn] DATETIMEOFFSET NOT NULL, 
    [RevisedBy] NVARCHAR(50) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [ResourceTypeId] INT NULL, 
	[ParentResourceTypeId] INT NULL, 
    [ResourceId] INT NULL, 
    [PermissionDescription] NCHAR(255) NULL, 
    CONSTRAINT [FK_Permission_ToResource] FOREIGN KEY (ResourceId) REFERENCES [cam].[Resource]([ResourceId]), 
	CONSTRAINT [FK_Permission_ToParentResourceType] FOREIGN KEY (ParentResourceTypeId) REFERENCES [cam].[ResourceType]([ResourceTypeId]),
    CONSTRAINT [FK_Permission_ToResourceType] FOREIGN KEY (ResourceTypeId) REFERENCES [cam].[ResourceType]([ResourceTypeId])
)

GO

CREATE INDEX [IX_Permission_PermissionName] ON [cam].[Permission] ([PermissionName])

GO

CREATE INDEX [IX_Permission_PermissionDescription] ON [cam].[Permission] ([PermissionDescription])

GO
