/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[ProgramType].

PRINT 'Updating static data table [dbo].[ProgramType]'

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
[ProgramTypeId] int,
[ProgramTypeName] nvarchar(20),
[History_CreatedBy] int,
[History_CreatedOn] datetimeoffset,
[History_RevisedBy] int,
[History_RevisedOn] datetimeoffset,
[Program_ProgramId] int
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.
INSERT INTO @tblTempTable ([ProgramTypeId], [ProgramTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn], [Program_ProgramId]) VALUES ('1', 'Program', '0', '2/22/2015 12:00:00 AM -05:00', '0', '2/22/2015 12:00:00 AM -05:00', NULL)
INSERT INTO @tblTempTable ([ProgramTypeId], [ProgramTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn], [Program_ProgramId]) VALUES ('2', 'Office', '0', '2/22/2015 12:00:00 AM -05:00', '0', '2/22/2015 12:00:00 AM -05:00', NULL)
INSERT INTO @tblTempTable ([ProgramTypeId], [ProgramTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn], [Program_ProgramId]) VALUES ('3', 'Branch', '0', '2/22/2015 12:00:00 AM -05:00', '0', '2/22/2015 12:00:00 AM -05:00', NULL)


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[ProgramType] ON
INSERT INTO [dbo].[ProgramType] ([ProgramTypeId], [ProgramTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn], [Program_ProgramId])
SELECT tmp.[ProgramTypeId], tmp.[ProgramTypeName], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn], tmp.[Program_ProgramId]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[ProgramType] tbl ON tbl.[ProgramTypeId] = tmp.[ProgramTypeId]
WHERE tbl.[ProgramTypeId] IS NULL
SET IDENTITY_INSERT [dbo].[ProgramType] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[ProgramTypeName] = tmp.[ProgramTypeName],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn],
LiveTable.[Program_ProgramId] = tmp.[Program_ProgramId]
FROM [dbo].[ProgramType] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[ProgramTypeId] = tmp.[ProgramTypeId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[ProgramType] FROM [dbo].[ProgramType] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[ProgramTypeId] = tmp.[ProgramTypeId]
	WHERE tmp.[ProgramTypeId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[ProgramType]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO