/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [sevis].[USGovernmentAgency].

PRINT 'Updating static data table [sevis].[USGovernmentAgency]'

-- Set date format to ensure text dates are parsed correctly
SET DATEFORMAT ymd

-- Turn off affected rows being returned
SET NOCOUNT ON

-- Change this to 1 to delete missing records in the target
-- WARNING: Setting this to 1 can cause damage to your database
-- and cause failed deployment if there are any rows referencing
-- a record which has been deleted.
DECLARE @DeleteMissingRecords BIT
SET @DeleteMissingRecords = 0

-- 1: Define table variable
DECLARE @tblTempTable TABLE (
[AgencyId] int,
[AgencyCode] nvarchar(10),
[Description] nvarchar(250),
[History_CreatedBy] int,
[History_CreatedOn] datetimeoffset,
[History_RevisedBy] int,
[History_RevisedOn] datetimeoffset
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', 'ACT', 'ACTION (INCLUDING PEACE CORPS AND VISTA)', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', 'AID', 'AGENCY FOR INTERNATIONAL DEVELOPMENT', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', 'DOD', 'DEPARTMENT OF DEFENSE', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', 'DOE', 'DEPARTMENT OF ENERGY', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', 'DOT', 'DEPARTMENT OF TRANSPORTATION', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', 'EXIM', 'EXPORT-IMPORT BANK OF THE UNITED STATES', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', 'HHS', 'DEPARTMENT OF HEALTH AND HUMAN SERVICES', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('8', 'HUD', 'DEPARTMENT OF HOUSING AND URBAN DEVELOPMENT', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('9', 'DOL', 'DEPARTMENT OF LABOR', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('10', 'NASA', 'NATIONAL AERONAUTICS AND SPACE ADMINISTRATION', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('11', 'NSF', 'NATIONAL SCIENCE FOUNDATION', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('12', 'SI', 'SMITHSONIAN INSTITUTION', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('13', 'USDA', 'DEPARTMENT OF AGRICULTURE', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('14', 'VA', 'VETERANS ADMINISTRATION', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('15', 'BBG', 'BROADCASTING BOARD OF GOVERNORS', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('16', 'GAO', 'GENERAL ACCOUNTING OFFICE', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('17', 'HMC', 'HOLOCAUST MEMORIAL COUNCIL', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('18', 'LOC', 'LIBRARY OF CONGRESS', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('19', 'NEA', 'NATIONAL ENDOWMENT FOR THE ARTS', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('20', 'NDH', 'NATIONAL ENDOWMENT FOR THE HUMANITIES', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('21', 'USIP', 'U.S. INSTITUTE OF PEACE', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('22', 'OTHER', 'OTHER', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('23', 'DOC', 'DEPARTMENT OF COMMERCE', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('24', 'DOED', 'DEPARTMENT OF EDUCATION ', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('25', 'DOI', 'DEPARTMENT OF THE INTERIOR', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('26', 'DOJ', 'DEPARTMENT OF JUSTICE', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('27', 'DOS', 'DEPARTMENT OF STATE', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('28', 'TREAS', 'DEPARTMENT OF THE TREASURY', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')
INSERT INTO @tblTempTable ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('29', 'EPA', 'ENVIRONMENTAL PROTECTION AGENCY', '1', '10/9/2015 8:40:08 PM -04:00', '1', '10/9/2015 8:40:08 PM -04:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [sevis].[USGovernmentAgency] ON
INSERT INTO [sevis].[USGovernmentAgency] ([AgencyId], [AgencyCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[AgencyId], tmp.[AgencyCode], tmp.[Description], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [sevis].[USGovernmentAgency] tbl ON tbl.[AgencyId] = tmp.[AgencyId]
WHERE tbl.[AgencyId] IS NULL
SET IDENTITY_INSERT [sevis].[USGovernmentAgency] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[AgencyCode] = tmp.[AgencyCode],
LiveTable.[Description] = tmp.[Description],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [sevis].[USGovernmentAgency] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[AgencyId] = tmp.[AgencyId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [sevis].[USGovernmentAgency] FROM [sevis].[USGovernmentAgency] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[AgencyId] = tmp.[AgencyId]
	WHERE tmp.[AgencyId] IS NULL
END

PRINT 'Finished updating static data table [sevis].[USGovernmentAgency]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO