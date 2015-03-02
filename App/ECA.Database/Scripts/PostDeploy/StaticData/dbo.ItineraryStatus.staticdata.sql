/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[ItineraryStatus].

PRINT 'Updating static data table [dbo].[ItineraryStatus]'

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
[ItineraryStatusId] int,
[ItineraryStatusName] nvarchar(20),
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
INSERT INTO @tblTempTable ([ItineraryStatusId], [ItineraryStatusName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', 'Planned', '0', '2/22/2015 12:00:00 AM -05:00', '0', '2/22/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([ItineraryStatusId], [ItineraryStatusName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', 'InProgress', '0', '2/22/2015 12:00:00 AM -05:00', '0', '2/22/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([ItineraryStatusId], [ItineraryStatusName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', 'Completed', '0', '2/22/2015 12:00:00 AM -05:00', '0', '2/22/2015 12:00:00 AM -05:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[ItineraryStatus] ON
INSERT INTO [dbo].[ItineraryStatus] ([ItineraryStatusId], [ItineraryStatusName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[ItineraryStatusId], tmp.[ItineraryStatusName], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[ItineraryStatus] tbl ON tbl.[ItineraryStatusId] = tmp.[ItineraryStatusId]
WHERE tbl.[ItineraryStatusId] IS NULL
SET IDENTITY_INSERT [dbo].[ItineraryStatus] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[ItineraryStatusName] = tmp.[ItineraryStatusName],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [dbo].[ItineraryStatus] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[ItineraryStatusId] = tmp.[ItineraryStatusId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[ItineraryStatus] FROM [dbo].[ItineraryStatus] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[ItineraryStatusId] = tmp.[ItineraryStatusId]
	WHERE tmp.[ItineraryStatusId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[ItineraryStatus]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO