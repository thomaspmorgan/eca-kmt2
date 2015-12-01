/****** Object:  Table [dbo].[Local_IVLP_Project_Mapping_XREF_Interim]    Script Date: 10/21/2015 11:35:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ECA_Local_IVLP_KMT_Project_XREF_Interim](
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
	[IVLP_KMT_MappedProgramName] [nvarchar](255) NULL,
	[KMT_ProgramId] [int] NULL,
	[KMT_Name] [nvarchar](255) NULL,
	[KMT_ProgramStatusId] [int] NULL,
	[KMT_Description] [nvarchar](3000) NULL,
	[KMT_StartDate] [datetimeoffset](7) NULL,
	[KMT_EndDate] [datetimeoffset](7) NULL,
	[KMT_History_CreatedBy] [int] NULL,
	[KMT_History_CreatedOn] [datetimeoffset](7) NULL,
	[KMT_History_RevisedBy] [int] NULL,
	[KMT_History_RevisedOn] [datetimeoffset](7) NULL,
	[KMT_ParentProgram_ProgramId] [int] NULL,
	[KMT_Owner_OrganizationId] [int] NULL,
	[ProjectStatus_KMT] [nvarchar](20) NULL,
	[ProjectType_KMT] [nvarchar](50) NULL,
	[KMT_MappedProjectLanguage] [nvarchar](100) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


