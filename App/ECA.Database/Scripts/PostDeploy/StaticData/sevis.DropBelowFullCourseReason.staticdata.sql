/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [sevis].[DropBelowFullCourseReason].

PRINT 'Updating static data table [sevis].[DropBelowFullCourseReason]'

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
[DropBelowFullCourseReasonId] int,
[ReasonCode] char(2),
[Description] nvarchar(250),
[F_1_Ind] bit,
[M_1_Ind] bit,
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
INSERT INTO @tblTempTable ([DropBelowFullCourseReasonId], [ReasonCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', '01', 'ILLNESS OR MEDICAL CONDITION', 'True', 'True', '1', '12/7/2015 12:09:41 PM -05:00', '1', '12/7/2015 12:09:41 PM -05:00')
INSERT INTO @tblTempTable ([DropBelowFullCourseReasonId], [ReasonCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', '02', 'INITIAL DIFFICULTY WITH THE ENGLISH LANGUAGE', 'True', 'False', '1', '12/7/2015 12:09:41 PM -05:00', '1', '12/7/2015 12:09:41 PM -05:00')
INSERT INTO @tblTempTable ([DropBelowFullCourseReasonId], [ReasonCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', '03', 'INITIAL DIFFICULTY WITH READING REQUIREMENTS', 'True', 'False', '1', '12/7/2015 12:09:41 PM -05:00', '1', '12/7/2015 12:09:41 PM -05:00')
INSERT INTO @tblTempTable ([DropBelowFullCourseReasonId], [ReasonCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', '04', 'UNFAMILIARITY WITH AMERICAN TEACHING METHODS', 'True', 'False', '1', '12/7/2015 12:09:41 PM -05:00', '1', '12/7/2015 12:09:41 PM -05:00')
INSERT INTO @tblTempTable ([DropBelowFullCourseReasonId], [ReasonCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', '05', 'IMPROPER COURSE LEVEL PLACEMENT', 'True', 'False', '1', '12/7/2015 12:09:41 PM -05:00', '1', '12/7/2015 12:09:41 PM -05:00')
INSERT INTO @tblTempTable ([DropBelowFullCourseReasonId], [ReasonCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', '06', 'TO COMPLETE COURSE OF STUDY IN CURRENT TERM', 'True', 'False', '1', '12/7/2015 12:09:41 PM -05:00', '1', '12/7/2015 12:09:41 PM -05:00')
INSERT INTO @tblTempTable ([DropBelowFullCourseReasonId], [ReasonCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', '07', 'PART-TIME COMMUTER STUDENT', 'True', 'True', '1', '12/7/2015 12:09:41 PM -05:00', '1', '12/7/2015 12:09:41 PM -05:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [sevis].[DropBelowFullCourseReason] ON
INSERT INTO [sevis].[DropBelowFullCourseReason] ([DropBelowFullCourseReasonId], [ReasonCode], [Description], [F_1_Ind], [M_1_Ind], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[DropBelowFullCourseReasonId], tmp.[ReasonCode], tmp.[Description], tmp.[F_1_Ind], tmp.[M_1_Ind], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [sevis].[DropBelowFullCourseReason] tbl ON tbl.[DropBelowFullCourseReasonId] = tmp.[DropBelowFullCourseReasonId]
WHERE tbl.[DropBelowFullCourseReasonId] IS NULL
SET IDENTITY_INSERT [sevis].[DropBelowFullCourseReason] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[ReasonCode] = tmp.[ReasonCode],
LiveTable.[Description] = tmp.[Description],
LiveTable.[F_1_Ind] = tmp.[F_1_Ind],
LiveTable.[M_1_Ind] = tmp.[M_1_Ind],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [sevis].[DropBelowFullCourseReason] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[DropBelowFullCourseReasonId] = tmp.[DropBelowFullCourseReasonId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [sevis].[DropBelowFullCourseReason] FROM [sevis].[DropBelowFullCourseReason] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[DropBelowFullCourseReasonId] = tmp.[DropBelowFullCourseReasonId]
	WHERE tmp.[DropBelowFullCourseReasonId] IS NULL
END

PRINT 'Finished updating static data table [sevis].[DropBelowFullCourseReason]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO