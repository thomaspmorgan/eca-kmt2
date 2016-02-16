/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [cam].[Permission].

PRINT 'Updating static data table [cam].[Permission]'

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
[PermissionId] int,
[PermissionName] nvarchar(50),
[CreatedOn] datetimeoffset,
[CreatedBy] int,
[RevisedOn] datetimeoffset,
[RevisedBy] nvarchar(50),
[IsActive] bit,
[ResourceTypeId] int,
[ParentResourceTypeId] int,
[ResourceId] int,
[PermissionDescription] nchar(255)
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.
INSERT INTO @tblTempTable ([PermissionId], [PermissionName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceTypeId], [ParentResourceTypeId], [ResourceId], [PermissionDescription]) VALUES ('1', 'View Office', '4/14/2015 12:00:00 AM -04:00', '1', '4/14/2015 12:00:00 AM -04:00', '1', 'True', '2', NULL, NULL, 'Can View Office Overview Information.')
INSERT INTO @tblTempTable ([PermissionId], [PermissionName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceTypeId], [ParentResourceTypeId], [ResourceId], [PermissionDescription]) VALUES ('2', 'View Program', '4/14/2015 12:00:00 AM -04:00', '1', '4/14/2015 12:00:00 AM -04:00', '1', 'True', '3', '2', NULL, 'Can View Program Overview Information.')
INSERT INTO @tblTempTable ([PermissionId], [PermissionName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceTypeId], [ParentResourceTypeId], [ResourceId], [PermissionDescription]) VALUES ('3', 'View Project', '4/14/2015 12:00:00 AM -04:00', '1', '4/14/2015 12:00:00 AM -04:00', '1', 'True', '4', '3', NULL, 'Can View Project Overview Information.')
INSERT INTO @tblTempTable ([PermissionId], [PermissionName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceTypeId], [ParentResourceTypeId], [ResourceId], [PermissionDescription]) VALUES ('4', 'Edit Office', '4/14/2015 12:00:00 AM -04:00', '1', '4/14/2015 12:00:00 AM -04:00', '1', 'True', '2', NULL, NULL, 'Can Create/Edit Offices.')
INSERT INTO @tblTempTable ([PermissionId], [PermissionName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceTypeId], [ParentResourceTypeId], [ResourceId], [PermissionDescription]) VALUES ('5', 'Edit Program', '4/14/2015 12:00:00 AM -04:00', '1', '4/14/2015 12:00:00 AM -04:00', '1', 'True', '3', '2', NULL, 'Can Create/Edit Program.')
INSERT INTO @tblTempTable ([PermissionId], [PermissionName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceTypeId], [ParentResourceTypeId], [ResourceId], [PermissionDescription]) VALUES ('7', 'Edit Project', '4/14/2015 12:00:00 AM -04:00', '1', '4/14/2015 12:00:00 AM -04:00', '1', 'True', '4', '3', NULL, 'Can Create/Edit Project.')
INSERT INTO @tblTempTable ([PermissionId], [PermissionName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceTypeId], [ParentResourceTypeId], [ResourceId], [PermissionDescription]) VALUES ('8', 'Project Owner', '4/14/2015 12:00:00 AM -04:00', '1', '4/14/2015 12:00:00 AM -04:00', '1', 'True', '4', '3', NULL, 'Can Assign Collaborators to a Project.')
INSERT INTO @tblTempTable ([PermissionId], [PermissionName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceTypeId], [ParentResourceTypeId], [ResourceId], [PermissionDescription]) VALUES ('9', 'Program Owner', '4/14/2015 12:00:00 AM -04:00', '1', '4/14/2015 12:00:00 AM -04:00', '1', 'True', '3', '2', NULL, 'Can Assign Collaborators to a Program.')
INSERT INTO @tblTempTable ([PermissionId], [PermissionName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceTypeId], [ParentResourceTypeId], [ResourceId], [PermissionDescription]) VALUES ('10', 'Office Owner', '4/14/2015 12:00:00 AM -04:00', '1', '4/14/2015 12:00:00 AM -04:00', '1', 'True', '2', NULL, NULL, 'Can Assign Collaborators to a Office.')
INSERT INTO @tblTempTable ([PermissionId], [PermissionName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceTypeId], [ParentResourceTypeId], [ResourceId], [PermissionDescription]) VALUES ('11', 'Administrator', '4/14/2015 12:00:00 AM -04:00', '1', '4/14/2015 12:00:00 AM -04:00', '1', 'True', '1', NULL, NULL, 'Can administer a web api application.')
INSERT INTO @tblTempTable ([PermissionId], [PermissionName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceTypeId], [ParentResourceTypeId], [ResourceId], [PermissionDescription]) VALUES ('12', 'Search', '4/14/2015 12:00:00 AM -04:00', '1', '4/14/2015 12:00:00 AM -04:00', '1', 'True', '1', NULL, '1', 'Can search the KMT web api application.')
INSERT INTO @tblTempTable ([PermissionId], [PermissionName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceTypeId], [ParentResourceTypeId], [ResourceId], [PermissionDescription]) VALUES ('13', 'Edit Sevis', '4/14/2015 12:00:00 AM -04:00', '1', '4/14/2015 12:00:00 AM -04:00', '1', 'True', '4', '3', NULL, 'Can edit project participant sevis information.')
INSERT INTO @tblTempTable ([PermissionId], [PermissionName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceTypeId], [ParentResourceTypeId], [ResourceId], [PermissionDescription]) VALUES ('14', 'Submit To Sevis', '4/14/2015 12:00:00 AM -04:00', '1', '4/14/2015 12:00:00 AM -04:00', '1', 'True', '4', '3', NULL, 'Can submit project participants to sevis.')

-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [cam].[Permission] ON
INSERT INTO [cam].[Permission] ([PermissionId], [PermissionName], [CreatedOn], [CreatedBy], [RevisedOn], [RevisedBy], [IsActive], [ResourceTypeId], [ParentResourceTypeId], [ResourceId], [PermissionDescription])
SELECT tmp.[PermissionId], tmp.[PermissionName], tmp.[CreatedOn], tmp.[CreatedBy], tmp.[RevisedOn], tmp.[RevisedBy], tmp.[IsActive], tmp.[ResourceTypeId], tmp.[ParentResourceTypeId], tmp.[ResourceId], tmp.[PermissionDescription]
FROM @tblTempTable tmp
LEFT JOIN [cam].[Permission] tbl ON tbl.[PermissionId] = tmp.[PermissionId]
WHERE tbl.[PermissionId] IS NULL
SET IDENTITY_INSERT [cam].[Permission] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[PermissionName] = tmp.[PermissionName],
LiveTable.[CreatedOn] = tmp.[CreatedOn],
LiveTable.[CreatedBy] = tmp.[CreatedBy],
LiveTable.[RevisedOn] = tmp.[RevisedOn],
LiveTable.[RevisedBy] = tmp.[RevisedBy],
LiveTable.[IsActive] = tmp.[IsActive],
LiveTable.[ResourceTypeId] = tmp.[ResourceTypeId],
LiveTable.[ParentResourceTypeId] = tmp.[ParentResourceTypeId],
LiveTable.[ResourceId] = tmp.[ResourceId],
LiveTable.[PermissionDescription] = tmp.[PermissionDescription]
FROM [cam].[Permission] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[PermissionId] = tmp.[PermissionId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [cam].[Permission] FROM [cam].[Permission] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[PermissionId] = tmp.[PermissionId]
	WHERE tmp.[PermissionId] IS NULL
END

PRINT 'Finished updating static data table [cam].[Permission]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO