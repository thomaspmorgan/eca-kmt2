/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [sevis].[EducationLevel].

PRINT 'Updating static data table [sevis].[EducationLevel]'

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
[EducationLevelId] int,
[EducationLevelCode] char(2),
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
INSERT INTO @tblTempTable ([EducationLevelId], [EducationLevelCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', '01', 'PRIMARY', 'Y', 'N', '0', '5/29/2015 9:44:04 AM -04:00', '0', '5/29/2015 9:44:04 AM -04:00')
INSERT INTO @tblTempTable ([EducationLevelId], [EducationLevelCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', '02', 'SECONDARY', 'Y', 'N', '0', '5/29/2015 9:44:04 AM -04:00', '0', '5/29/2015 9:44:04 AM -04:00')
INSERT INTO @tblTempTable ([EducationLevelId], [EducationLevelCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', '03', 'ASSOCIATE', 'Y', 'N', '0', '5/29/2015 9:44:04 AM -04:00', '0', '5/29/2015 9:44:04 AM -04:00')
INSERT INTO @tblTempTable ([EducationLevelId], [EducationLevelCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', '04', 'BACHELOR''S', 'Y', 'N', '0', '5/29/2015 9:44:04 AM -04:00', '0', '5/29/2015 9:44:04 AM -04:00')
INSERT INTO @tblTempTable ([EducationLevelId], [EducationLevelCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', '05', 'MASTER''S', 'Y', 'N', '0', '5/29/2015 9:44:04 AM -04:00', '0', '5/29/2015 9:44:04 AM -04:00')
INSERT INTO @tblTempTable ([EducationLevelId], [EducationLevelCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', '06', 'DOCTORATE', 'Y', 'N', '0', '5/29/2015 9:44:04 AM -04:00', '0', '5/29/2015 9:44:04 AM -04:00')
INSERT INTO @tblTempTable ([EducationLevelId], [EducationLevelCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', '07', 'LANGUAGE TRAINING', 'Y', 'N', '0', '5/29/2015 9:44:04 AM -04:00', '0', '5/29/2015 9:44:04 AM -04:00')
INSERT INTO @tblTempTable ([EducationLevelId], [EducationLevelCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('8', '08', 'HIGH SCHOOL', 'N', 'Y', '0', '5/29/2015 9:44:04 AM -04:00', '0', '5/29/2015 9:44:04 AM -04:00')
INSERT INTO @tblTempTable ([EducationLevelId], [EducationLevelCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('9', '09', 'FLIGHT TRAINING', 'N', 'Y', '0', '5/29/2015 9:44:04 AM -04:00', '0', '5/29/2015 9:44:04 AM -04:00')
INSERT INTO @tblTempTable ([EducationLevelId], [EducationLevelCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('10', '10', 'VOCATIONAL SCHOOL', 'N', 'Y', '0', '5/29/2015 9:44:04 AM -04:00', '0', '5/29/2015 9:44:04 AM -04:00')
INSERT INTO @tblTempTable ([EducationLevelId], [EducationLevelCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('11', '11', 'OTHER', 'Y', 'N', '0', '5/29/2015 9:44:04 AM -04:00', '0', '5/29/2015 9:44:04 AM -04:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [sevis].[EducationLevel] ON
INSERT INTO [sevis].[EducationLevel] ([EducationLevelId], [EducationLevelCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[EducationLevelId], tmp.[EducationLevelCode], tmp.[Description], tmp.[F_1_Ind], tmp.[M_1_Ind], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [sevis].[EducationLevel] tbl ON tbl.[EducationLevelId] = tmp.[EducationLevelId]
WHERE tbl.[EducationLevelId] IS NULL
SET IDENTITY_INSERT [sevis].[EducationLevel] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[EducationLevelCode] = tmp.[EducationLevelCode],
LiveTable.[Description] = tmp.[Description],
LiveTable.[F_1_Ind] = tmp.[F_1_Ind],
LiveTable.[M_1_Ind] = tmp.[M_1_Ind],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [sevis].[EducationLevel] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[EducationLevelId] = tmp.[EducationLevelId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [sevis].[EducationLevel] FROM [sevis].[EducationLevel] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[EducationLevelId] = tmp.[EducationLevelId]
	WHERE tmp.[EducationLevelId] IS NULL
END

PRINT 'Finished updating static data table [sevis].[EducationLevel]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO