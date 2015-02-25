/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[ParticipantType].

PRINT 'Updating static data table [dbo].[ParticipantType]'

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
[ParticipantTypeId] int,
[Name] nvarchar(MAX)
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.
INSERT INTO @tblTempTable ([ParticipantTypeId], [Name]) VALUES ('1', 'Foreign Educational Institution')
INSERT INTO @tblTempTable ([ParticipantTypeId], [Name]) VALUES ('2', 'Foreign Government')
INSERT INTO @tblTempTable ([ParticipantTypeId], [Name]) VALUES ('3', 'U.S. Educational Institution')
INSERT INTO @tblTempTable ([ParticipantTypeId], [Name]) VALUES ('4', 'Public International Organization (PIO)')
INSERT INTO @tblTempTable ([ParticipantTypeId], [Name]) VALUES ('5', 'U.S. Non-Profit Organization (501(c)(3))')
INSERT INTO @tblTempTable ([ParticipantTypeId], [Name]) VALUES ('6', 'Individual')
INSERT INTO @tblTempTable ([ParticipantTypeId], [Name]) VALUES ('7', 'Foreign NGO/PVO')
INSERT INTO @tblTempTable ([ParticipantTypeId], [Name]) VALUES ('8', 'Other')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[ParticipantType] ON
INSERT INTO [dbo].[ParticipantType] ([ParticipantTypeId], [Name])
SELECT tmp.[ParticipantTypeId], tmp.[Name]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[ParticipantType] tbl ON tbl.[ParticipantTypeId] = tmp.[ParticipantTypeId]
WHERE tbl.[ParticipantTypeId] IS NULL
SET IDENTITY_INSERT [dbo].[ParticipantType] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[Name] = tmp.[Name]
FROM [dbo].[ParticipantType] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[ParticipantTypeId] = tmp.[ParticipantTypeId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[ParticipantType] FROM [dbo].[ParticipantType] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[ParticipantTypeId] = tmp.[ParticipantTypeId]
	WHERE tmp.[ParticipantTypeId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[ParticipantType]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO