CREATE TABLE [cam].[UserAccount]
(
	[PrincipalId] INT NOT NULL PRIMARY KEY, 
    [AdGuid] UNIQUEIDENTIFIER NULL, 
    [LastAccessed] DATETIMEOFFSET NULL, 
    [History_CreatedOn] DATETIMEOFFSET NULL, 
    [History_CreatedBy] INT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NULL, 
    [History_RevisedBy] INT NULL, 
    [AccountStatusId] INT NULL, 
    [PermissionsRevisedOn] DATETIMEOFFSET NULL, 
    [Note] NVARCHAR(2000) NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [FirstName] NVARCHAR(50) NULL, 
    [DisplayName] NVARCHAR(101) NULL, 
    [EmalAddress] NVARCHAR(50) NULL, 
    CONSTRAINT [FK_UserAccount_ToPrincipal] FOREIGN KEY ([PrincipalId]) REFERENCES [cam].[Principal]([PrincipalId])
)
