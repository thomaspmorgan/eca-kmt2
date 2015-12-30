/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [sevis].[OccupationalCategory].

PRINT 'Updating static data table [sevis].[OccupationalCategory]'

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
[OccupationalCategoryId] int,
[OccupationalCategoryCode] char(2),
[Description] nvarchar(200),
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
INSERT INTO @tblTempTable ([OccupationalCategoryId], [OccupationalCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', '01', 'Arts and Culture', '0', '5/29/2015 9:46:55 AM -04:00', '0', '5/29/2015 9:46:55 AM -04:00')
INSERT INTO @tblTempTable ([OccupationalCategoryId], [OccupationalCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', '02', 'Information Media and Communications', '0', '5/29/2015 9:46:55 AM -04:00', '0', '5/29/2015 9:46:55 AM -04:00')
INSERT INTO @tblTempTable ([OccupationalCategoryId], [OccupationalCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', '03', 'Education, Social Sciences, Library Sciences, Counseling and Social Services', '0', '5/29/2015 9:46:55 AM -04:00', '0', '5/29/2015 9:46:55 AM -04:00')
INSERT INTO @tblTempTable ([OccupationalCategoryId], [OccupationalCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', '04', 'Management Business, Commerce and Finance', '0', '5/29/2015 9:46:55 AM -04:00', '0', '5/29/2015 9:46:55 AM -04:00')
INSERT INTO @tblTempTable ([OccupationalCategoryId], [OccupationalCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', '05', 'Health Related Occupations', '0', '5/29/2015 9:46:55 AM -04:00', '0', '5/29/2015 9:46:55 AM -04:00')
INSERT INTO @tblTempTable ([OccupationalCategoryId], [OccupationalCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', '07', 'The Sciences, engineering, Architecture, Mathematics and Industrial Occupations', '0', '5/29/2015 9:46:55 AM -04:00', '0', '5/29/2015 9:46:55 AM -04:00')
INSERT INTO @tblTempTable ([OccupationalCategoryId], [OccupationalCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', '08', 'Construction and building trades', '0', '5/29/2015 9:46:55 AM -04:00', '0', '5/29/2015 9:46:55 AM -04:00')
INSERT INTO @tblTempTable ([OccupationalCategoryId], [OccupationalCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('8', '09', 'Agriculture, Forestry and Fishing', '0', '5/29/2015 9:46:55 AM -04:00', '0', '5/29/2015 9:46:55 AM -04:00')
INSERT INTO @tblTempTable ([OccupationalCategoryId], [OccupationalCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('9', '10', 'Public Administration and Law', '0', '5/29/2015 9:46:55 AM -04:00', '0', '5/29/2015 9:46:55 AM -04:00')
INSERT INTO @tblTempTable ([OccupationalCategoryId], [OccupationalCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('10', '11', 'Other', '0', '5/29/2015 9:46:55 AM -04:00', '0', '5/29/2015 9:46:55 AM -04:00')
INSERT INTO @tblTempTable ([OccupationalCategoryId], [OccupationalCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('11', '12', 'Hospitality and Tourism', '0', '5/29/2015 9:46:55 AM -04:00', '0', '5/29/2015 9:46:55 AM -04:00')
INSERT INTO @tblTempTable ([OccupationalCategoryId], [OccupationalCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('12', '99', 'Unknown', '0', '5/29/2015 9:46:55 AM -04:00', '0', '5/29/2015 9:46:55 AM -04:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [sevis].[OccupationalCategory] ON
INSERT INTO [sevis].[OccupationalCategory] ([OccupationalCategoryId], [OccupationalCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[OccupationalCategoryId], tmp.[OccupationalCategoryCode], tmp.[Description], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [sevis].[OccupationalCategory] tbl ON tbl.[OccupationalCategoryId] = tmp.[OccupationalCategoryId]
WHERE tbl.[OccupationalCategoryId] IS NULL
SET IDENTITY_INSERT [sevis].[OccupationalCategory] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[OccupationalCategoryCode] = tmp.[OccupationalCategoryCode],
LiveTable.[Description] = tmp.[Description],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [sevis].[OccupationalCategory] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[OccupationalCategoryId] = tmp.[OccupationalCategoryId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [sevis].[OccupationalCategory] FROM [sevis].[OccupationalCategory] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[OccupationalCategoryId] = tmp.[OccupationalCategoryId]
	WHERE tmp.[OccupationalCategoryId] IS NULL
END

PRINT 'Finished updating static data table [sevis].[OccupationalCategory]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO