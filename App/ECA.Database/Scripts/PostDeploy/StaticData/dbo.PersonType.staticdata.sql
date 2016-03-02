/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[PersonType].

PRINT 'Updating static data table [dbo].[PersonType]'

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
[PersonTypeId] int,
[Name] nvarchar(150),
[SevisDependentTypeCode] NVARCHAR(2) NULL,
[IsDependentPersonType] BIT NOT NULL 
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.
INSERT INTO @tblTempTable ([PersonTypeId], [Name], [SevisDependentTypeCode], [IsDependentPersonType]) VALUES ('1', 'Participant', null, 'False')
INSERT INTO @tblTempTable ([PersonTypeId], [Name], [SevisDependentTypeCode], [IsDependentPersonType]) VALUES ('2', 'Spouse', '01', 'True')
INSERT INTO @tblTempTable ([PersonTypeId], [Name], [SevisDependentTypeCode], [IsDependentPersonType]) VALUES ('3', 'Child', '02', 'True')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[PersonType] ON
INSERT INTO [dbo].[PersonType] ([PersonTypeId], [Name], [SevisDependentTypeCode], [IsDependentPersonType])
SELECT tmp.[PersonTypeId], tmp.[Name], tmp.[SevisDependentTypeCode], tmp.[IsDependentPersonType]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[PersonType] tbl ON tbl.[PersonTypeId] = tmp.[PersonTypeId]
WHERE tbl.[PersonTypeId] IS NULL
SET IDENTITY_INSERT [dbo].[PersonType] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[Name] = tmp.[Name],
LiveTable.[SevisDependentTypeCode] = tmp.[SevisDependentTypeCode],
LiveTable.[IsDependentPersonType] = tmp.[IsDependentPersonType]
FROM [dbo].[PersonType] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[PersonTypeId] = tmp.[PersonTypeId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[PersonType] FROM [dbo].[PersonType] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[PersonTypeId] = tmp.[PersonTypeId]
	WHERE tmp.[PersonTypeId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[PersonType]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO