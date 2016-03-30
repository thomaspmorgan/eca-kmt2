/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[DependentType].

PRINT 'Updating static data table [dbo].[DependentType]'

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
[DependentTypeId] int,
[Name] nvarchar(150),
[SevisDependentTypeCode] NVARCHAR(2) NULL
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.
INSERT INTO @tblTempTable ([DependentTypeId], [Name], [SevisDependentTypeCode]) VALUES ('1', 'Participant', null)
INSERT INTO @tblTempTable ([DependentTypeId], [Name], [SevisDependentTypeCode]) VALUES ('2', 'Spouse', '01')
INSERT INTO @tblTempTable ([DependentTypeId], [Name], [SevisDependentTypeCode]) VALUES ('3', 'Child', '02')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[DependentType] ON
INSERT INTO [dbo].[DependentType] ([DependentTypeId], [Name], [SevisDependentTypeCode])
SELECT tmp.[DependentTypeId], tmp.[Name], tmp.[SevisDependentTypeCode]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[DependentType] tbl ON tbl.[DependentTypeId] = tmp.[DependentTypeId]
WHERE tbl.[DependentTypeId] IS NULL
SET IDENTITY_INSERT [dbo].[DependentType] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[Name] = tmp.[Name],
LiveTable.[SevisDependentTypeCode] = tmp.[SevisDependentTypeCode]
FROM [dbo].[DependentType] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[DependentTypeId] = tmp.[DependentTypeId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[DependentType] FROM [dbo].[DependentType] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[DependentTypeId] = tmp.[DependentTypeId]
	WHERE tmp.[DependentTypeId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[DependentType]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO