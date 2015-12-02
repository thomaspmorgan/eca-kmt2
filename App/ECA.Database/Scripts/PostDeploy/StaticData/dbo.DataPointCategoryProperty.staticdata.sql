/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[DataPointCategoryProperty].

PRINT 'Updating static data table [dbo].[DataPointCategoryProperty]'

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
[DataPointCategoryPropertyId] int,
[DataPointCategoryId] int,
[DataPointPropertyId] int
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('1', '1', '1')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('2', '1', '2')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('3', '2', '1')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('4', '2', '2')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('5', '2', '3')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('6', '2', '4')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('7', '2', '5')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('8', '3', '1')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('9', '3', '2')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('10', '3', '3')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('11', '3', '4')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('12', '3', '5')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('13', '3', '6')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('14', '4', '7')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('15', '4', '8')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('16', '4', '9')
INSERT INTO @tblTempTable ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId]) VALUES ('17', '4', '10')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[DataPointCategoryProperty] ON
INSERT INTO [dbo].[DataPointCategoryProperty] ([DataPointCategoryPropertyId], [DataPointCategoryId], [DataPointPropertyId])
SELECT tmp.[DataPointCategoryPropertyId], tmp.[DataPointCategoryId], tmp.[DataPointPropertyId]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[DataPointCategoryProperty] tbl ON tbl.[DataPointCategoryPropertyId] = tmp.[DataPointCategoryPropertyId]
WHERE tbl.[DataPointCategoryPropertyId] IS NULL
SET IDENTITY_INSERT [dbo].[DataPointCategoryProperty] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[DataPointCategoryId] = tmp.[DataPointCategoryId],
LiveTable.[DataPointPropertyId] = tmp.[DataPointPropertyId]
FROM [dbo].[DataPointCategoryProperty] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[DataPointCategoryPropertyId] = tmp.[DataPointCategoryPropertyId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[DataPointCategoryProperty] FROM [dbo].[DataPointCategoryProperty] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[DataPointCategoryPropertyId] = tmp.[DataPointCategoryPropertyId]
	WHERE tmp.[DataPointCategoryPropertyId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[DataPointCategoryProperty]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO