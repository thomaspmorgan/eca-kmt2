﻿/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [CAM].[PrincipalType].

PRINT 'Updating static data table [CAM].[PrincipalType]'

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
[PrincipalTypeId] int,
[PrincipalTypeName] nvarchar(50),
[PrincipalTypeDescription] nvarchar(255),
[CreatedBy] int,
[CreatedOn] datetimeoffset,
[RevisedBy] int,
[RevisedOn] datetimeoffset,
[IsActive] bit
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.
INSERT INTO @tblTempTable ([PrincipalTypeId], [PrincipalTypeName], [CreatedBy], [CreatedOn], [RevisedBy], [RevisedOn], [IsActive]) VALUES (1, 'Person', 0, '4/14/2015 12:00:00 AM -05:00', 0, '4/14/2015 12:00:00 AM -05:00',1)
INSERT INTO @tblTempTable ([PrincipalTypeId], [PrincipalTypeName], [CreatedBy], [CreatedOn], [RevisedBy], [RevisedOn], [IsActive]) VALUES (2, 'Group', 0, '4/14/2015 12:00:00 AM -05:00', 0, '4/14/2015 12:00:00 AM -05:00',1)


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [CAM].[PrincipalType] ON
INSERT INTO [CAM].[PrincipalType] ([PrincipalTypeId], [PrincipalTypeName], [CreatedBy], [CreatedOn], [RevisedBy], [RevisedOn])
SELECT tmp.[PrincipalTypeId], tmp.[PrincipalTypeName], tmp.[CreatedBy], tmp.[CreatedOn], tmp.[RevisedBy], tmp.[RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [CAM].[PrincipalType] tbl ON tbl.[PrincipalTypeId] = tmp.[PrincipalTypeId]
WHERE tbl.[PrincipalTypeId] IS NULL
SET IDENTITY_INSERT [CAM].[PrincipalType] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[PrincipalTypeName] = tmp.[PrincipalTypeName],
LiveTable.[CreatedBy] = tmp.[CreatedBy],
LiveTable.[CreatedOn] = tmp.[CreatedOn],
LiveTable.[RevisedBy] = tmp.[RevisedBy],
LiveTable.[RevisedOn] = tmp.[RevisedOn]
FROM [CAM].[PrincipalType] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[PrincipalTypeId] = tmp.[PrincipalTypeId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [CAM].[PrincipalType] FROM [CAM].[PrincipalType] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[PrincipalTypeId] = tmp.[PrincipalTypeId]
	WHERE tmp.[PrincipalTypeId] IS NULL
END

PRINT 'Finished updating static data table [CAM].[PrincipalType]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO