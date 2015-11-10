/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [cam].[Role].

PRINT 'Updating static data table [cam].[Role]'

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
[RoleId] int,
[RoleName] nvarchar(50),
[RoleDescription] nchar(255),
[CreatedOn] datetimeoffset,
[CreatedBy] int,
[RevisedOn] datetimeoffset,
[RevisedBy] int,
[IsActive] bit,
[ResourceId] int,
[ResourceTypeId] int
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.
INSERT INTO @tblTempTable ([RoleId], [RoleName], [RoleDescription], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceId], [ResourceTypeId]) VALUES ('1', 'KMT Super User', NULL, '4/14/2014 12:00:00 AM -04:00', '1', '4/14/2014 12:00:00 AM -04:00', '1', 'True', NULL, NULL)
INSERT INTO @tblTempTable ([RoleId], [RoleName], [RoleDescription], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceId], [ResourceTypeId]) VALUES ('4', 'Cultural Heritage Program Manager', NULL, '4/14/2014 12:00:00 AM -04:00', '1', '4/14/2014 12:00:00 AM -04:00', '1', 'True', NULL, NULL)
INSERT INTO @tblTempTable ([RoleId], [RoleName], [RoleDescription], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceId], [ResourceTypeId]) VALUES ('5', 'CH Project Manager', NULL, '4/14/2014 12:00:00 AM -04:00', '1', '4/14/2014 12:00:00 AM -04:00', '1', 'True', NULL, NULL)


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [cam].[Role] ON
INSERT INTO [cam].[Role] ([RoleId], [RoleName], [RoleDescription], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceId], [ResourceTypeId])
SELECT tmp.[RoleId], tmp.[RoleName], tmp.[RoleDescription], tmp.[CreatedOn], tmp.[CreatedBy], tmp.[RevisedOn], tmp.[RevisedBy], tmp.[IsActive], tmp.[ResourceId], tmp.[ResourceTypeId]
FROM @tblTempTable tmp
LEFT JOIN [cam].[Role] tbl ON tbl.[RoleId] = tmp.[RoleId]
WHERE tbl.[RoleId] IS NULL
SET IDENTITY_INSERT [cam].[Role] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[RoleName] = tmp.[RoleName],
LiveTable.[RoleDescription] = tmp.[RoleDescription],
LiveTable.[CreatedOn] = tmp.[CreatedOn],
LiveTable.[CreatedBy] = tmp.[CreatedBy],
LiveTable.[RevisedOn] = tmp.[RevisedOn],
LiveTable.[RevisedBy] = tmp.[RevisedBy],
LiveTable.[IsActive] = tmp.[IsActive],
LiveTable.[ResourceId] = tmp.[ResourceId],
LiveTable.[ResourceTypeId] = tmp.[ResourceTypeId]
FROM [cam].[Role] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[RoleId] = tmp.[RoleId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [cam].[Role] FROM [cam].[Role] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[RoleId] = tmp.[RoleId]
	WHERE tmp.[RoleId] IS NULL
END

PRINT 'Finished updating static data table [cam].[Role]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO