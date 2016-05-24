--USE [ECA_KMT_PRE]
--GO

INSERT INTO [dbo].[Project]
           ([Name]
           ,[Description]
           ,[ProjectTypeId]
           ,[FocusArea]
           ,[StartDate]
           ,[EndDate]
           ,[Language]
           ,[AudienceReach]
           ,[ActivityId]
           ,[History_CreatedBy]
           ,[History_CreatedOn]
           ,[History_RevisedBy]
           ,[History_RevisedOn]
           ,[NominationSource_OrganizationId]
           ,[ProgramId]
           ,[ProjectStatusId]
           ,[ProjectNumberIVLP]
           ,[VisitorTypeId]
           ,[UsParticipantsEst]
           ,[NonUsParticipantsEst]
           ,[UsParticipantsActual]
           ,[NonUsParticipantsActual]
)
     VALUES
           ('Local, State and Federal Public Policymaking for Pakistani Student Leaders FY 2014'
           ,'Local, State and Federal Public Policymaking for Pakistani Student Leaders FY 2014'
           ,1
           ,NULL
           ,N'10/1/2013 12:00:00 AM -05:00'
           ,N'9/30/2018 12:00:00 AM -05:00'
           ,NULL
           ,0
           ,NULL
           ,1
           ,GETDATE()
           ,1
           ,GETDATE()
           ,NULL
           ,1213   /* Local,dev, */
--         ,1256  /* QA */
--         ,1118  /* Pre,Prod */
           ,1
           ,NULL
           ,2
           ,0
           ,0
           ,0
           ,0)


GO


