USE [ECA_Dev]
GO

/****** Object:  Table [dbo].[LocationType]    Script Date: 1/12/2015 10:29:28 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LocationType](
	[LocationTypeId] [int] IDENTITY(1,1) NOT NULL,
	[LocationName] [nvarchar](max) NOT NULL,
	[History_CreatedBy] [int] NOT NULL,
	[History_CreatedOn] [datetimeoffset](7) NOT NULL,
	[History_RevisedBy] [int] NOT NULL,
	[History_RevisedOn] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_dbo.LocationType] PRIMARY KEY CLUSTERED 
(
	[LocationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

/* load the data */
begin tran t1
insert into locationtype
values 
 ('Region',0,getdate(),0,getdate()),
 ('Country',0,getdate(),0,getdate()),
 ('State',0,getdate(),0,getdate()),
 ('City',0,getdate(),0,getdate()),
 ('Building',0,getdate(),0,getdate())

Commit tran t1
GO



