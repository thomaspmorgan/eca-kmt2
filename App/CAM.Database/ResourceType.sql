CREATE TABLE [cam].[ResourceType]
(
	[ResourceTypeId] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	[ResourceTypeName] NCHAR(10) NOT NULL, 
	[ResourceTypeDescription] NVARCHAR(255) NULL, 
	[History_CreatedOn] DATETIMEOFFSET NOT NULL, 
	[History_CreatedBy] INT NOT NULL, 
	[History_RevisedOn] DATETIMEOFFSET NOT NULL, 
	[History_RevisedBy] INT NOT NULL, 
	[IsActive] BIT NOT NULL
)
