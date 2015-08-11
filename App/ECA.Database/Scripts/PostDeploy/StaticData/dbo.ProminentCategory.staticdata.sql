/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [dbo].[ProminentCategory].

PRINT 'Updating static data table [dbo].[ProminentCategory]'

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
[ProminentCategoryId] int,
[Name] nvarchar(255),
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
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', 'Current President of a University/College', '1', '8/4/2015 12:00:00 AM -04:00', '1', '8/4/2015 12:00:00 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', 'Ambassador to/from the U.S.', '1', '8/4/2015 12:00:00 AM -04:00', '1', '8/4/2015 12:00:00 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', 'Cabinet Minister', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', 'Chief Justice of Supreme Court', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', 'First Lady (or First Gentleman)', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('8', 'Former Ambassador to/from the U.S.', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('9', 'Former Cabinet Minister', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('10', 'Former Chief Justice of Supreme Court', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('11', 'Former First Lady (or First Gentleman)', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('12', 'Former Head of Government/Chief of State', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('13', 'Former Head of International Organization', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('14', 'Former High-Ranking EU Official', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('15', 'Former Member of European Parliament', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('16', 'Former Member of U.S. Congress', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('17', 'Former President of a University/College', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('18', 'Former Representative to the Organization of American States', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('19', 'Former Representative to the United Nations', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('20', 'Former Vice President', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('21', 'Head of Government/Chief of State', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('22', 'Head of International Organization', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('23', 'High-Ranking EU Official', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('24', 'International Prize recipient', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('25', 'Member of European Parliament', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('26', 'Member of U.S. Congress', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('27', 'Nobel Laureate', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('28', 'Olympic Athlete', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('29', 'Other Prominent in Sports', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('30', 'Other prominent in Academia', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('31', 'Other prominent in Arts and Entertainment', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('32', 'Other prominent in Business', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('33', 'Other prominent in Civil Society', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('34', 'Other prominent in Journalism', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('35', 'Other prominent in Politics and Government', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('36', 'Pulitzer Prize Winner', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('37', 'Representative to the Organization of American States', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('38', 'Representative to the United Nations', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('39', 'U.S. Presidential Medal of Freedom', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')
INSERT INTO @tblTempTable ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('40', 'Vice President', '0', '8/10/2015 9:26:50 AM -04:00', '0', '8/10/2015 9:26:50 AM -04:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [dbo].[ProminentCategory] ON
INSERT INTO [dbo].[ProminentCategory] ([ProminentCategoryId], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[ProminentCategoryId], tmp.[Name], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [dbo].[ProminentCategory] tbl ON tbl.[ProminentCategoryId] = tmp.[ProminentCategoryId]
WHERE tbl.[ProminentCategoryId] IS NULL
SET IDENTITY_INSERT [dbo].[ProminentCategory] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[Name] = tmp.[Name],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [dbo].[ProminentCategory] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[ProminentCategoryId] = tmp.[ProminentCategoryId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [dbo].[ProminentCategory] FROM [dbo].[ProminentCategory] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[ProminentCategoryId] = tmp.[ProminentCategoryId]
	WHERE tmp.[ProminentCategoryId] IS NULL
END

PRINT 'Finished updating static data table [dbo].[ProminentCategory]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO