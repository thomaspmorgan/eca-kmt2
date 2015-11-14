CREATE TABLE [cam].[UserAccount]
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
    CONSTRAINT [FK_UserAccount_ToPrincipal] FOREIGN KEY ([PrincipalId]) REFERENCES [cam].[Principal]([PrincipalId]), 
    CONSTRAINT [FK_UserAccount_ToAccountStatus] FOREIGN KEY ([AccountStatusId]) REFERENCES [cam].[AccountStatus]([AccountStatusId]), 
    CONSTRAINT [CK_UserAccount_UniqueAdGuid] UNIQUE([AdGuid])
)

GO

CREATE INDEX [IX_UserAccount_AdGuid] ON [CAM].[UserAccount] ([AdGuid])

GO

CREATE INDEX [IX_UserAccount_DisplayName] ON [cam].[UserAccount] ([DisplayName])

GO

CREATE INDEX [IX_UserAccount_FirstName] ON [cam].[UserAccount] ([FirstName])

GO

CREATE INDEX [IX_UserAccount_LastName] ON [cam].[UserAccount] ([LastName])

GO
