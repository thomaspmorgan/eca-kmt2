/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [sevis].[StudentTermination].

PRINT 'Updating static data table [sevis].[StudentTermination]'

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
[StudentTerminationId] int,
[TerminationCode] char(2),
[Description] nvarchar(100),
[F_1_Ind] char(1),
[M_1_Ind] char(1),
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
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', '01', 'UNAUTHORIZED WITHDRAWAL', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', '02', 'DEATH', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', '03', 'UNAUTHORIZED EMPLOYMENT', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', '04', 'UNAUTHORIZED DROP BELOW FULL COURSE', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', '05', 'AUTHORIZED DROP BELOW FULL COURSE TIME EXCEEDED', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', '06', 'CHANGE OF NONIMMIGRANT CLASSIFICATION', 'N', 'N', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', '07', 'CHANGE OF NONIMMIGRANT CLASSIFICATION DENIED', 'N', 'N', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('8', '08', 'EXPULSIONS', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('9', '09', 'SUSPENSION', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('10', '10', 'ABSENT FROM COUNTRY FOR FIVE MONTHS', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('11', '11', 'FAILURE TO ENROLL', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('12', '12', 'COSTS EXCEED RESOURCES', 'N', 'N', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('13', '13', 'TRANSFER STUDENT NO SHOW', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('14', '14', 'DENIED TRANSFER', 'N', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('15', '15', 'EXTENSION DENIED', 'N', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('16', '16', 'OTHERWISE FAILING TO MAINTAIN STATUS', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('17', '17', 'VIOLATION OF CHANGE OF STATUS REQUIREMENTS', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('18', '18', 'CHANGE OF STATUS DENIED', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('19', '19', 'CHANGE OF STATUS WITHDRAWN', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('20', '20', 'CHANGE OF STATUS APPROVED', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('21', '21', 'TRANSFER WITHDRAWN', 'N', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('22', '22', 'NO SHOW - MANUAL TERMINATION', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('23', '23', 'AUTHORIZED EARLY WITHDRAWAL', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('24', '24', 'NO SHOW - SYSTEM TERMINATION', 'N', 'N', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')
INSERT INTO @tblTempTable ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('25', '25', 'SCHOOL WITHDRAWN', 'Y', 'Y', '0', '5/29/2015 9:47:59 AM -04:00', '0', '5/29/2015 9:47:59 AM -04:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [sevis].[StudentTermination] ON
INSERT INTO [sevis].[StudentTermination] ([StudentTerminationId], [TerminationCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[StudentTerminationId], tmp.[TerminationCode], tmp.[Description], tmp.[F_1_Ind], tmp.[M_1_Ind], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [sevis].[StudentTermination] tbl ON tbl.[StudentTerminationId] = tmp.[StudentTerminationId]
WHERE tbl.[StudentTerminationId] IS NULL
SET IDENTITY_INSERT [sevis].[StudentTermination] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[TerminationCode] = tmp.[TerminationCode],
LiveTable.[Description] = tmp.[Description],
LiveTable.[F_1_Ind] = tmp.[F_1_Ind],
LiveTable.[M_1_Ind] = tmp.[M_1_Ind],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [sevis].[StudentTermination] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[StudentTerminationId] = tmp.[StudentTerminationId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [sevis].[StudentTermination] FROM [sevis].[StudentTermination] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[StudentTerminationId] = tmp.[StudentTerminationId]
	WHERE tmp.[StudentTerminationId] IS NULL
END

PRINT 'Finished updating static data table [sevis].[StudentTermination]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO