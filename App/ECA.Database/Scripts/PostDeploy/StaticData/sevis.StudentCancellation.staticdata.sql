/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [sevis].[StudentCancellation].

PRINT 'Updating static data table [sevis].[StudentCancellation]'

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
[StudentCancellationId] int,
[CancellationCode] char(2),
[Reason] nvarchar(100),
[History_CreateBy] int,
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
INSERT INTO @tblTempTable ([StudentCancellationId], [CancellationCode], [Reason], [History_CreateBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', '01', 'Student not attending', '0', '5/29/2015 9:50:03 AM -04:00', '0', '5/29/2015 9:50:03 AM -04:00')
INSERT INTO @tblTempTable ([StudentCancellationId], [CancellationCode], [Reason], [History_CreateBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', '02', 'Visa Issued for different SEVIS ID', '0', '5/29/2015 9:50:03 AM -04:00', '0', '5/29/2015 9:50:03 AM -04:00')
INSERT INTO @tblTempTable ([StudentCancellationId], [CancellationCode], [Reason], [History_CreateBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', '03', 'Student registered under different SEVIS ID', '0', '5/29/2015 9:50:03 AM -04:00', '0', '5/29/2015 9:50:03 AM -04:00')
INSERT INTO @tblTempTable ([StudentCancellationId], [CancellationCode], [Reason], [History_CreateBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', '04', 'Student arrived under different SEVIS ID', '0', '5/29/2015 9:50:03 AM -04:00', '0', '5/29/2015 9:50:03 AM -04:00')
INSERT INTO @tblTempTable ([StudentCancellationId], [CancellationCode], [Reason], [History_CreateBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', '05', 'Record Created in error', '0', '5/29/2015 9:50:03 AM -04:00', '0', '5/29/2015 9:50:03 AM -04:00')
INSERT INTO @tblTempTable ([StudentCancellationId], [CancellationCode], [Reason], [History_CreateBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', '06', 'Offer Withdrawn', '0', '5/29/2015 9:50:03 AM -04:00', '0', '5/29/2015 9:50:03 AM -04:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [sevis].[StudentCancellation] ON
INSERT INTO [sevis].[StudentCancellation] ([StudentCancellationId], [CancellationCode], [Reason], [History_CreateBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[StudentCancellationId], tmp.[CancellationCode], tmp.[Reason], tmp.[History_CreateBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [sevis].[StudentCancellation] tbl ON tbl.[StudentCancellationId] = tmp.[StudentCancellationId]
WHERE tbl.[StudentCancellationId] IS NULL
SET IDENTITY_INSERT [sevis].[StudentCancellation] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[CancellationCode] = tmp.[CancellationCode],
LiveTable.[Reason] = tmp.[Reason],
LiveTable.[History_CreateBy] = tmp.[History_CreateBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [sevis].[StudentCancellation] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[StudentCancellationId] = tmp.[StudentCancellationId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [sevis].[StudentCancellation] FROM [sevis].[StudentCancellation] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[StudentCancellationId] = tmp.[StudentCancellationId]
	WHERE tmp.[StudentCancellationId] IS NULL
END

PRINT 'Finished updating static data table [sevis].[StudentCancellation]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO