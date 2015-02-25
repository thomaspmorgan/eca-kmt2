/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[Country].

PRINT 'Updating static data table [dbo].[Country]'

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
[Id] int,
[Name] varchar(50)
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.


-- 3: Insert any new items into the table from the table variable
INSERT INTO [dbo].[Country] ([Id], [Name])
SELECT tmp.[Id], tmp.[Name]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[Country] tbl ON tbl.[Id] = tmp.[Id]
WHERE tbl.[Id] IS NULL

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[Name] = tmp.[Name]
FROM [dbo].[Country] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[Id] = tmp.[Id]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[Country] FROM [dbo].[Country] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[Id] = tmp.[Id]
	WHERE tmp.[Id] IS NULL
END

PRINT 'Finished updating static data table [dbo].[Country]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO