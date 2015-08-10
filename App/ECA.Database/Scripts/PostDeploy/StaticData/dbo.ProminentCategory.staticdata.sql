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
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('1','Current President of a University/College','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('2','Ambassador to/from the U.S.','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('4','Cabinet Minister','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('5','Chief Justice of Supreme Court','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('7','First Lady (or First Gentleman)','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('8','Former Ambassador to/from the U.S.','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('9','Former Cabinet Minister','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('10','Former Chief Justice of Supreme Court','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('11','Former First Lady (or First Gentleman)','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('12','Former Head of Government/Chief of State','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('13','Former Head of International Organization','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('14','Former High-Ranking EU Official','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('15','Former Member of European Parliament','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('16','Former Member of U.S. Congress','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('17','Former President of a University/College','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('18','Former Representative to the Organization of American States','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('19','Former Representative to the United Nations','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('20','Former Vice President','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('21','Head of Government/Chief of State','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('22','Head of International Organization','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('23','High-Ranking EU Official','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('24','International Prize recipient','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('25','Member of European Parliament','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('26','Member of U.S. Congress','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('27','Nobel Laureate','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('28','Olympic Athlete','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('29','Other Prominent in Sports','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('30','Other prominent in Academia','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('31','Other prominent in Arts and Entertainment','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('32','Other prominent in Business','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('33','Other prominent in Civil Society','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('34','Other prominent in Journalism','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('35','Other prominent in Politics and Government','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('36','Pulitzer Prize Winner','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('37','Representative to the Organization of American States','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('38','Representative to the United Nations','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('39','U.S. Presidential Medal of Freedom','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')
INSERT INTO @tblTempTable ([ProminentCategoryId],[Name],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_Revised_On]) VALUES ('40','Vice President','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00','8/10/2015 12:00:00 AM -00:00')

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