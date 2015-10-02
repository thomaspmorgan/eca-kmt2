/* This script updates the Owner_OrganizationID and the ParentProgram_ProgramID on existing Programs */
USE IVLP
GO

DECLARE S_WEB_ADDRESS] nvarchar](200) NULL,
DECLARE	S_PARENT_ORG_ID] [nvarchar](32) NULL,
DECLARE	[S_ORG_CREATE_ID] [nvarchar](32) NULL,
DECLARE	[S_ORG_CREATE_DT] [datetime] NULL,
DECLARE	[S_ORG_UPDATE_ID] [nvarchar](32) NULL,
DECLARE	[S_ORG_UPDATE_DT] [datetime] NULL,
DECLARE	[S_ORG_ID] [nvarchar](32) NULL,
DECLARE	[S_ORG_NAME] [nvarchar](240) NULL,
DECLARE	[ECA_OrganizationTypeName] [nvarchar](5) NULL,
DECLARE	[ECA_History_CreatedBy] [int] NULL,
DECLARE	[ECA_History_RevisedBy] [int] NULL,
DECLARE	[ECA_OrganizationStatus] [nvarchar](6) NULL,
DECLARE	[ECA_OrganizationId] [int] NULL,
DECLARE	[ECA_History_CreatedOn] [datetime] NULL,
DECLARE	[ECA_History_RevisedOn] [datetime] NULL,
DECLARE	[ECA_ParentOrganization_OrganizationId] [int] NULL,
DECLARE	[ECA_OfficeSymbol] [nvarchar](128) NULL,
DECLARE	[ECA_Description] [nvarchar](3000) NULL,
DECLARE	[ECA_Name] [nvarchar](600) NULL,
DECLARE	[ECA_Website] [nvarchar](2000) NULL,
DECLARE	[ECA_OrganizationTypeId] [int]



Declare @cursorOrganization CURSOR

/* Define the cursor */
SET @cursorOrganization = CURSOR 
FOR
SELECT [S_WEB_ADDRESS]
      ,[S_PARENT_ORG_ID]
      ,[S_ORG_CREATE_ID]
      ,[S_ORG_CREATE_DT]
      ,[S_ORG_UPDATE_ID]
      ,[S_ORG_UPDATE_DT]
      ,[S_ORG_ID]
      ,[S_ORG_NAME]
      ,[ECA_OrganizationTypeName]
      ,[ECA_History_CreatedBy]
      ,[ECA_History_RevisedBy]
      ,[ECA_OrganizationStatus]
      ,[ECA_OrganizationId]
      ,[ECA_History_CreatedOn]
      ,[ECA_History_RevisedOn]
      ,[ECA_ParentOrganization_OrganizationId]
      ,[ECA_OfficeSymbol]
      ,[ECA_Description]
      ,[ECA_Name]
      ,[ECA_Website]
      ,[ECA_OrganizationTypeId]
  FROM [IVLP].[dbo].[Local_IVLP_Organization_XREF] x

/* Open the cursor */
OPEN @cursorOrganization

/* Fetch the first organization XREF row */
FETCH NEXT 
FROM @cursorOrganization 
INTO @ProgramId,@Owner_OrganizationId,@ParentProgram_ProgramId,
                @ProgramName,@OfficeName,@OfficeSymbol,
                @OrganizationId,@OrganizationName,
                @ParentProgramId,@ParentProgramName

/* Loop thru all programs (staging) - should match DB */
WHILE @@FETCH_STATUS = 0
BEGIN

/* Check if Organization already exists in the organization table */
/* If yes, then retrieve the OrganizationId and Update the XREF table */
/* If No, the insert the new organization into the organization table */


/* Fetch the first organization XREF row */
FETCH NEXT 
FROM @cursorOrganization 
INTO @ProgramId,@Owner_OrganizationId,@ParentProgram_ProgramId,
                @ProgramName,@OfficeName,@OfficeSymbol,
                @OrganizationId,@OrganizationName,
                @ParentProgramId,@ParentProgramName

END

/* Cleanup */
CLOSE @cursorOrganization
DEALLOCATE @cursorOrganization
GO





