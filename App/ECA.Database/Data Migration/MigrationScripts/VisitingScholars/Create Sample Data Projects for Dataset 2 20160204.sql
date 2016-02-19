USE [ECA_KMT_DEV]
GO

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
           ,[VisitorTypeId])
     VALUES
           ('Fulbright Specialist FY 2012'
           ,'Fulbright Specialist Project FY 2012'
           ,1
           ,NULL
           ,N'10/1/2011 12:00:00 AM -00:00'
           ,N'9/30/2012 12:00:00 AM -00:00'
           ,NULL
           ,0
           ,NULL
           ,1
           ,GETDATE()
           ,1
           ,GETDATE()
           ,NULL
           ,1187
           ,1
           ,NULL
           ,1),
           ('Fulbright Specialist FY 2010'
           ,'Fulbright Specialist Project FY 2010'
           ,1
           ,NULL
           ,N'10/1/2009 12:00:00 AM -00:00'
           ,N'9/30/2010 12:00:00 AM -00:00'
           ,NULL
           ,0
           ,NULL
           ,1
           ,GETDATE()
           ,1
           ,GETDATE()
           ,NULL
           ,1187
           ,1
           ,NULL
           ,1),
('Fulbright Lecturer FY 2014'
           ,'Fulbright Lecturer Project FY 2014'
           ,1
           ,NULL
           ,N'10/1/2013 12:00:00 AM -00:00'
           ,N'9/30/2014 12:00:00 AM -00:00'
           ,NULL
           ,0
           ,NULL
           ,1
           ,GETDATE()
           ,1
           ,GETDATE()
           ,NULL
           ,1174
           ,1
           ,NULL
           ,1),
('Fulbright Lecturer/Researcher FY 2014'
           ,'Fulbright Lecturer/Researcher Project FY 2014'
           ,1
           ,NULL
           ,N'10/1/2013 12:00:00 AM -00:00'
           ,N'9/30/2014 12:00:00 AM -00:00'
           ,NULL
           ,0
           ,NULL
           ,1
           ,GETDATE()
           ,1
           ,GETDATE()
           ,NULL
           ,1175
           ,1
           ,NULL
           ,1),
('Fulbright Scholar Researcher FY 2014'
           ,'Fulbright Scholar Researcher Project FY 2014'
           ,1
           ,NULL
           ,N'10/1/2013 12:00:00 AM -00:00'
           ,N'9/30/2014 12:00:00 AM -00:00'
           ,NULL
           ,0
           ,NULL
           ,1
           ,GETDATE()
           ,1
           ,GETDATE()
           ,NULL
           ,1177
           ,1
           ,NULL
           ,1)


GO


