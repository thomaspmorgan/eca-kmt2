CREATE TABLE [CAM].[Application]
(
	[ResourceId] INT NOT NULL PRIMARY KEY, 
    [ApplicationName] NVARCHAR(50) NULL, 
    [CreatedOn] DATETIMEOFFSET NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [RevisedOn] DATETIMEOFFSET NOT NULL, 
    [RevisedBy] INT NOT NULL, 
    CONSTRAINT [FK_Application_ToResource] FOREIGN KEY ([ResourceId]) REFERENCES [CAM].[Resource]([ResourceId])
)
