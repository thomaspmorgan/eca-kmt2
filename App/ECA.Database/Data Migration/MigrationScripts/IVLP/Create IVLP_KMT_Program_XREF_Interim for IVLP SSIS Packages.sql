/****** Object:  Table [dbo].[ECA_Local_IVLP_KMT_Program_XREF_Interim]    Script Date: 10/21/2015 10:37:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ECA_Local_IVLP_KMT_Program_XREF_Interim](
	[IVLP_PROJECT_ID] [nvarchar](32) NULL,
	[IVLP_PROGRAM] [nvarchar](100) NULL,
	[IVLP_PROJECT_NUMBER] [nvarchar](15) NULL,
	[IVLP_PROJECT_TITLE] [nvarchar](140) NULL,
	[IVLP_PROJECT_TYPE] [nvarchar](50) NULL,
	[IVLP_FISCAL_YEAR] [numeric](4, 0) NULL,
	[IVLP_START_DATE] [datetime] NULL,
	[IVLP_END_DATE] [datetime] NULL,
	[IVLP_EST_PARTICIPANT_CNT] [numeric](6, 0) NULL,
	[IVLP_EST_ARRIVAL_DATE] [datetime] NULL,
	[IVLP_EST_DEPARTURE_DATE] [datetime] NULL,
	[IVLP_PROJECT_STATUS] [nvarchar](50) NULL,
	[IVLP_PROJECT_LANGUAGE] [nvarchar](100) NULL,
	[IVLP_FUNDING_ID] [nvarchar](32) NULL,
	[IVLP_FUNDING_SOURCE] [nvarchar](100) NULL,
	[IVLP_FUNDING_COUNTRY] [nvarchar](100) NULL,
	[IVLP_NOMINATED_BY_ORG_ID] [nvarchar](32) NULL,
	[IVLP_COUNTRY] [nvarchar](60) NULL,
	[IVLP_REGION] [nvarchar](100) NULL,
	[IVLP_NPA_ORG_ID] [nvarchar](32) NULL,
	[IVLP_NPA_PGM_OFFICER_PERSON_ID] [nvarchar](32) NULL,
	[IVLP_NPA_PGM_ASSOCIATE_PERSON_ID] [nvarchar](32) NULL,
	[IVLP_DOS_PGM_OFFICE_ORG_ID] [nvarchar](32) NULL,
	[IVLP_DOS_PGM_OFFICER_PERSON_ID] [nvarchar](32) NULL,
	[IVLP_DOS_PGM_COORDINATOR_PERSON_ID] [nvarchar](32) NULL,
	[IVLP_PROJECT_CREATE_ID] [nvarchar](32) NULL,
	[IVLP_PROJECT_CREATE_DT] [datetime] NULL,
	[IVLP_PROJECT_UPDATE_ID] [nvarchar](32) NULL,
	[IVLP_PROJECT_UPDATE_DT] [datetime] NULL,
	[IVLP_KMT_MappedProgramName] [nvarchar](255) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


