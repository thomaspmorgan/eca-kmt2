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
           (@LastInsertedContactId + 1,N'Meghann A. Curtis',N'Deputy Assistant Secretary', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 2,N'Marianne Craven',N'Managing Director', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 3,N'P. David Plack',N'Senior Policy Advisor', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 4,N'Lisa M. Kraus',N'Special Assistant for Policy', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 5,N'Paul H. Kruchoski',N'Special Assistant for Policy', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 6,N'Brent J. LaRosa',N'Special Assistant', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 7,N'Sarah H. Thompson',N'Staff Assistant', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 8,N'Arielle J. Berney',N'Presidential Management Fellow', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 9,N'Britta S. Bjornlund',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 10,N'Robin G. Bradley',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 11,N'Matthew P. McMahon',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 12,N'R. Wes Carrington',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 13,N'Donna A. Ives',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 14,N'Susan E. Borja',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 15,N'Jenny E. Verdaguer',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 16,N'Martha E. Estell',N'Director', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 17,N'Caryn B. Danz',N'Deputy Director', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 18,N'Alice Murray',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 19,N'Kevin McCaughey',N'Branch Chief (Acting)', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 20,N'Heidi Manley',N'Deputy Director', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 21,N'Heidi Arola',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 22,N'John Z. Sedlins',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00'),
           (@LastInsertedContactId + 23,N'Jennifer M. Gibson',N'Branch Chief', 0,N'4/1/2015 12:00:00 AM -05:00',0,N'4/1/2015 12:00:00 AM -05:00')



   
/* Load email addresses */
INSERT INTO [dbo].[EmailAddress]
           ([EmailAddressTypeId]
           ,[Address]
           ,[Contact_ContactId]
           ,[Person_PersonId])
     VALUES
           (5,N'CurtisMA@state.gov',@LastInsertedContactId + 1,NULL),
           (5,N'CravenMX@state.gov',@LastInsertedContactId + 2,NULL),
           (5,N'PlackPD@state.gov',@LastInsertedContactId + 3,NULL),
           (5,N'KrausLM@state.gov',@LastInsertedContactId + 4,NULL),
           (5,N'KruchoskiPH@state.gov',@LastInsertedContactId + 5,NULL),
           (5,N'LaRosaBJ@state.gov',@LastInsertedContactId + 6,NULL),
           (5,N'ThompsonSH@state.gov',@LastInsertedContactId + 7,NULL),
           (5,N'BerneyAJ@state.gov',@LastInsertedContactId + 8,NULL),
           (5,N'KirkME@state.gov',12,NULL),
           (5,N'MuckLS@state.gov',13,NULL),
           (5,N'BjornlundBS@State.gov',@LastInsertedContactId + 9,NULL),
           (5,N'BradleyRG@State.gov',@LastInsertedContactId + 10,NULL),
           (5,N'McMahonMP@State.gov',@LastInsertedContactId + 11,NULL),
           (5,N'CarringtonRW@state.gov',@LastInsertedContactId + 12,NULL),
           (5,N'IvesDA@State.gov',@LastInsertedContactId + 13,NULL),
           (5,N'BorjaSE@State.gov',@LastInsertedContactId + 14,NULL),
           (5,N'VerdaguerME@State.gov',@LastInsertedContactId + 15,NULL),
           (5,N'EstellME@state.gov',@LastInsertedContactId + 16,NULL),
           (5,N'DanzCB@State.gov',@LastInsertedContactId + 17,NULL),
           (5,N'MurrayAM@State.gov',@LastInsertedContactId + 18,NULL),
           (5,N'McCaugheyKB@State.gov',@LastInsertedContactId + 19,NULL),
           (5,N'ManleyHL@state.gov',@LastInsertedContactId + 20,NULL),
           (5,N'ArolaHR@state.gov',@LastInsertedContactId + 21,NULL),
           (5,N'SedlinsJZ@State.gov',@LastInsertedContactId + 22,NULL),
           (5,N'GibsonJM@State.gov',@LastInsertedContactId + 23,NULL)


	

/* Build Phone Numbers */
INSERT INTO [dbo].[PhoneNumber]
           ([Number]
           ,[PhoneNumberTypeId]
           ,[Contact_ContactId]
           ,[Person_PersonId])
     VALUES
           (N'(202) 632-9331',2,@LastInsertedContactId + 1,NULL),
           (N'(202) 632-9331',2,@LastInsertedContactId + 2,NULL),
           (N'(202) 632-9324',2,@LastInsertedContactId + 3,NULL),
           (N'(202) 632-2906',2,@LastInsertedContactId + 4,NULL),
           (N'(202) 632-3294',2,@LastInsertedContactId + 5,NULL),
           (N'(202) 632-9330',2,@LastInsertedContactId + 6,NULL),
           (N'(202) 632-9332',2,@LastInsertedContactId + 7,NULL),
           (N'(202) 632-6440',2,@LastInsertedContactId + 8,NULL),
           (N'(202) 632-3234',2,12,NULL),
           (N'(202) 632-3233',2,13,NULL),
           (N'(202) 632-3342',2,@LastInsertedContactId + 9,NULL),
           (N'(202) 632-3223',2,@LastInsertedContactId + 10,NULL),
           (N'(202) 632-9432',2,@LastInsertedContactId + 11,NULL),
           (N'(202) 632-9443',2,@LastInsertedContactId + 12,NULL),
           (N'(202) 632-6050',2,@LastInsertedContactId + 13,NULL),
           (N'(202) 632-3264',2,@LastInsertedContactId + 14,NULL),
           (N'(202) 632-6047',2,@LastInsertedContactId + 15,NULL),
           (N'(202) 632-9281',2,@LastInsertedContactId + 16,NULL),
           (N'(202) 632-9412',2,@LastInsertedContactId + 17,NULL),
           (N'(202) 632-9279',2,@LastInsertedContactId + 18,NULL),
           (N'(202) 632-9419',2,@LastInsertedContactId + 19,NULL),
           (N'(202) 632-3240',2,@LastInsertedContactId + 20,NULL),
           (N'(202) 632-6353',2,@LastInsertedContactId + 21,NULL),
           (N'(202) 632-6328',2,@LastInsertedContactId + 22,NULL),
           (N'(202) 632-6343',2,@LastInsertedContactId + 23,NULL)



 
/* Find the OrgIds for each org being loaded */
DECLARE @OrgIdToLoad  int

--ECA/A
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+1),
	(@OrgIdToLoad,@LastInsertedContactId+2),
	(@OrgIdToLoad,@LastInsertedContactId+3),
	(@OrgIdToLoad,@LastInsertedContactId+4),
	(@OrgIdToLoad,@LastInsertedContactId+5),
	(@OrgIdToLoad,@LastInsertedContactId+6),
	(@OrgIdToLoad,@LastInsertedContactId+7),
	(@OrgIdToLoad,@LastInsertedContactId+8)
       
--ECA/A/E/USS
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A/E/USS'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+9)


--ECA/A/E/AF
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A/E/AF'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+10)



--ECA/A/E/EAP
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A/E/EAP'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+11)


--ECA/A/E/EUR
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A/E/EUR'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+12)


--ECA/A/E/NEA
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A/E/NEA'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+13)



--ECA/A/E/SCA
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A/E/SCA'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+14)


--ECA/A/E/WHA
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A/E/WHA'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+15)



--ECA/A/L
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A/L'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+16),
       (@OrgIdToLoad,@LastInsertedContactId+17)


--ECA/A/L/W
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A/L/W'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+18)


--ECA/A/L/M
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A/L/M'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+19)


--ECA/A/S
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A/S'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+20)


--ECA/A/S/Q
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A/S/Q'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+20)


--ECA/A/S/A
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A/S/A'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+21)



--ECA/A/S/U
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A/S/U'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+22)


--ECA/A/S/X
SELECT @OrgIdToLoad = organizationid
FROM dbo.Organization
WHERE officeSymbol = N'ECA/A/S/X'

/* Create Contact to Organization Mappings */
INSERT INTO [dbo].[OrganizationContact]
	(OrganizationId,
         ContactId)
VALUES (@OrgIdToLoad,@LastInsertedContactId+23)





SET IDENTITY_INSERT [dbo].[Contact] OFF
GO

/*
*ECA/A	*Deputy Assistant Secretary for Academic Programs
	*Meghann A. Curtis, Deputy Assistant Secretary	(202) 632-9331
	SA-5 5-U17	CurtisMA@state.gov
	
	*Marianne Craven, Managing Director	(202) 632-9331
	SA-5 5-S17	CravenMX@state.gov
	
	*P. David Plack, Senior Policy Advisor	(202) 632-9324
	SA-5 5-R17	PlackPD@state.gov
	
	*Lisa M. Kraus, Special Assistant for Policy	(202) 632-2906
	SA-5 5-Q14	KrausLM@state.gov
	
	*Paul H. Kruchoski, Special Assistant for Policy	(202) 632-3294
	SA-5 5-P14	KruchoskiPH@state.gov
	
	*Brent J. LaRosa, Special Assistant	(202) 632-9330
	SA-5 5-R15	LaRosaBJ@state.gov
	
	*Sarah H. Thompson, Staff Assistant	(202) 632-9332
	SA-5 5-S15	ThompsonSH@state.gov
	
	*Arielle J. Berney, Presidential Management Fellow	(202) 632-6440
	SA-5 5-P15	BerneyAJ@state.gov

*ECA/A/E	Office of Academic Exchange Programs
	Mary E. Kirk, Director 	(202) 632-3234
	SA-5 4-B06	KirkME@state.gov
	
	Lana S. Muck, Deputy Director ………………………………...(202) 632-3233
	SA-5 4-B07…………………………………………………MuckLS@state.gov
	
*ECA/A/E/USS	Britta S. Bjornlund, Branch Chief	(202) 632-3342
	SA-5 4-N06, Study of the U.S.	BjornlundBS@State.gov
	
*ECA/A/E/AF	Robin G. Bradley, Branch Chief	(202) 632-3223
	SA-5 4-B11, African Programs	BradleyRG@State.gov
	
*ECA/A/E/EAP	Matthew P. McMahon, Branch Chief	(202) 632-9432
	SA-5 4-K06, East Asian & Pacific Programs	McMahonMP@State.gov
	
*ECA/A/E/EUR	R. Wes Carrington, Branch Chief	(202) 632-9443
	SA-5 4-B08, Europe & Eurasian Programs	CarringtonRW@state.gov
	
*ECA/A/E/NEA	Donna A. Ives, Branch Chief	(202) 632-6050
	SA-5 4-B09, Near East Programs	IvesDA@State.gov
	
*ECA/A/E/SCA	Susan E. Borja, Branch Chief	(202) 632-3264
	SA-5 4-B12, South & Central Asian Programs	BorjaSE@State.gov

*ECA/A/E/WHA	Jenny E. Verdaguer, Branch Chief	(202) 632-6047
	SA-5 4-J06, Western Hemisphere Programs	VerdaguerME@State.gov


	Office of English Language Programs
*ECA/A/L	Martha E. Estell, Director	(202) 632-9281
	SA-5 4-B16	EstellME@state.gov
	
	Caryn B. Danz, Deputy Director	(202) 632-9412
	SA-5 4-B15	DanzCB@State.gov
	
*ECA/A/L/W	English Language Programs Branch
	Alice Murray, Branch Chief	(202) 632-9279
	SA-5 4-B13	MurrayAM@State.gov
---	
*ECA/A/L/M	English Language Materials Development Branch
	Kevin McCaughey, Branch Chief (Acting)	(202) 632-9419
	SA-5 4-I15	McCaugheyKB@State.gov
	
*ECA/A/S	Office of Global Educational Programs
	Vacant, Director	(202) 632-6342
	SA-5 4-CC17	
	
	Heidi Manley, Deputy Director.....................................................(202) 632-3240
	SA-5 4-L12.........................................................................ManleyHL@state.gov

*ECA/A/S/A	Educational Information & Resources Branch
	Heidi Arola, Branch Chief	(202) 632-6353
	SA-5 4-CC16	ArolaHR@state.gov
	
*ECA/A/S/U	Humphrey Fellowships & Institutional Linkages Branch
	John Z. Sedlins, Branch Chief	(202) 632-6328
	SA-5 4-CC13	SedlinsJZ@State.gov

*ECA/A/S/X	Teacher Exchange Branch
	Jennifer M. Gibson, Branch Chief	(202) 632-6343
	SA-5 4-CC14	GibsonJM@State.gov
	
*ECA/A/S/Q	U.S. Study Abroad Branch
	Heidi Manley, Deputy Director...........................................(202) 632-3240
	SA-5 4-L12................................................................ManleyHL@state.gov
	


*/

