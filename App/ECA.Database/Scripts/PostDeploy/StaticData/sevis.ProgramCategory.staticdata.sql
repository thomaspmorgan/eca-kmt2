/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [sevis].[ProgramCategory].

PRINT 'Updating static data table [sevis].[ProgramCategory]'

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
[ProgramCategoryId] int,
[ProgramCategoryCode] char(2),
[Description] nvarchar(100),
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
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', '1A', 'STUDENT SECONDARY', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', '1B', 'STUDENT ASSOCIATE', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', '1C', 'STUDENT BACHELORS', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', '1D', 'STUDENT MASTERS', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', '1E', 'STUDENT DOCTORATE', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', '1F', 'STUDENT NON-DEGREE', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', '1G', 'STUDENT INTERN', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('8', '2A', 'TRAINEE (SPECIALTY)', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('9', '2B', 'TRAINEE (NON-SPECIALTY)', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('10', '03', 'TEACHER', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('11', '04', 'PROFESSOR', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('12', '05', 'INTERNATIONAL VISITOR', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('13', '06', 'ALIEN PHYSICIAN', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('14', '07', 'GOVERNMENT VISITOR', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('15', '08', 'RESEARCH SCHOLAR', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('16', '09', 'SHORT-TERM SCHOLAR', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('17', '10', 'SPECIALIST', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('18', '11', 'CAMP COUNSELOR', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('19', '12', 'SUMMER WORK/TRAVEL', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('20', '13', 'AUPAIR', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('21', '14', 'TRAINEE', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')
INSERT INTO @tblTempTable ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('22', '15', 'INTERN', '0', '5/29/2015 9:45:26 AM -04:00', '0', '5/29/2015 9:45:26 AM -04:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [sevis].[ProgramCategory] ON
INSERT INTO [sevis].[ProgramCategory] ([ProgramCategoryId], [ProgramCategoryCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[ProgramCategoryId], tmp.[ProgramCategoryCode], tmp.[Description], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [sevis].[ProgramCategory] tbl ON tbl.[ProgramCategoryId] = tmp.[ProgramCategoryId]
WHERE tbl.[ProgramCategoryId] IS NULL
SET IDENTITY_INSERT [sevis].[ProgramCategory] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[ProgramCategoryCode] = tmp.[ProgramCategoryCode],
LiveTable.[Description] = tmp.[Description],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [sevis].[ProgramCategory] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[ProgramCategoryId] = tmp.[ProgramCategoryId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [sevis].[ProgramCategory] FROM [sevis].[ProgramCategory] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[ProgramCategoryId] = tmp.[ProgramCategoryId]
	WHERE tmp.[ProgramCategoryId] IS NULL
END

PRINT 'Finished updating static data table [sevis].[ProgramCategory]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO