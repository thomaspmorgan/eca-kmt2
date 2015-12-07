/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [sevis].[ExchangeVisitorTerminationReason].

PRINT 'Updating static data table [sevis].[ExchangeVisitorTerminationReason]'

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
[ExchangeVisitorTerminationReasonId] int,
[TerminationCode] nvarchar(20),
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
INSERT INTO @tblTempTable ([ExchangeVisitorTerminationReasonId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', 'CONVIC', 'CONVICTION OF A CRIME', '1', '12/7/2015 12:15:34 PM -05:00', '1', '12/7/2015 12:15:34 PM -05:00')
INSERT INTO @tblTempTable ([ExchangeVisitorTerminationReasonId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', 'DISCIP', 'DISCIPLINARY ACTION', '1', '12/7/2015 12:15:34 PM -05:00', '1', '12/7/2015 12:15:34 PM -05:00')
INSERT INTO @tblTempTable ([ExchangeVisitorTerminationReasonId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', 'ENGEMP', 'ENGAGING IN UNAUTHORIZED EMPLOYMENT', '1', '12/7/2015 12:15:34 PM -05:00', '1', '12/7/2015 12:15:34 PM -05:00')
INSERT INTO @tblTempTable ([ExchangeVisitorTerminationReasonId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', 'FALACT', 'FAILURE TO PURSUE EV PROGRAM ACTIVITIES', '1', '12/7/2015 12:15:34 PM -05:00', '1', '12/7/2015 12:15:34 PM -05:00')
INSERT INTO @tblTempTable ([ExchangeVisitorTerminationReasonId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', 'FALADD', 'FAILURE TO SUBMIT CHANGE OF CURRENT ADDRESS WITHIN 10 DAYS', '1', '12/7/2015 12:15:34 PM -05:00', '1', '12/7/2015 12:15:34 PM -05:00')
INSERT INTO @tblTempTable ([ExchangeVisitorTerminationReasonId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', 'FALINS', 'FAILURE TO MAINTAIN HEALTH INSURANCE', '1', '12/7/2015 12:15:34 PM -05:00', '1', '12/7/2015 12:15:34 PM -05:00')
INSERT INTO @tblTempTable ([ExchangeVisitorTerminationReasonId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', 'FALSTD', 'FAILURE TO MAINTAIN A FULL COURSE OF STUDY', '1', '12/7/2015 12:15:34 PM -05:00', '1', '12/7/2015 12:15:34 PM -05:00')
INSERT INTO @tblTempTable ([ExchangeVisitorTerminationReasonId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('8', 'INVSUS', 'INVOLUNTARY SUSPENSION', '1', '12/7/2015 12:15:34 PM -05:00', '1', '12/7/2015 12:15:34 PM -05:00')
INSERT INTO @tblTempTable ([ExchangeVisitorTerminationReasonId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('9', 'OTHER', 'OTHER', '1', '12/7/2015 12:15:34 PM -05:00', '1', '12/7/2015 12:15:34 PM -05:00')
INSERT INTO @tblTempTable ([ExchangeVisitorTerminationReasonId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('10', 'VIOEXV', 'VIOLATING EXCHANGE VISITOR PROGRAM REGULATIONS', '1', '12/7/2015 12:15:34 PM -05:00', '1', '12/7/2015 12:15:34 PM -05:00')
INSERT INTO @tblTempTable ([ExchangeVisitorTerminationReasonId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('11', 'VIOSPN', 'VIOLATING SPONSOR RULES GOVERNING THE PROGRAM', '1', '12/7/2015 12:15:34 PM -05:00', '1', '12/7/2015 12:15:34 PM -05:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [sevis].[ExchangeVisitorTerminationReason] ON
INSERT INTO [sevis].[ExchangeVisitorTerminationReason] ([ExchangeVisitorTerminationReasonId], [TerminationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[ExchangeVisitorTerminationReasonId], tmp.[TerminationCode], tmp.[Description], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [sevis].[ExchangeVisitorTerminationReason] tbl ON tbl.[ExchangeVisitorTerminationReasonId] = tmp.[ExchangeVisitorTerminationReasonId]
WHERE tbl.[ExchangeVisitorTerminationReasonId] IS NULL
SET IDENTITY_INSERT [sevis].[ExchangeVisitorTerminationReason] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[TerminationCode] = tmp.[TerminationCode],
LiveTable.[Description] = tmp.[Description],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [sevis].[ExchangeVisitorTerminationReason] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[ExchangeVisitorTerminationReasonId] = tmp.[ExchangeVisitorTerminationReasonId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [sevis].[ExchangeVisitorTerminationReason] FROM [sevis].[ExchangeVisitorTerminationReason] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[ExchangeVisitorTerminationReasonId] = tmp.[ExchangeVisitorTerminationReasonId]
	WHERE tmp.[ExchangeVisitorTerminationReasonId] IS NULL
END

PRINT 'Finished updating static data table [sevis].[ExchangeVisitorTerminationReason]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO