/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [sevis].[EmploymentTime].

PRINT 'Updating static data table [sevis].[EmploymentTime]'

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
[EmploymentTimeId] int,
[EmploymentTimeCode] char(2),
[Description] nvarchar(50),
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
INSERT INTO @tblTempTable ([EmploymentTimeId], [EmploymentTimeCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', '01', 'FULL TIME', '1', '12/7/2015 12:10:39 PM -05:00', '1', '12/7/2015 12:10:39 PM -05:00')
INSERT INTO @tblTempTable ([EmploymentTimeId], [EmploymentTimeCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', '02', 'PART TIME', '1', '12/7/2015 12:10:39 PM -05:00', '1', '12/7/2015 12:10:39 PM -05:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [sevis].[EmploymentTime] ON
INSERT INTO [sevis].[EmploymentTime] ([EmploymentTimeId], [EmploymentTimeCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[EmploymentTimeId], tmp.[EmploymentTimeCode], tmp.[Description], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [sevis].[EmploymentTime] tbl ON tbl.[EmploymentTimeId] = tmp.[EmploymentTimeId]
WHERE tbl.[EmploymentTimeId] IS NULL
SET IDENTITY_INSERT [sevis].[EmploymentTime] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[EmploymentTimeCode] = tmp.[EmploymentTimeCode],
LiveTable.[Description] = tmp.[Description],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [sevis].[EmploymentTime] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[EmploymentTimeId] = tmp.[EmploymentTimeId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [sevis].[EmploymentTime] FROM [sevis].[EmploymentTime] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[EmploymentTimeId] = tmp.[EmploymentTimeId]
	WHERE tmp.[EmploymentTimeId] IS NULL
END

PRINT 'Finished updating static data table [sevis].[EmploymentTime]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO