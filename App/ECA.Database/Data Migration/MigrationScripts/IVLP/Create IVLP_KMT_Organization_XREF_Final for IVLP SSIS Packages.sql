USE [IVLP_XREF]
GO

/****** Object:  Table [dbo].[Local_IVLP_Organization_XREF]    Script Date: 10/21/2015 5:17:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ECA_Local_IVLP_KMT_Organization_XREF](
	[IVLP_ORG_ID] [nvarchar](32) NULL,
	[IVLP_ORG_NAME] [nvarchar](240) NULL,
    [IVLP_WEB_ADDRESS] [nvarchar](200) NULL,
	[IVLP_PARENT_ORG_ID] [nvarchar](32) NULL,
	[IVLP_ORG_CREATE_ID] [nvarchar](32) NULL,
	[IVLP_ORG_CREATE_DT] [datetime] NULL,
	[IVLP_ORG_UPDATE_ID] [nvarchar](32) NULL,
	[IVLP_ORG_UPDATE_DT] [datetime] NULL,
	[KMT_OrganizationId] [int] NULL,
	[KMT_OrganizationTypeId] [int] NULL,
	[KMT_OfficeSymbol] [nvarchar](128) NULL,
	[KMT_Description] [nvarchar](3000) NULL,
	[KMT_Status] [nvarchar](20) NULL,
	[KMT_Name] [nvarchar](600) NULL,
	[KMT_Website] [nvarchar](2000) NULL,
	[KMT_History_CreatedBy] [int] NULL,
	[KMT_History_CreatedOn] [datetimeoffset](7) NULL,
	[KMT_History_RevisedBy] [int] NULL,
	[KMT_History_RevisedOn] [datetimeoffset](7) NULL,
	[KMT_ParentOrganization_OrganizationId] [int] NULL
) ON [PRIMARY]

GO



