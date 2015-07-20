/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[ParticipantStatus].

PRINT 'Updating static data table [dbo].[ParticipantStatus]'

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
[ParticipantStatusId] int,
[Status] nvarchar(50),
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
INSERT INTO @tblTempTable ([ParticipantStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', 'Alumnus', '0', '7/8/2015 3:20:21 PM +00:00', '0', '7/8/2015 3:20:21 PM +00:00')
INSERT INTO @tblTempTable ([ParticipantStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', 'Active', '0', '7/8/2015 3:20:21 PM +00:00', '0', '7/8/2015 3:20:21 PM +00:00')
INSERT INTO @tblTempTable ([ParticipantStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', 'Nominee', '0', '7/8/2015 3:20:21 PM +00:00', '0', '7/8/2015 3:20:21 PM +00:00')
INSERT INTO @tblTempTable ([ParticipantStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', 'Applicant', '0', '7/8/2015 3:20:21 PM +00:00', '0', '7/8/2015 3:20:21 PM +00:00')
INSERT INTO @tblTempTable ([ParticipantStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', 'Withdrawn', '0', '7/8/2015 3:20:21 PM +00:00', '0', '7/8/2015 3:20:21 PM +00:00')
INSERT INTO @tblTempTable ([ParticipantStatusId], [Status, [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', 'Terminated', '0', '7/8/2015 3:20:21 PM +00:00', '0', '7/8/2015 3:20:21 PM +00:00')
INSERT INTO @tblTempTable ([ParticipantStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', 'Rejected', '0', '7/8/2015 3:20:21 PM +00:00', '0', '7/8/2015 3:20:21 PM +00:00')
INSERT INTO @tblTempTable ([ParticipantStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('8', 'Suspended', '0', '7/8/2015 3:20:21 PM +00:00', '0', '7/8/2015 3:20:21 PM +00:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[ParticipantStatus] ON
INSERT INTO [dbo].[ParticipantStatus] ([ParticipantStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[ParticipantStatusId], tmp.[Status], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[ParticipantStatus] tbl ON tbl.[ParticipantStatusId] = tmp.[ParticipantStatusId]
WHERE tbl.[ParticipantStatusId] IS NULL
SET IDENTITY_INSERT [dbo].[ParticipantStatus] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[Status] = tmp.[Status],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [dbo].[ParticipantStatus] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[ParticipantStatusId] = tmp.[ParticipantStatusId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[ParticipantStatus] FROM [dbo].[ParticipantStatus] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[ParticipantStatusId] = tmp.[ParticipantStatusId]
	WHERE tmp.[ParticipantStatusId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[ParticipantStatus]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO