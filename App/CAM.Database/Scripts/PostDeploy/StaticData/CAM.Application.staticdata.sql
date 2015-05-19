/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [cam].[Application].

PRINT 'Updating static data table [cam].[Application]'

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
[ResourceId] int,
[ApplicationName] nvarchar(50),
[CreatedOn] datetimeoffset,
[CreatedBy] int,
[RevisedOn] datetimeoffset,
[RevisedBy] int
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.
INSERT INTO @tblTempTable ([ResourceId], [ApplicationName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy]) VALUES ('1', 'KMT', '4/15/2015 12:00:00 AM -04:00', '1', '4/15/2015 12:00:00 AM -04:00', '1')


-- 3: Insert any new items into the table from the table variable
INSERT INTO [cam].[Application] ([ResourceId], [ApplicationName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy])
SELECT tmp.[ResourceId], tmp.[ApplicationName], tmp.[CreatedOn], tmp.[CreatedBy], tmp.[RevisedOn], tmp.[RevisedBy]
FROM @tblTempTable tmp
LEFT JOIN [cam].[Application] tbl ON tbl.[ResourceId] = tmp.[ResourceId]
WHERE tbl.[ResourceId] IS NULL

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[ApplicationName] = tmp.[ApplicationName],
LiveTable.[CreatedOn] = tmp.[CreatedOn],
LiveTable.[CreatedBy] = tmp.[CreatedBy],
LiveTable.[RevisedOn] = tmp.[RevisedOn],
LiveTable.[RevisedBy] = tmp.[RevisedBy]
FROM [cam].[Application] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[ResourceId] = tmp.[ResourceId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [cam].[Application] FROM [cam].[Application] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[ResourceId] = tmp.[ResourceId]
	WHERE tmp.[ResourceId] IS NULL
END

PRINT 'Finished updating static data table [cam].[Application]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO