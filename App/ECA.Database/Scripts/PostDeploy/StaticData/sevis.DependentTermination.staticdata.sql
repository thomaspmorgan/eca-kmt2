/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [sevis].[DependentTermination].

PRINT 'Updating static data table [sevis].[DependentTermination]'

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
[DependentTerminationId] int,
[TerminationCode] char(2),
[Description] nvarchar(100),
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
INSERT INTO @tblTempTable ([DependentTerminationId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', '02', 'DEATH', '1', '12/7/2015 12:08:35 PM -05:00', '1', '12/7/2015 12:08:35 PM -05:00')
INSERT INTO @tblTempTable ([DependentTerminationId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', '03', 'CHILD OVER 21', '1', '12/7/2015 12:08:35 PM -05:00', '1', '12/7/2015 12:08:35 PM -05:00')
INSERT INTO @tblTempTable ([DependentTerminationId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', '04', 'DIVORCE', '1', '12/7/2015 12:08:35 PM -05:00', '1', '12/7/2015 12:08:35 PM -05:00')
INSERT INTO @tblTempTable ([DependentTerminationId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', '05', 'UNAUTHORIZED EMPLOYMENT', '1', '12/7/2015 12:08:35 PM -05:00', '1', '12/7/2015 12:08:35 PM -05:00')
INSERT INTO @tblTempTable ([DependentTerminationId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', '06', 'PRINCIPAL STATUS TERMINATED', '1', '12/7/2015 12:08:35 PM -05:00', '1', '12/7/2015 12:08:35 PM -05:00')
INSERT INTO @tblTempTable ([DependentTerminationId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', '09', 'OTHER', '1', '12/7/2015 12:08:35 PM -05:00', '1', '12/7/2015 12:08:35 PM -05:00')
INSERT INTO @tblTempTable ([DependentTerminationId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', '10', 'PRINCIPAL STATUS COMPLETED', '1', '12/7/2015 12:08:35 PM -05:00', '1', '12/7/2015 12:08:35 PM -05:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [sevis].[DependentTermination] ON
INSERT INTO [sevis].[DependentTermination] ([DependentTerminationId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[DependentTerminationId], tmp.[TerminationCode], tmp.[Description], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [sevis].[DependentTermination] tbl ON tbl.[DependentTerminationId] = tmp.[DependentTerminationId]
WHERE tbl.[DependentTerminationId] IS NULL
SET IDENTITY_INSERT [sevis].[DependentTermination] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[TerminationCode] = tmp.[TerminationCode],
LiveTable.[Description] = tmp.[Description],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [sevis].[DependentTermination] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[DependentTerminationId] = tmp.[DependentTerminationId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [sevis].[DependentTermination] FROM [sevis].[DependentTermination] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[DependentTerminationId] = tmp.[DependentTerminationId]
	WHERE tmp.[DependentTerminationId] IS NULL
END

PRINT 'Finished updating static data table [sevis].[DependentTermination]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO