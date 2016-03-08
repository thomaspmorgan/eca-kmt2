/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[OrganizationStatus].

PRINT 'Updating static data table [dbo].[OrganizationStatus]'

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
[OrganizationStatusId] int,
[Status] nvarchar(50),
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
INSERT INTO @tblTempTable ([OrganizationStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', 'Active', '0', '3/8/2016 4:53:49 PM +00:00', '0', '3/8/2016 4:53:49 PM +00:00')
INSERT INTO @tblTempTable ([OrganizationStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', 'Inactive', '0', '3/8/2016 4:53:49 PM +00:00', '0', '3/8/2016 4:53:49 PM +00:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[OrganizationStatus] ON
INSERT INTO [dbo].[OrganizationStatus] ([OrganizationStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[OrganizationStatusId], tmp.[Status], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[OrganizationStatus] tbl ON tbl.[OrganizationStatusId] = tmp.[OrganizationStatusId]
WHERE tbl.[OrganizationStatusId] IS NULL
SET IDENTITY_INSERT [dbo].[OrganizationStatus] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[Status] = tmp.[Status],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [dbo].[OrganizationStatus] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[OrganizationStatusId] = tmp.[OrganizationStatusId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[OrganizationStatus] FROM [dbo].[OrganizationStatus] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[OrganizationStatusId] = tmp.[OrganizationStatusId]
	WHERE tmp.[OrganizationStatusId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[OrganizationStatus]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO