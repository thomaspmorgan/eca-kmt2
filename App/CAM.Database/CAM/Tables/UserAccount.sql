CREATE TABLE [CAM].[UserAccount]
(
	[PrincipalId] INT NOT NULL PRIMARY KEY, 
    [AdGuid] UNIQUEIDENTIFIER NOT NULL, 
    [LastAccessed] DATETIMEOFFSET NULL, 
    [CreatedOn] DATETIMEOFFSET NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [RevisedOn] DATETIMEOFFSET NOT NULL, 
    [RevisedBy] INT NOT NULL, 
    [AccountStatusId] INT NOT NULL, 
    [PermissionsRevisedOn] DATETIMEOFFSET NULL, 
    [Note] NVARCHAR(2000) NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [DisplayName] NVARCHAR(101) NOT NULL, 
    [EmailAddress] NVARCHAR(50) NULL, 
    [ExpiredDate] DATETIMEOFFSET NULL, 
    [SuspendedDate] DATETIMEOFFSET NULL, 
    [RevokedDate] DATETIMEOFFSET NULL, 
    [RestoredDate] DATETIMEOFFSET NULL, 
    CONSTRAINT [FK_UserAccount_ToPrincipal] FOREIGN KEY ([PrincipalId]) REFERENCES [CAM].[Principal]([PrincipalId]), 
    CONSTRAINT [FK_UserAccount_ToAccountStatus] FOREIGN KEY ([AccountStatusId]) REFERENCES [CAM].[AccountStatus]([AccountStatusId])
)
