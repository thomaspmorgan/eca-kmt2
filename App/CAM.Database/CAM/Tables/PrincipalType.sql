CREATE TABLE [CAM].[PrincipalType]
(
	[PrincipalTypeId] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [PrincipalTypeName] NCHAR(10) NOT NULL, 
    [PrincipalTypeDescription] NVARCHAR(255) NULL, 
    [CreatedBy] INT NOT NULL, 
    [CreatedOn] DATETIMEOFFSET NOT NULL, 
    [RevisedBy] INT NOT NULL, 
    [RevisedOn] DATETIMEOFFSET NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1
)
