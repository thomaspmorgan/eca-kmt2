/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [cam].[Principal].

PRINT 'Updating static data table [cam].[Principal]'

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
[PrincipalId] int,
[PrincipalTypeId] int
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.
INSERT INTO @tblTempTable ([PrincipalId], [PrincipalTypeId]) VALUES ('1', '1')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [cam].[Principal] ON
INSERT INTO [cam].[Principal] ([PrincipalId], [PrincipalTypeId])
SELECT tmp.[PrincipalId], tmp.[PrincipalTypeId]
FROM @tblTempTable tmp
LEFT JOIN [cam].[Principal] tbl ON tbl.[PrincipalId] = tmp.[PrincipalId]
WHERE tbl.[PrincipalId] IS NULL
SET IDENTITY_INSERT [cam].[Principal] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[PrincipalTypeId] = tmp.[PrincipalTypeId]
FROM [cam].[Principal] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[PrincipalId] = tmp.[PrincipalId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [cam].[Principal] FROM [cam].[Principal] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[PrincipalId] = tmp.[PrincipalId]
	WHERE tmp.[PrincipalId] IS NULL
END

PRINT 'Finished updating static data table [cam].[Principal]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO