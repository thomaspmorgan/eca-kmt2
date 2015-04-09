CREATE TABLE [cam].[Resource]
(
	[ResourceId] INT NOT NULL PRIMARY KEY Identity(1,1), 
    [ResourceTypeId] INT NOT NULL, 
    [ForeignResourceId] INT NOT NULL, 
    CONSTRAINT [FK_Resource_ToResourceType] FOREIGN KEY ([ResourceTypeId]) REFERENCES [cam].[ResourceType]([ResourceTypeId])
)
