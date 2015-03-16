CREATE TABLE [dbo].[EmailAddressType]
(
	[EmailAddressTypeId] INT IDENTITY(1,1) NOT NULL ,
	[EmailAddressTypeName] [nvarchar](128) NOT NULL,
	[History_CreatedBy] [int] NOT NULL,
	[History_CreatedOn] [datetimeoffset](7) NOT NULL,
	[History_RevisedBy] [int] NOT NULL,
	[History_RevisedOn] [datetimeoffset](7) NOT NULL, 
    CONSTRAINT [PK_EmailAddressType] PRIMARY KEY ([EmailAddressTypeId]) 
)
