CREATE TABLE [cam].[PrincipalType]
(
	[PrincipalTypeId] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [PrincipalTypeName] NCHAR(10) NOT NULL, 
    [PrincipalTypeDescription] NVARCHAR(255) NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    [IsActive] BIT NOT NULL
)
