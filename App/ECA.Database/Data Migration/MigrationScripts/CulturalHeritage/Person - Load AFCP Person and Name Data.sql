USE AFCP
GO

Declare @LastName nvarchar(max),@FirstName nvarchar(max),@NamePrefix nvarchar(max), @NameSuffix nvarchar(max)
Declare @gender int
Declare @DateOfBirth nvarchar(max) = N'1/1/2015 12:00:00 AM -05:00'
Declare @Ethnicity nvarchar(max) = NULL
Declare @PermissionToContact int = 1
Declare @EvaluationRetention nvarchar(max) = NULL
Declare @Impactid int = 1
Declare @History_CreatedBy int = 0
Declare @History_CreatedOn nvarchar(max) = N'2/18/2015 12:00:00 AM -05:00'
Declare @History_RevisedBy int = 0
Declare @History_RevisedOn nvarchar(max) = N'2/18/2015 12:00:00 AM -05:00'
Declare @LocationId int
Declare @MedicalConditions nvarchar(max) = NULL
Declare @Awards nvarchar(max) = NULL
Declare @Country nvarchar(max)
Declare @IndividualTypeId int
Declare @PersonId int
Declare @OrganizationId int
Declare @PersonId_Found int=0
Declare @SqlStmt nvarchar(max) = NULL
DECLARE @ParmDefinition NVARCHAR(500)
DECLARE @IntVariable INT

SET @ParmDefinition = N'@PersonId_FoundOUT int OUTPUT'

Declare @cursorIndividualParticipant CURSOR

/* Define the cursor */
SET @cursorIndividualParticipant = CURSOR FOR
SELECT p.[Name Last], 
       p.[Name First],
       p.Prefix,
       p.suffix,
       CASE 
          WHEN gender = 'M' THEN 0 
          WHEN gender = 'F' THEN 1
	  WHEN gender IS NULL THEN 3
	  ELSE 2
       END,
       p.country,
       CASE
         WHEN p.country = 'USA' THEN 193
         ELSE l.locationid
       END,
       o.organizationid
  FROM AFCP.dbo.participants p
--  LEFT OUTER JOIN eca_dev_backup.dbo.location l ON (l.locationtypeid = 3 AND l.locationname = p.country)
   LEFT OUTER JOIN eca_dev.eca_dev.dbo.location l ON (l.locationtypeid = 3 AND l.locationname = p.country)
--  LEFT OUTER JOIN eca_dev_backup.dbo.organization o ON (o.description = p.institution)
  LEFT OUTER JOIN eca_dev.eca_dev.dbo.organization o ON (o.description = p.institution)
 WHERE [Name Last] IS NOT NULL
 GROUP BY p.[Name Last], 
       p.[Name First],
       p.Prefix,
       p.suffix,
       p.gender,
       p.country,
	   l.locationid,
	   o.organizationid
 ORDER By [Name Last],[Name First]

/* Get the Individual ParticipantType */
SELECT @IndividualTypeId = ParticipantTypeId
--FROM ECA_Dev_Backup.dbo.ParticipantType
FROM ECA_Dev.ECA_Dev.dbo.ParticipantType
WHERE Name = N'Individual'

/* Open the cursor */
OPEN @cursorIndividualParticipant

/* Fetch the first project */
FETCH NEXT FROM @cursorIndividualParticipant 
 INTO @LastName, @FirstName, @NamePrefix, @NameSuffix, @Gender, @Country, @LocationId, @organizationid

/* Loop thru all projects (staging) - should match DB */
WHILE @@FETCH_STATUS = 0
BEGIN

/* See if the person already exists */
--SET @SqlStmt = 'SELECT @PersonId_FoundOUT=Personid FROM eca_dev.eca_dev.dbo.CombinedNameVw WHERE'
--SET @SqlStmt = 'SELECT @PersonId_FoundOUT=Personid FROM eca_dev_backup.dbo.CombinedNameVw WHERE'
/* LastName NULL Check */
--IF @LastName IS NULL SET @SqlStmt = @SqlStmt +' '+ 'LastName IS NULL'
--ELSE SET @SqlStmt = @SqlStmt +' '+ 'LastName = ''' + @LastName + ''''
/* FirstName is NULL Check */
--IF @FirstName IS NULL SET @SqlStmt = @SqlStmt +' AND '+ 'FirstName IS NULL'
--ELSE SET @SqlStmt = @SqlStmt +' AND '+ 'FirstName = ''' + @FirstName + ''''
/* Prefix is NULL Check */
--IF @NamePrefix IS NULL SET @SqlStmt = @SqlStmt +' AND '+ 'Prefix IS NULL'
--ELSE SET @SqlStmt = @SqlStmt +' AND '+ 'Prefix = ''' + @NamePrefix + ''''
/* Suffix is NULL Check */
--IF @NameSuffix IS NULL SET @SqlStmt = @SqlStmt +' AND '+ 'Suffix IS NULL'
--ELSE SET @SqlStmt = @SqlStmt +' AND '+ 'Suffix = ''' + @NameSuffix + ''''

--print @SQLStmt

/* Run the statement to get results */           
--EXECUTE sp_executesql @SqlStmt, @ParmDefinition, @PersonId_FoundOUT=@PersonId_Found OUTPUT

/* If there is no match then we add person and related tables */
--IF @@ROWCOUNT <> 0 SELECT @PersonId_Found
--ELSE SET @PersonId_Found = 0

--print @PersonId_Found
SET @PersonId_Found = 0   /* always do this */

/* If the Person already exists then don't do the INSERT to Person, but use the */
IF @PersonID_Found = 0       
BEGIN
/* Insert the Person row */
INSERT INTO ECA_Dev.ECA_Dev.dbo.Person
--INSERT INTO ECA_Dev_Backup.dbo.Person
(Gender,DateOfBirth,Ethnicity,PermissionToContact,EvaluationRetention,
 History_CreatedBy,History_CreatedOn,History_RevisedBy,History_RevisedOn,
 Location_LocationId,MedicalConditions,Awards)
VALUES
(@Gender,@DateOfBirth,@Ethnicity,@PermissionToContact,@EvaluationRetention,
 @History_CreatedBy,@History_CreatedOn,@History_RevisedBy,@History_RevisedOn,
 NULL,@MedicalConditions,@Awards)
 
/* Retrieve the PersonId just created (Identity) */
--SELECT @PersonId = IDENT_CURRENT('ECA_Dev_Backup.dbo.Person')
--SELECT @PersonId = IDENT_CURRENT('ECA_Dev.ECA_Dev.dbo.Person')

/* Have to get current identity via linked server this way */
EXEC @PersonId = ECA_Dev.ECA_Dev.dbo.sp_GetCurrentPersonIdentity
--SELECT 'Person Identity' = @PersonId

/* Insert a Participant row */
INSERT INTO ECA_Dev.ECA_Dev.dbo.Participant
--INSERT INTO ECA_Dev_Backup.dbo.Participant
(OrganizationId,PersonId,ParticipantTypeId,History_CreatedBy,History_CreatedOn,History_RevisedBy,History_RevisedOn)
VALUES
(@Organizationid,@PersonId,@IndividualTypeId,@History_CreatedBy,@History_CreatedOn,@History_RevisedBy,@History_RevisedOn)

/* Insert Name Part rows */
/* Last Name */
IF @LastName IS NOT NULL
  INSERT INTO ECA_Dev.ECA_Dev.dbo.NamePart
--  INSERT INTO ECA_Dev_Backup.dbo.NamePart
    (Value,NameType,PersonId)
  VALUES
    (@LastName,1,@PersonId)

/* First Name */
IF @FirstName IS NOT NULL
  INSERT INTO ECA_Dev.ECA_Dev.dbo.NamePart
--  INSERT INTO ECA_Dev_Backup.dbo.NamePart
    (Value,NameType,PersonId)
  VALUES
    (@FirstName,2,@PersonId)

/* Prefix */
IF @NamePrefix IS NOT NULL
  INSERT INTO ECA_Dev.ECA_Dev.dbo.NamePart
--  INSERT INTO ECA_Dev_Backup.dbo.NamePart
    (Value,NameType,PersonId)
  VALUES
    (@NamePrefix,3,@PersonId)

/* Suffix */
IF @NameSuffix IS NOT NULL
  INSERT INTO ECA_Dev.ECA_Dev.dbo.NamePart
--  INSERT INTO ECA_Dev_Backup.dbo.NamePart
    (Value,NameType,PersonId)
  VALUES
    (@NameSuffix,4,@PersonId)

/* Country */
IF @LocationId IS NOT NULL
  INSERT INTO ECA_Dev.ECA_Dev.dbo.CitizenCountry
--  INSERT INTO ECA_Dev_Backup.dbo.CitizenCountry
    (PersonId,LocationId)
  VALUES
    (@PersonId,@LocationId)
END

/* Fetch the next project */
FETCH NEXT FROM @cursorIndividualParticipant 
 INTO @LastName, @FirstName, @NamePrefix, @NameSuffix, @Gender, @Country, @LocationId, @OrganizationId
END

/* Cleanup */
CLOSE @cursorIndividualParticipant
DEALLOCATE @cursorIndividualParticipant
GO



