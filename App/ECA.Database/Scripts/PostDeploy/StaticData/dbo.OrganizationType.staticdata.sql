/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[OrganizationType].

PRINT 'Updating static data table [dbo].[OrganizationType]'

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
[OrganizationTypeId] int,
[OrganizationTypeName] nvarchar(MAX),
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
INSERT INTO @tblTempTable ([OrganizationTypeId], [OrganizationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', 'Office', '0', '1/19/2015 12:00:00 AM -05:00', '0', '1/19/2015 12:00:00 AM -05:00')
INSERT INTO @tblTempTable ([OrganizationTypeId], [OrganizationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', 'Branch', '0', '1/1/2015 12:00:00 AM +00:00', '0', '1/1/2016 12:00:00 AM +00:00')
INSERT INTO @tblTempTable ([OrganizationTypeId], [OrganizationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', 'Division', '0', '1/26/2015 12:00:00 AM +00:00', '0', '1/26/2015 12:00:00 AM +00:00')
INSERT INTO @tblTempTable ([OrganizationTypeId], [OrganizationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', 'Foreign Educational Institution', '0', '1/30/2015 6:39:38 PM -05:00', '0', '1/30/2015 6:39:38 PM -05:00')
INSERT INTO @tblTempTable ([OrganizationTypeId], [OrganizationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', 'Foreign Government', '0', '1/30/2015 6:39:38 PM -05:00', '0', '1/30/2015 6:39:38 PM -05:00')
INSERT INTO @tblTempTable ([OrganizationTypeId], [OrganizationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', 'Foreign NGO/PVO', '0', '1/30/2015 6:39:38 PM -05:00', '0', '1/30/2015 6:39:38 PM -05:00')
INSERT INTO @tblTempTable ([OrganizationTypeId], [OrganizationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', 'Other', '0', '1/30/2015 6:39:38 PM -05:00', '0', '1/30/2015 6:39:38 PM -05:00')
INSERT INTO @tblTempTable ([OrganizationTypeId], [OrganizationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('8', 'Public International Organization (PIO)', '0', '1/30/2015 6:39:38 PM -05:00', '0', '1/30/2015 6:39:38 PM -05:00')
INSERT INTO @tblTempTable ([OrganizationTypeId], [OrganizationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('9', 'U.S. Educational Institution', '0', '1/30/2015 6:39:38 PM -05:00', '0', '1/30/2015 6:39:38 PM -05:00')
INSERT INTO @tblTempTable ([OrganizationTypeId], [OrganizationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('10', 'U.S. Non-Profit Organization (501(c)(3))', '0', '1/30/2015 6:39:38 PM -05:00', '0', '1/30/2015 6:39:38 PM -05:00')
INSERT INTO @tblTempTable ([OrganizationTypeId], [OrganizationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('11', 'Individual', '0', '2/3/2015 12:00:00 AM -05:00', '0', '2/3/2015 12:00:00 AM -05:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[OrganizationType] ON
INSERT INTO [dbo].[OrganizationType] ([OrganizationTypeId], [OrganizationTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[OrganizationTypeId], tmp.[OrganizationTypeName], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[OrganizationType] tbl ON tbl.[OrganizationTypeId] = tmp.[OrganizationTypeId]
WHERE tbl.[OrganizationTypeId] IS NULL
SET IDENTITY_INSERT [dbo].[OrganizationType] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[OrganizationTypeName] = tmp.[OrganizationTypeName],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [dbo].[OrganizationType] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[OrganizationTypeId] = tmp.[OrganizationTypeId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[OrganizationType] FROM [dbo].[OrganizationType] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[OrganizationTypeId] = tmp.[OrganizationTypeId]
	WHERE tmp.[OrganizationTypeId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[OrganizationType]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO