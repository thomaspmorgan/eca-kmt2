/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[EmailAddressType].

PRINT 'Updating static data table [dbo].[EmailAddressType]'

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
[EmailAddressTypeId] int,
[EmailAddressTypeName] nvarchar(128),
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
INSERT INTO @tblTempTable ([EmailAddressTypeId], [EmailAddressTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', 'Home', '0', '3/13/2015 12:00:00 AM -05:00', '0', '3/13/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([EmailAddressTypeId], [EmailAddressTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', 'Home Emergency', '0', '3/13/2015 12:00:00 AM -05:00', '0', '3/13/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([EmailAddressTypeId], [EmailAddressTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', 'Host', '0', '3/13/2015 12:00:00 AM -05:00', '0', '3/13/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([EmailAddressTypeId], [EmailAddressTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', 'Host Emergency', '0', '3/13/2015 12:00:00 AM -05:00', '0', '3/13/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([EmailAddressTypeId], [EmailAddressTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', 'Organization', '0', '3/13/2015 12:00:00 AM -05:00', '0', '3/13/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([EmailAddressTypeId], [EmailAddressTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', 'Personal', '0', '3/13/2015 12:00:00 AM -05:00', '0', '3/13/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([EmailAddressTypeId], [EmailAddressTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', 'Other', '0', '3/13/2015 12:00:00 AM -05:00', '0', '3/13/2015 12:00:00 AM -05:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[EmailAddressType] ON
INSERT INTO [dbo].[EmailAddressType] ([EmailAddressTypeId], [EmailAddressTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[EmailAddressTypeId], tmp.[EmailAddressTypeName], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[EmailAddressType] tbl ON tbl.[EmailAddressTypeId] = tmp.[EmailAddressTypeId]
WHERE tbl.[EmailAddressTypeId] IS NULL
SET IDENTITY_INSERT [dbo].[EmailAddressType] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[EmailAddressTypeName] = tmp.[EmailAddressTypeName],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [dbo].[EmailAddressType] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[EmailAddressTypeId] = tmp.[EmailAddressTypeId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[EmailAddressType] FROM [dbo].[EmailAddressType] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[EmailAddressTypeId] = tmp.[EmailAddressTypeId]
	WHERE tmp.[EmailAddressTypeId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[EmailAddressType]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO