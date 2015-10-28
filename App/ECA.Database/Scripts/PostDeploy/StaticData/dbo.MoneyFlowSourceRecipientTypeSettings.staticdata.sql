/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[MoneyFlowSourceRecipientTypeSettings].

PRINT 'Updating static data table [dbo].[MoneyFlowSourceRecipientTypeSettings]'

-- Set date format to ensure text dates are parsed correctly
SET DATEFORMAT ymd

-- Turn off affected rows being returned
SET NOCOUNT ON

-- Change this to 1 to delete missing records in the target
-- WARNING: Setting this to 1 can cause damage to your database
-- and cause failed deployment if there are any rows referencing
-- a record which has been deleted.
DECLARE @DeleteMissingRecords BIT
SET @DeleteMissingRecords = 1

-- 1: Define table variable
DECLARE @tblTempTable TABLE (
[MoneyFlowSourceRecipientTypeId] int,
[PeerMoneyFlowSourceRecipientTypeId] int,
[IsSource] bit,
[IsRecipient] bit
)

-- 2: Populate the table variable with data
-- This is where you manage your data in source control. You
-- can add and modify entries, but because of potential foreign
-- key contraint violations this script will not delete any
-- removed entries. If you remove an entry then it will no longer
-- be added to new databases based on your schema, but the entry
-- will not be deleted from databases in which the value already exists.

--Read this as follows to help figure out new values...
--From the organization perspective, a program is NOT a source, it IS a recipient
--therefore, 1, 2, false, true
--therefore -> INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('1', '2', 'False', 'True')

INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('1', '2', 'True', 'True')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('1', '3', 'True', 'True')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('1', '8', 'False', 'True')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('1', '10', 'True', 'True')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('2', '1', 'True', 'True')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('2', '2', 'True', 'True')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('2', '3', 'False', 'True')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('2', '10', 'True', 'False')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('3', '1', 'True', 'True')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('3', '2', 'True', 'False')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('3', '4', 'False', 'True')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('3', '8', 'False', 'True')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('10', '1', 'True', 'True')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('10', '2', 'False', 'True')
INSERT INTO @tblTempTable ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient]) VALUES ('10', '10', 'True', 'True')


-- 3: Insert any new items into the table from the table variable
INSERT INTO [dbo].[MoneyFlowSourceRecipientTypeSettings] ([MoneyFlowSourceRecipientTypeId], [PeerMoneyFlowSourceRecipientTypeId], [IsSource], [IsRecipient])
SELECT tmp.[MoneyFlowSourceRecipientTypeId], tmp.[PeerMoneyFlowSourceRecipientTypeId], tmp.[IsSource], tmp.[IsRecipient]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[MoneyFlowSourceRecipientTypeSettings] tbl ON tbl.[MoneyFlowSourceRecipientTypeId] = tmp.[MoneyFlowSourceRecipientTypeId] AND tbl.[PeerMoneyFlowSourceRecipientTypeId] = tmp.[PeerMoneyFlowSourceRecipientTypeId]
WHERE tbl.[MoneyFlowSourceRecipientTypeId] IS NULL AND tbl.[PeerMoneyFlowSourceRecipientTypeId] IS NULL

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[IsSource] = tmp.[IsSource],
LiveTable.[IsRecipient] = tmp.[IsRecipient]
FROM [dbo].[MoneyFlowSourceRecipientTypeSettings] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[MoneyFlowSourceRecipientTypeId] = tmp.[MoneyFlowSourceRecipientTypeId] AND LiveTable.[PeerMoneyFlowSourceRecipientTypeId] = tmp.[PeerMoneyFlowSourceRecipientTypeId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[MoneyFlowSourceRecipientTypeSettings] FROM [dbo].[MoneyFlowSourceRecipientTypeSettings] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[MoneyFlowSourceRecipientTypeId] = tmp.[MoneyFlowSourceRecipientTypeId] AND LiveTable.[PeerMoneyFlowSourceRecipientTypeId] = tmp.[PeerMoneyFlowSourceRecipientTypeId]
	WHERE tmp.[MoneyFlowSourceRecipientTypeId] IS NULL AND tmp.[PeerMoneyFlowSourceRecipientTypeId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[MoneyFlowSourceRecipientTypeSettings]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO