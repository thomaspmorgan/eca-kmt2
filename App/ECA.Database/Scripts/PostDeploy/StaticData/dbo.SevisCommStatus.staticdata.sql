/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[SevisCommStatus].

PRINT 'Updating static data table [dbo].[SevisCommStatus]'

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
[SevisCommStatusId] int,
[SevisCommStatusName] nvarchar(50),
[History_CreatedOn] datetimeoffset,
[History_CreatedBy] int,
[History_RevisedOn] datetimeoffset,
[History_RevisedBy] int,
[IsActive] bit
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('1', 'Information Required', '2/17/2015 12:00:00 AM -05:00', '0', '2/17/2015 12:00:00 AM -05:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('2', 'Ready To Submit', '2/17/2015 12:00:00 AM -05:00', '0', '2/17/2015 12:00:00 AM -05:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('3', 'Sent To DHS', '2/17/2015 12:00:00 AM -05:00', '0', '2/17/2015 12:00:00 AM -05:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('4', 'Validated', '2/17/2015 12:00:00 AM -05:00', '0', '2/17/2015 12:00:00 AM -05:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('5', 'Queued To Submit', '2/17/2015 12:00:00 AM -05:00', '0', '2/17/2015 12:00:00 AM -05:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('6', 'Sent to DHS via RTI', '12/15/2015 10:46:07 PM +00:00', '0', '12/15/2015 10:46:07 PM +00:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('7', 'Cancelled', '12/15/2015 10:46:07 PM +00:00', '0', '12/15/2015 10:46:07 PM +00:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('8', 'DS-2019 Signed', '12/15/2015 10:46:07 PM +00:00', '0', '12/15/2015 10:46:07 PM +00:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('9', 'DS-2019 Printed', '12/15/2015 10:46:07 PM +00:00', '0', '12/15/2015 10:46:07 PM +00:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('10', 'RTI Request Successful', '12/15/2015 10:46:07 PM +00:00', '0', '12/15/2015 10:46:07 PM +00:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('11', 'RTI Request Unsuccessful', '12/15/2015 10:46:07 PM +00:00', '0', '12/15/2015 10:46:07 PM +00:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('12', 'Form I-515A Issued', '12/15/2015 10:46:07 PM +00:00', '0', '12/15/2015 10:46:07 PM +00:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('13', 'Pending Sevis Send', '12/15/2015 10:46:07 PM +00:00', '0', '12/15/2015 10:46:07 PM +00:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('14', 'Sent By Batch', '12/15/2015 10:46:07 PM +00:00', '0', '12/15/2015 10:46:07 PM +00:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('15', 'Created By Batch', '12/15/2015 10:46:07 PM +00:00', '0', '12/15/2015 10:46:07 PM +00:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('16', 'Validated By Batch', '12/15/2015 10:46:07 PM +00:00', '0', '12/15/2015 10:46:07 PM +00:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('17', 'Batch Cancelled by System', '12/15/2015 10:46:07 PM +00:00', '0', '12/15/2015 10:46:07 PM +00:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('18', 'Needs Validation Info', '12/15/2015 10:46:07 PM +00:00', '0', '12/15/2015 10:46:07 PM +00:00', '0', 'True')
INSERT INTO @tblTempTable ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive]) VALUES ('19', 'Ready to Validate', '12/15/2015 10:46:07 PM +00:00', '0', '12/15/2015 10:46:07 PM +00:00', '0', 'True')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[SevisCommStatus] ON
INSERT INTO [dbo].[SevisCommStatus] ([SevisCommStatusId], [SevisCommStatusName], [History_CreatedOn], [History_CreatedBy], [History_RevisedOn], [History_RevisedBy], [IsActive])
SELECT tmp.[SevisCommStatusId], tmp.[SevisCommStatusName], tmp.[History_CreatedOn], tmp.[History_CreatedBy], tmp.[History_RevisedOn], tmp.[History_RevisedBy], tmp.[IsActive]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[SevisCommStatus] tbl ON tbl.[SevisCommStatusId] = tmp.[SevisCommStatusId]
WHERE tbl.[SevisCommStatusId] IS NULL
SET IDENTITY_INSERT [dbo].[SevisCommStatus] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[SevisCommStatusName] = tmp.[SevisCommStatusName],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[IsActive] = tmp.[IsActive]
FROM [dbo].[SevisCommStatus] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[SevisCommStatusId] = tmp.[SevisCommStatusId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[SevisCommStatus] FROM [dbo].[SevisCommStatus] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[SevisCommStatusId] = tmp.[SevisCommStatusId]
	WHERE tmp.[SevisCommStatusId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[SevisCommStatus]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO