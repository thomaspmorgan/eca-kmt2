/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [sevis].[EndProgramReason].

PRINT 'Updating static data table [sevis].[EndProgramReason]'

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
[EndProgramReasonId] int,
[ReasonCode] nvarchar(10),
[Description] nvarchar(200),
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
INSERT INTO @tblTempTable ([EndProgramReasonId], [ReasonCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', 'COMP', 'COMPLETED', '0', '12/7/2015 1:02:45 PM -05:00', '0', '12/7/2015 1:02:45 PM -05:00')
INSERT INTO @tblTempTable ([EndProgramReasonId], [ReasonCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', 'DOE', 'DEATH OF EV', '0', '12/7/2015 1:02:45 PM -05:00', '0', '12/7/2015 1:02:45 PM -05:00')
INSERT INTO @tblTempTable ([EndProgramReasonId], [ReasonCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', 'ICP', 'INABILITY TO CONTINUE PROGRAM', '0', '12/7/2015 1:02:45 PM -05:00', '0', '12/7/2015 1:02:45 PM -05:00')
INSERT INTO @tblTempTable ([EndProgramReasonId], [ReasonCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', 'PCP', 'PROGRAM COMPLETED 30 OR MORE DAYS BEFORE PROGRAM END DATE', '0', '12/7/2015 1:02:45 PM -05:00', '0', '12/7/2015 1:02:45 PM -05:00')
INSERT INTO @tblTempTable ([EndProgramReasonId], [ReasonCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', 'WFP', 'WITHDRAWAL FROM PROGRAM', '0', '12/7/2015 1:02:45 PM -05:00', '0', '12/7/2015 1:02:45 PM -05:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [sevis].[EndProgramReason] ON
INSERT INTO [sevis].[EndProgramReason] ([EndProgramReasonId], [ReasonCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[EndProgramReasonId], tmp.[ReasonCode], tmp.[Description], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [sevis].[EndProgramReason] tbl ON tbl.[EndProgramReasonId] = tmp.[EndProgramReasonId]
WHERE tbl.[EndProgramReasonId] IS NULL
SET IDENTITY_INSERT [sevis].[EndProgramReason] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[ReasonCode] = tmp.[ReasonCode],
LiveTable.[Description] = tmp.[Description],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [sevis].[EndProgramReason] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[EndProgramReasonId] = tmp.[EndProgramReasonId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [sevis].[EndProgramReason] FROM [sevis].[EndProgramReason] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[EndProgramReasonId] = tmp.[EndProgramReasonId]
	WHERE tmp.[EndProgramReasonId] IS NULL
END

PRINT 'Finished updating static data table [sevis].[EndProgramReason]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO