USE [ECA_Dev]
GO

SET IDENTITY_INSERT [dbo].[Contact] ON
GO

DECLARE @LastInsertedContactId   int

/* See what the last added contactid is */
SELECT @LastInsertedContactId = max(contactid) FROM dbo.contact

/* LOad contact names */
INSERT INTO [dbo].[Contact]
           ([Contactid]
           ,[FullName]
           ,[Position]
           ,[History_CreatedBy]
           ,[History_CreatedOn]
           ,[History_RevisedBy]
           ,[History_RevisedOn])
     VALUES
           (@LastInsertedContactId + 1,N'Alma R. Candelaria',N'Director', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 2,N'Carol L. Grabauskas',N'Deputy Director (VolVis and CR)', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 3,N'Melissa E. Clegg-Tripp',N'Deputy Director (Regional Programs)', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 4,N'Sana Abed-Kotob',N'Division Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 5,N'Wendy Barton',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 6,N'Charlotte F. Titus',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 7,N'Terry Blatt',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 8,N'Colleen H. Fleming',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 9,N'Elisabeth M. Gomez',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 10,N'Ryan D. Matheny',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 11,N'Alison P. Moylan',N'Division Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 12,N'Robin E. Neilson',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 13,N'Christopher M. Mrozowski',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 14,N'Diane E. Crow',N'Division Chief (Acting)', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 15,N'Diane E. Crow',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 16,N'Mitchell A. Cohn',N'Acting Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00')

   
/* Load email addresses */
INSERT INTO [dbo].[EmailAddress]
           ([EmailAddressTypeId]
           ,[Address]
           ,[Contact_ContactId]
           ,[Person_PersonId])
     VALUES
           (5,N'CandelariaAR@State.gov',@LastInsertedContactId + 1,NULL),
	   (5,N'GrabauskasCL@state.gov',@LastInsertedContactId + 2,NULL),
	   (5,N'Clegg-TrippME@state.gov',@LastInsertedContactId + 3,NULL),
	   (5,N'Abed-KotobS@state.gov',@LastInsertedContactId + 4,NULL),
	   (5,N'BartonW@state.gov',@LastInsertedContactId + 5,NULL),
	   (5,N'TitusCF@state.gov',@LastInsertedContactId + 6,NULL),
	   (5,N'BlattTM@state.gov',@LastInsertedContactId + 7,NULL),
	   (5,N'FlemingCH@state.gov',@LastInsertedContactId + 8,NULL),
	   (5,N'GomezEM2@state.gov',@LastInsertedContactId + 9,NULL),
	   (5,N'MathenyRD@state.gov',@LastInsertedContactId + 10,NULL),
	   (5,N'MoylanAP@state.gov',@LastInsertedContactId + 11,NULL),
	   (5,N'NeilsonRE@state.gov',@LastInsertedContactId + 12,NULL),
	   (5,N'MrozowskiCM@state.gov',@LastInsertedContactId + 13,NULL),
	   (5,N'CrowDE@state.gov',@LastInsertedContactId + 14,NULL),
	   (5,N'CrowDE@state.gov',@LastInsertedContactId + 15,NULL),
	   (5,N'CohnMA@state.gov',@LastInsertedContactId + 16,NULL)


/* Build Phone Numbers */
INSERT INTO [dbo].[PhoneNumber]
           ([Number]
           ,[PhoneNumberTypeId]
           ,[Contact_ContactId]
           ,[Person_PersonId])
     VALUES
           (N'(202) 632-9361',2,@LastInsertedContactId + 1,NULL),
           (N'(202) 632-3282',2,@LastInsertedContactId + 2,NULL),
           (N'(202) 632-3303',2,@LastInsertedContactId + 3,NULL),
           (N'(202) 632-9380',2,@LastInsertedContactId + 4,NULL),
           (N'(202) 632-9338',2,@LastInsertedContactId + 5,NULL),
           (N'(202) 632-9390',2,@LastInsertedContactId + 6,NULL),
           (N'(202) 632-3319',2,@LastInsertedContactId + 7,NULL),
           (N'(202) 632-9357',2,@LastInsertedContactId + 8,NULL),
           (N'(202) 632-9335',2,@LastInsertedContactId + 9,NULL),
           (N'(202) 632-9288',2,@LastInsertedContactId + 10,NULL),
           (N'(202) 632-6166',2,@LastInsertedContactId + 11,NULL),
           (N'(202) 632-6091',2,@LastInsertedContactId + 12,NULL),
           (N'(202) 632-3221',2,@LastInsertedContactId + 13,NULL),
           (N'(202) 632-6162',2,@LastInsertedContactId + 14,NULL),
           (N'(202) 632-6162',2,@LastInsertedContactId + 15,NULL),
           (N'(646) 282-2867',2,@LastInsertedContactId + 16,NULL)


 
/* Find the OrgIds for each org being loaded */
DECLARE @OrgIdToLoad  int

--ECA/PE/V
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/PE/V'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+1),
       (@OrgIdToLoad,@LastInsertedContactId+2),
       (@OrgIdToLoad,@LastInsertedContactId+3)

--ECA/PE/V/R
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/PE/V/R'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+4)
          
--ECA/PE/V/R/A
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/PE/V/R/A'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+5)

--ECA/PE/V/R/E
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/PE/V/R/E'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+6)


--ECA/PE/V/R/F
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/PE/V/R/F'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+7)


--ECA/PE/V/R/N
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/PE/V/R/N'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+8)


--ECA/PE/V/R/S
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/PE/V/R/S'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+9)

--ECA/PE/V/R/W
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/PE/V/R/W'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+10)

--ECA/PE/V/F
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/PE/V/F'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+11)

--ECA/PE/V/F/E
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/PE/V/F/E'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+12)


--ECA/PE/V/F/A
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/PE/V/F/A'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+13)

--ECA/PE/V/C
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/PE/V/C'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+14)

--ECA/PE/V/C/R
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/PE/V/C/R'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+15)


--ECA/PE/V/C/N
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/PE/V/C/N'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+16)




SET IDENTITY_INSERT [dbo].[Contact] OFF
GO

/*
*ECA/PE/V
	Alma R. Candelaria, Director	(202) 632-9361
	SA-5  3-B06	CandelariaAR@state.gov
	
	Carol L. Grabauskas, Deputy Director (VolVis and CR)	(202) 632-3282
	SA-5  3-B07	GrabauskasCL@state.gov

	Melissa E. Clegg-Tripp, Deputy Director (Regional Programs)	(202) 632-3303
	SA-5  3-B08	Clegg-TrippME@state.gov

*ECA/PE/V/R	Regional Programs Division
	Sana Abed-Kotob, Division Chief	(202) 632-9380
	SA-5 3-BB05	Abed-KotobS@state.gov
	
*ECA/PE/V/R/A	Africa Branch (AF) 
	Wendy Barton, Branch Chief	(202) 632-9338
	SA-5 3-K06	BartonW@state.gov
	
*ECA/PE/V/R/E	Europe & Eurasia Branch (EUR) 
	Charlotte F. Titus, Branch Chief	(202) 632-9390
	SA-5 3-T06	TitusCF@state.gov
	
*ECA/PE/V/R/F	East Asia & Pacific Branch (EAP) 
	Terry Blatt, Branch Chief	(202) 632-3319
	SA-5 3-Q06	BlattTM@state.gov
	
*ECA/PE/V/R/N	Near East and North Africa Branch (NEA) 
	Colleen H. Fleming, Branch Chief	(202) 632-9357
	SA-5 3-N06	FlemingCH@state.gov
	
	South and Central Asia Branch (SCA)
*ECA/PE/V/R/S	Elisabeth M. Gomez, Branch Chief	(202) 632-9335
	SA-5 3-M06	GomezEM2@state.gov
	
*ECA/PE/V/R/W	Western Hemisphere Branch (WHA) 
	Ryan D. Matheny, Branch Chief	(202) 632-9288
	SA-5 3-R06	MathenyRD@state.gov

*ECA/PE/V/F	Voluntary Visitors (VolVis) Division
	Alison P. Moylan, Division Chief	(202) 632-6166
	SA-5 05-N06	MoylanAP@state.gov
	
*ECA/PE/V/F/E	AF & EUR Branch
	Robin E. Neilson, Branch Chief	(202) 632-6091
	SA-5 05-M06	NeilsonRE@state.gov
	
*ECA/PE/V/F/A	WHA, EAP, NEA, & SCA Branch
	Christopher M. Mrozowski, Branch Chief	(202) 632-3221
	SA-5 05-R06	MrozowskiCM@state.gov

*ECA/PE/V/C	Community Resources Division
	Diane E. Crow, Division Chief (Acting)	(202) 632-6162
	SA-5 3-U06	CrowDE@state.gov
	
*ECA/PE/V/C/R	Community Relations Branch
	Diane E. Crow, Branch Chief	(202) 632-6162
	SA-5 3-U06	CrowDE@state.gov
	
*ECA/PE/V/C/N	New York Program Branch
	Mitchell A. Cohn, Acting Branch Chief	(646) 282-2867
	799 UN Plaza, 9th Floor, NYC	CohnMA@state.gov
	


*/

