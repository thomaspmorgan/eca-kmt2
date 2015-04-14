CREATE TABLE [CAM].[AccountStatus]
(
	[AccountStatusId] INT NOT NULL PRIMARY KEY IDENTITY (1,1), 
    [Status] NVARCHAR(50) NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [CreatedOn] DATETIMEOFFSET NOT NULL, 
    [RevisedBy] INT NOT NULL, 
    [RevisedOn] DATETIMEOFFSET NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1
)
