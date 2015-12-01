/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[MoneyFlowSourceRecipientType].

PRINT 'Updating static data table [dbo].[MoneyFlowSourceRecipientType]'

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
[MoneyFlowSourceRecipientTypeId] int,
[TypeName] nvarchar(20),
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
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [TypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', 'Organization', '0', '2/17/2015 12:00:00 AM -05:00', '0', '2/17/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [TypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', 'Program', '0', '2/17/2015 12:00:00 AM -05:00', '0', '2/17/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [TypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', 'Project', '0', '2/17/2015 12:00:00 AM -05:00', '0', '2/17/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [TypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', 'Participant', '0', '2/17/2015 12:00:00 AM -05:00', '0', '2/17/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [TypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', 'Travel Stop', '0', '2/17/2015 12:00:00 AM -05:00', '0', '2/17/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [TypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', 'Accomodation', '0', '2/17/2015 12:00:00 AM -05:00', '0', '2/17/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [TypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', 'Transportation', '0', '2/17/2015 12:00:00 AM -05:00', '0', '2/17/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [TypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('8', 'Expense', '0', '2/17/2015 12:00:00 AM -05:00', '0', '2/17/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [TypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('9', 'Post', '0', '2/22/2015 12:00:00 AM -05:00', '0', '2/22/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [TypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('10', 'Office', '0', '2/22/2015 12:00:00 AM -05:00', '0', '2/22/2015 12:00:00 AM -05:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[MoneyFlowSourceRecipientType] ON
INSERT INTO [dbo].[MoneyFlowSourceRecipientType] ([MoneyFlowSourceRecipientTypeId], [TypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[MoneyFlowSourceRecipientTypeId], tmp.[TypeName], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[MoneyFlowSourceRecipientType] tbl ON tbl.[MoneyFlowSourceRecipientTypeId] = tmp.[MoneyFlowSourceRecipientTypeId]
WHERE tbl.[MoneyFlowSourceRecipientTypeId] IS NULL
SET IDENTITY_INSERT [dbo].[MoneyFlowSourceRecipientType] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[TypeName] = tmp.[TypeName],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [dbo].[MoneyFlowSourceRecipientType] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[MoneyFlowSourceRecipientTypeId] = tmp.[MoneyFlowSourceRecipientTypeId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[MoneyFlowSourceRecipientType] FROM [dbo].[MoneyFlowSourceRecipientType] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[MoneyFlowSourceRecipientTypeId] = tmp.[MoneyFlowSourceRecipientTypeId]
	WHERE tmp.[MoneyFlowSourceRecipientTypeId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[MoneyFlowSourceRecipientType]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO