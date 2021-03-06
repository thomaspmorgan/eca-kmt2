﻿/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[DataPointCategory].

PRINT 'Updating static data table [dbo].[DataPointCategory]'

-- Set date format to ensure text dates are parsed correctly
SET DATEFORMAT ymd

-- Turn off affected rows being returned
SET NOCOUNT ON

-- Change this to 1 to delete missing records in the target
-- WARNING: Setting this to 1 can cause damage to your database
-- and cause failed deployment if there are any rows referencing
-- a record which has been deleted.
DECLARE @DeleteMissingRecords BIT
SET @DeleteMissingRecords = 1

-- 1: Define table variable
DECLARE @tblTempTable TABLE (
[DataPointCategoryId] int,
[DataPointCategoryName] nvarchar(50)
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.
INSERT INTO @tblTempTable ([DataPointCategoryId], [DataPointCategoryName]) VALUES ('1', 'Office')
INSERT INTO @tblTempTable ([DataPointCategoryId], [DataPointCategoryName]) VALUES ('2', 'Program')
INSERT INTO @tblTempTable ([DataPointCategoryId], [DataPointCategoryName]) VALUES ('3', 'Project')
INSERT INTO @tblTempTable ([DataPointCategoryId], [DataPointCategoryName]) VALUES ('4', 'Person')

-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[DataPointCategory] ON
INSERT INTO [dbo].[DataPointCategory] ([DataPointCategoryId], [DataPointCategoryName])
SELECT tmp.[DataPointCategoryId], tmp.[DataPointCategoryName]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[DataPointCategory] tbl ON tbl.[DataPointCategoryId] = tmp.[DataPointCategoryId]
WHERE tbl.[DataPointCategoryId] IS NULL
SET IDENTITY_INSERT [dbo].[DataPointCategory] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[DataPointCategoryName] = tmp.[DataPointCategoryName]
FROM [dbo].[DataPointCategory] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[DataPointCategoryId] = tmp.[DataPointCategoryId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[DataPointCategory] FROM [dbo].[DataPointCategory] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[DataPointCategoryId] = tmp.[DataPointCategoryId]
	WHERE tmp.[DataPointCategoryId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[DataPointCategory]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO