/****** Object:  Table [dbo].[Local_IVLP_Project_Mapping_XREF_Final]    Script Date: 10/21/2015 11:37:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ECA_Local_IVLP_KMT_Project_XREF_Final](
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
	[IVLP_NOMINATED_BY_ORG_ID] [varchar](32) NULL,
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
	[IVLP_PROJECT_STATUS_Unicode] [nvarchar](20) NULL,
	[IVLP_PROJECT_TYPE_Unicode] [nvarchar](100) NULL,
	[IVLP_PROJECT_NUMBER_Unicode] [nvarchar](100) NULL,
	[KMT_Program_ProgramId] [int] NULL,
	[KMT_Program_Name] [nvarchar](255) NULL,
	[KMT_Program_ProgramStatusId] [int] NULL,
	[KMT_Program_Description] [nvarchar](3000) NULL,
	[KMT_Program_StartDate] [datetimeoffset](7) NULL,
	[KMT_Program_EndDate] [datetimeoffset](7) NULL,
	[KMT_Program_History_CreatedBy] [int] NULL,
	[KMT_Program_History_CreatedOn] [datetimeoffset](7) NULL,
	[KMT_Program_History_RevisedBy] [int] NULL,
	[KMT_Program_History_RevisedOn] [datetimeoffset](7) NULL,
	[KMT_Program_ParentProgram_ProgramId] [int] NULL,
	[KMT_Program_Owner_OrganizationId] [int] NULL,
	[KMT_ProjectStatus] [nvarchar](20) NULL,
	[KMT_ProjectType] [nvarchar](100) NULL,
	[KMT_MappedProjectLanguage] [nvarchar](100) NULL,
	[KMT_Project_ProjectId] [int] NULL,
	[KMT_Project_Name] [nvarchar](500) NULL,
	[KMT_Project_Description] [nvarchar](3000) NULL,
	[KMT_Project_ProjectTypeId] [int] NULL,
	[KMT_Project_FocusArea] [nvarchar](100) NULL,
	[KMT_Project_StartDate] [datetimeoffset](7) NULL,
	[KMT_Project_EndDate] [datetimeoffset](7) NULL,
	[KMT_Project_Language] [nvarchar](100) NULL,
	[KMT_Project_AudienceReach] [int] NULL,
	[KMT_Project_ActivityId] [int] NULL,
	[KMT_Project_History_CreatedBy] [int] NULL,
	[KMT_Project_History_CreatedOn] [datetimeoffset](7) NULL,
	[KMT_Project_History_RevisedBy] [int] NULL,
	[KMT_Project_History_RevisedOn] [datetimeoffset](7) NULL,
	[KMT_Project_NominationSource_OrganizationId] [int] NULL,
	[KMT_Project_ProgramId] [int] NULL,
	[KMT_Project_ProjectStatusId] [int] NULL,
	[KMT_Project_RowVersion] [binary](8) NULL,
	[KMT_Project_ProjectNumberIVLP] [nvarchar](100) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


