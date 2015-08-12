/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[LocationType].

PRINT 'Updating static data table [dbo].[LocationType]'

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
[LocationTypeId] int,
[LocationTypeName] nvarchar(MAX),
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
INSERT INTO @tblTempTable ([LocationTypeId], [LocationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', 'Region', '0', '1/19/2015 3:20:21 PM +00:00', '0', '1/19/2015 3:20:21 PM +00:00')
INSERT INTO @tblTempTable ([LocationTypeId], [LocationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', 'Country', '0', '1/19/2015 3:20:21 PM +00:00', '0', '1/19/2015 3:20:21 PM +00:00')
INSERT INTO @tblTempTable ([LocationTypeId], [LocationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', 'Division', '0', '1/19/2015 3:20:21 PM +00:00', '0', '1/19/2015 3:20:21 PM +00:00')
INSERT INTO @tblTempTable ([LocationTypeId], [LocationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', 'City', '0', '1/19/2015 3:20:21 PM +00:00', '0', '1/19/2015 3:20:21 PM +00:00')
INSERT INTO @tblTempTable ([LocationTypeId], [LocationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', 'Building', '0', '1/19/2015 3:20:21 PM +00:00', '0', '1/19/2015 3:20:21 PM +00:00')
INSERT INTO @tblTempTable ([LocationTypeId], [LocationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', 'Post', '0', '1/28/2015 7:39:36 PM +00:00', '0', '1/28/2015 7:39:36 PM +00:00')
INSERT INTO @tblTempTable ([LocationTypeId], [LocationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('8', 'Place', '0', '1/30/2015 3:34:32 PM +00:00', '0', '1/30/2015 3:34:32 PM +00:00')
INSERT INTO @tblTempTable ([LocationTypeId], [LocationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('9', 'Address', '0', '1/30/2015 3:34:32 PM +00:00', '0', '1/30/2015 3:34:32 PM +00:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[LocationType] ON
INSERT INTO [dbo].[LocationType] ([LocationTypeId], [LocationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[LocationTypeId], tmp.[LocationTypeName], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[LocationType] tbl ON tbl.[LocationTypeId] = tmp.[LocationTypeId]
WHERE tbl.[LocationTypeId] IS NULL
SET IDENTITY_INSERT [dbo].[LocationType] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[LocationTypeName] = tmp.[LocationTypeName],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [dbo].[LocationType] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[LocationTypeId] = tmp.[LocationTypeId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[LocationType] FROM [dbo].[LocationType] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[LocationTypeId] = tmp.[LocationTypeId]
	WHERE tmp.[LocationTypeId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[LocationType]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO