/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [sevis].[StudentReprint].

PRINT 'Updating static data table [sevis].[StudentReprint]'

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
[StudentReprintId] int,
[ReprintCode] char(2),
[ReprintReason] nvarchar(100),
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
INSERT INTO @tblTempTable ([StudentReprintId], [ReprintCode], [ReprintReason], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', '01', 'Travel (valid only for those with F-1, F-2, M-1, or M-2 visa class)', '0', '5/29/2015 1:54:26 PM -04:00', '0', '5/29/2015 1:54:26 PM -04:00')
INSERT INTO @tblTempTable ([StudentReprintId], [ReprintCode], [ReprintReason], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', '02', 'Lost', '0', '5/29/2015 1:54:26 PM -04:00', '0', '5/29/2015 1:54:26 PM -04:00')
INSERT INTO @tblTempTable ([StudentReprintId], [ReprintCode], [ReprintReason], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', '03', 'Stolen', '0', '5/29/2015 1:54:26 PM -04:00', '0', '5/29/2015 1:54:26 PM -04:00')
INSERT INTO @tblTempTable ([StudentReprintId], [ReprintCode], [ReprintReason], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', '04', 'Damaged', '0', '5/29/2015 1:54:26 PM -04:00', '0', '5/29/2015 1:54:26 PM -04:00')
INSERT INTO @tblTempTable ([StudentReprintId], [ReprintCode], [ReprintReason], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', '50', 'Updated', '0', '5/29/2015 1:54:26 PM -04:00', '0', '5/29/2015 1:54:26 PM -04:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [sevis].[StudentReprint] ON
INSERT INTO [sevis].[StudentReprint] ([StudentReprintId], [ReprintCode], [ReprintReason], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[StudentReprintId], tmp.[ReprintCode], tmp.[ReprintReason], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [sevis].[StudentReprint] tbl ON tbl.[StudentReprintId] = tmp.[StudentReprintId]
WHERE tbl.[StudentReprintId] IS NULL
SET IDENTITY_INSERT [sevis].[StudentReprint] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[ReprintCode] = tmp.[ReprintCode],
LiveTable.[ReprintReason] = tmp.[ReprintReason],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [sevis].[StudentReprint] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[StudentReprintId] = tmp.[StudentReprintId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [sevis].[StudentReprint] FROM [sevis].[StudentReprint] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[StudentReprintId] = tmp.[StudentReprintId]
	WHERE tmp.[StudentReprintId] IS NULL
END

PRINT 'Finished updating static data table [sevis].[StudentReprint]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO