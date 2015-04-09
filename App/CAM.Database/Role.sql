CREATE TABLE [cam].[Role]
(
	[RoleId] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [RoleName] NVARCHAR(50) NOT NULL, 
    [RoleDescription] NCHAR(255) NULL, 
    [CreatedOn] DATETIMEOFFSET NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [RevisedOn] DATETIMEOFFSET NOT NULL, 
    [RevisedBy] INT NOT NULL, 
    [IsActive] BIT NOT NULL, 
    [ResourceId] INT NULL, 
    [ResourceTypeId] INT NULL, 
    CONSTRAINT [FK_Role_ToResource] FOREIGN KEY ([ResourceId]) REFERENCES [cam].[Resource]([ResourceId]), 
    CONSTRAINT [FK_Role_ToResourceType] FOREIGN KEY ([ResourceTypeId]) REFERENCES [cam].[ResourceType]([ResourceTypeId]) 
)
