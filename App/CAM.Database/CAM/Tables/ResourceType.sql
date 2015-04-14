CREATE TABLE [CAM].[ResourceType]
(
	[ResourceTypeId] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	[ResourceTypeName] NVARCHAR(50) NOT NULL, 
	[ResourceTypeDescription] NVARCHAR(255) NULL, 
	[CreatedOn] DATETIMEOFFSET NOT NULL, 
	[CreatedBy] INT NOT NULL, 
	[RevisedOn] DATETIMEOFFSET NOT NULL, 
	[RevisedBy] INT NOT NULL, 
	[IsActive] BIT NOT NULL DEFAULT 1
)
