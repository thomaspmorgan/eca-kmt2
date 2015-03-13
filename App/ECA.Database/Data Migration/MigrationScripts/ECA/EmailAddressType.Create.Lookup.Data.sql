
USE [ECA_Dev]
GO

/****** Object:  Table [dbo].[EmailAddressType]    Script Date: 3/12/2015 8:55:19 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EmailAddressType](
	[EmailAddressTypeId] [int] IDENTITY(1,1) NOT NULL,
	[EmailAddressTypeName] [nvarchar](128) NOT NULL,
	[History_CreatedBy] [int] NOT NULL,
	[History_CreatedOn] [datetimeoffset](7) NOT NULL,
	[History_RevisedBy] [int] NOT NULL,
	[History_RevisedOn] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_dbo.EmailAddressType] PRIMARY KEY CLUSTERED 
(
	[EmailAddressTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

/* add the new column to the emailaddress table */
ALTER TABLE EmailAddress
ADD EmailAddressTypeId int NOT NULL
GO

/* Add the Foreign key relationship from EmailAddress to EmailAddressType */
ALTER TABLE [dbo].[EmailAddress]  WITH CHECK ADD  CONSTRAINT [FK_dbo.EmailAddress_dbo.EmailAddressType_EmailAddressTypeId] FOREIGN KEY([EmailAddressTypeId])
REFERENCES [dbo].[EmailAddressType] ([EmailAddressTypeId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[EmailAddress] CHECK CONSTRAINT [FK_dbo.EmailAddress_dbo.EmailAddressType_EmailAddressTypeId]
GO