
/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[DataPointProperty].

PRINT 'Updating static data table [dbo].[DataPointProperty]'

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
[DataPointPropertyId] int,
[DataPointPropertyName] nvarchar(50)
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.
INSERT INTO @tblTempTable ([DataPointPropertyId], [DataPointPropertyName]) VALUES ('1', 'Themes')
INSERT INTO @tblTempTable ([DataPointPropertyId], [DataPointPropertyName]) VALUES ('2', 'Goals')
INSERT INTO @tblTempTable ([DataPointPropertyId], [DataPointPropertyName]) VALUES ('3', 'Regions')
INSERT INTO @tblTempTable ([DataPointPropertyId], [DataPointPropertyName]) VALUES ('4', 'Categories')
INSERT INTO @tblTempTable ([DataPointPropertyId], [DataPointPropertyName]) VALUES ('5', 'Objectives')
INSERT INTO @tblTempTable ([DataPointPropertyId], [DataPointPropertyName]) VALUES ('6', 'Locations')
INSERT INTO @tblTempTable ([DataPointPropertyId], [DataPointPropertyName]) VALUES ('7', 'Addresses')
INSERT INTO @tblTempTable ([DataPointPropertyId], [DataPointPropertyName]) VALUES ('8', 'Emails')
INSERT INTO @tblTempTable ([DataPointPropertyId], [DataPointPropertyName]) VALUES ('9', 'Language Proficiencies')
INSERT INTO @tblTempTable ([DataPointPropertyId], [DataPointPropertyName]) VALUES ('10', 'Phone Numbers')

-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[DataPointProperty] ON
INSERT INTO [dbo].[DataPointProperty] ([DataPointPropertyId], [DataPointPropertyName])
SELECT tmp.[DataPointPropertyId], tmp.[DataPointPropertyName]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[DataPointProperty] tbl ON tbl.[DataPointPropertyId] = tmp.[DataPointPropertyId]
WHERE tbl.[DataPointPropertyId] IS NULL
SET IDENTITY_INSERT [dbo].[DataPointProperty] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[DataPointPropertyName] = tmp.[DataPointPropertyName]
FROM [dbo].[DataPointProperty] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[DataPointPropertyId] = tmp.[DataPointPropertyId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[DataPointProperty] FROM [dbo].[DataPointProperty] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[DataPointPropertyId] = tmp.[DataPointPropertyId]
	WHERE tmp.[DataPointPropertyId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[DataPointProperty]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO
