/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [sevis].[Position].

PRINT 'Updating static data table [sevis].[Position]'

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
[PositionId] int,
[PositionCode] nvarchar(4),
[Description] nvarchar(200),
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
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', '100', 'CATEGORY - GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', '110', 'CENTRAL GOVERNMENT GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', '111', 'HEAD OF GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', '112', 'MINISTERIAL LEVEL OFFICIAL', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', '113', 'EXECUTIVE LEVEL OFFICIAL', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', '114', 'CIVIL SERVICE EMPLOYEE IN CENTRAL GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', '115', 'PROFESSIONALS AND SCIENTISTS IN CENTRAL GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('8', '116', 'LEGISLATOR IN CENTRAL GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('9', '117', 'JUDGES IN CENTRAL GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('10', '118', 'MANAGER OF STATE ENTERPRISE', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('11', '119', 'CENTRAL GOVERNMENT, OTHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('12', '120', 'REGIONAL,  STATE OR PROVINCIAL GOVERNMENT GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('13', '121', 'GOVERNOR OR OTHER CHIEF OF REGIONAL UNIT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('14', '122', 'SENIOR HEAD OF REGIONAL DEPT.', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('15', '123', 'EXECUTIVE LEVEL REGIONAL OFFICIALS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('16', '124', 'CIVIL SERVICE EMPLOYEE IN REGIONAL/STATE GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('17', '125', 'PROFESSIONALS AND SCIENTISTS IN REGIONAL  GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('18', '126', 'LEGISLATOR IN REGIONAL OR STATE GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('19', '127', 'JUDGES IN REGIONAL OR STATE GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('20', '128', 'MANAGER OF REGIONAL ENTERPRISE', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('21', '129', 'OTHER, REGIONAL GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('22', '130', 'CITY OR TOWN GOVERNMENT GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('23', '131', 'MAYOR OR CITY MANAGER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('24', '132', 'HEAD OF CITY DEPARTMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('25', '133', 'EXECUTIVE LEVEL CITY OR TOWN OFFICIALS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('26', '134', 'CIVIL SERVICE EMPLOYEE IN CITY OR TOWN GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('27', '135', 'PROFESSIONALS AND SCIENTISTS IN CITY OR TOWN GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('28', '136', 'LEGISLATOR IN CITY OR TOWN GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('29', '137', 'JUDGES IN CITY OR TOWN GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('30', '138', 'MANAGER OF CITY ENTERPRISE', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('31', '139', 'OTHER, CITY OR TOWN GOVERNMENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('32', '140', 'INTERNATIONAL ORGANIZATION GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('33', '141', 'HEAD OF INTERNATIONAL ORGANIZATION GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('34', '142', 'SENIOR OFFICIAL OF INTERNATIONAL ORGANIZATION', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('35', '143', 'EMPLOYEE OF INTERNATIONAL ORGANIZATION', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('36', '200', 'CATEGORY - ACADEMIC COMMUNITY', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('37', '210', 'UNIVERSITY LEVEL GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('38', '211', 'UNIVERSITY PRESIDENT OR RECTOR', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('39', '212', 'UNIVERSITY ADMINISTRATIVE STAFF', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('40', '213', 'UNIVERSITY TEACHING STAFF INCLUDING RESEARCHERS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('41', '214', 'UNIVERSITY GRADUATE STUDENTS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('42', '215', 'UNIVERSITY UNDERGRADUATE STUDENTS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('43', '216', 'UNIVERSITY MEDICAL SCHOOL STUDENTS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('44', '217', 'UNIVERSITY SCHOOL STUDENTS IN OTHER PROFESSIONS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('45', '218', 'UNIVERSITY POST GRAD MEDICAL TRAINEE', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('46', '219', 'OTHER  UNIVERSITY', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('47', '220', 'SECONDARY SCHOOL GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('48', '221', 'SECONDARY SCHOOL PRINCIPAL', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('49', '222', 'SECONDARY SCHOOL TEACHER OR STAFF', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('50', '223', 'SECONDARY SCHOOL STUDENT', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('51', '229', 'OTHER  SECONDARY SCHOOL', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('52', '230', 'ELEMENTARY SCHOOL GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('53', '231', 'TEACHER OR STAFF" ELEMENTARY PRINCIPAL', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('54', '239', 'OTHER  ELEMENTARY SCHOOL', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('55', '240', 'SPECIAL SCHOOLS, INSTITUTES, OR VOCATIONAL SCHOOL GROUPS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('56', '241', 'SPECIAL SCHOOL, INSTITUTE, OR VOCATIONAL HEAD', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('57', '242', 'SPECIAL SCHOOL, INSTITUTE, OR VOCATIONAL TEACHER OR STAFF', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('58', '249', 'SPECIAL SCHOOL, INSTITUTE, OR VOCATIONAL - OTHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('59', '300', 'CATEGORY - PRIVATE SECTOR', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('60', '310', 'PRIVATE BUSINESS GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('61', '311', 'PRIVATE BUSINESS ENTREPRENEUR', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('62', '312', 'CORPORATE EXECUTIVE', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('63', '313', 'MANAGER EMPLOYED BY PRIV.BUS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('64', '314', 'EMPLOYEE OF PRIVATE BUSINESS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('65', '315', 'PROFESSIONAL OR SCIENTISTS IN PRIVATE BUSINESS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('66', '319', 'PRIVATE BUSINESS, OTHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('67', '320', 'SELF-EMPLOYED PROFESSIONALS GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('68', '321', 'SELF-EMPL.PROFES.(LEGAL FIELD)', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('69', '322', 'SELF-EMPL.PROFES.(MEDICAL FLD)', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('70', '323', 'SELF-EMPL.PROFES.(TECH.FIELD)', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('71', '329', 'SELF-EMPL.PROFES.-OTHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('72', '330', 'INDEPENDENT, NON-PROFIT, HOSPITALS, OR OTHER ORGANIZATION GROUPS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('73', '331', 'DIR. OF INSTIT/CORP.OR HOSPITAL', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('74', '332', 'MANAGER-EXEC EMPL.BY INST/CORP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('75', '334', 'EMPLOYEE OF IND.INSTIT OR CORP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('76', '335', 'INSTIT/CORP PROFESS.NAL/SCIEN.', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('77', '339', 'INDEPENDENT, NON-PROFIT, HOSPITALS, OR SIMILAR ORGANIZATION - OTHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('78', '340', 'AGRICULTURE (INCL. FORRESTRY AND FISHERIES) GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('79', '341', 'AGRICULTURAL ENTREPRENEUR', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('80', '342', 'EXECUTIVE OF AGRICULT BUSINESS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('81', '343', 'AGRICULTURAL MANAGER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('82', '344', 'EMPLOYEE OF AGRICUL.ENTERPRISE', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('83', '345', 'PROFESSIONAL OR SCIENTISTS IN AGRICULTURE', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('84', '349', 'AGRICULTURE, OTHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('85', '350', 'RELIGION GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('86', '351', 'MINISTER OF RELIGION', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('87', '352', 'RELIGIOUS ORDER/CONGREGAT.MBER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('88', '353', 'THEOLOGIAN', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('89', '400', 'CATEGORY - THE ARTS AND SPORTS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('90', '410', 'THE ARTS GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('91', '411', 'ARTIST (GRAPHIC ARTS)', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('92', '412', 'AUTHOR (PLAYWRIGHT POET)', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('93', '413', 'STAGE OR FILM ACTOR', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('94', '414', 'FILM (OR STAGE) PRODUCER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('95', '415', 'COMPOSER OR MUSICIAN', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('96', '419', 'ARTS OTHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('97', '420', 'THE SPORTS GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('98', '421', 'ATHLETE', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('99', '422', 'COACH', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('100', '429', 'SPORTS OTHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('101', '500', 'CATEGORY - LABOR', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('102', '510', 'LABOR UNION GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('103', '511', 'LABOR UNION HEAD', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('104', '512', 'LABOR UNION OFFICIAL', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('105', '513', 'LABOR UNION OTHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('106', '520', 'LABOR UNION MINISTRY GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('107', '521', 'LABOR MINISTER(OR LAB.AG.HEAD)', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('108', '522', 'SENIOR MINISTERIAL OFFICIAL', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('109', '523', 'MINISTERIAL OR LABOR AG.EMPL.', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('110', '529', 'MINISTRY OF LABOR OTHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('111', '530', 'LABOR EXPERTS IN ACADEMIA GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('112', '531', 'CODE DELETED - SEE 213', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('113', '539', 'LABOR EXPERTS IN ACADEMIA OTHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('114', '540', 'LABOR ORGANIZATION AND OTHER LABOR GROUPS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('115', '541', 'HEAD OF LABOR ORGANIZATION', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('116', '542', 'EMPLOYEE OF LABOR ORGANIZATION', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('117', '600', 'CATEGORY - COMMUNICATIONS', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('118', '610', 'ELECTRONIC MEDIA GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('119', '611', 'HEAD OF TV OR RADIO STATION', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('120', '612', 'RADIO OR TV JOURNALIST', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('121', '613', 'ELECTRONIC MEDIA TECHNICIAN', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('122', '619', 'ELECTONIC MEDIA OTHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('123', '620', 'PRINTED MEDIA GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('124', '621', 'EDITOR AND/OR PUBLISHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('125', '622', 'JOURNALIST', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('126', '623', 'TECH.OFFI.IN PRINTED MEDIA FLD', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('127', '629', 'PRINTED MEDIA OTHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('128', '630', 'FILM AS NEWS MEDIA GROUP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('129', '631', 'FILM MAKER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('130', '639', 'FILM AS NEWS MEDIA OTHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('131', '700', 'CATEGORY - IMPORTANT POLITICAL FIGURES NOT CLASSIFIED ELSEWHERE', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('132', '710', 'OPPOSITION LEADER(NOT IN GOVT)', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('133', '720', 'OPPOSITION LEADER - LEGISLATURE GRP', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('134', '730', 'FORMER INFLUENTIAL POLITICAL OFFICIAL', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('135', '790', 'IMPORTANT POLITICAL FIGURE', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('136', '800', 'CATEGORY - MILITARY', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')
INSERT INTO @tblTempTable ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('137', '900', 'CATEGORY - OTHER', '0', '5/27/2015 9:28:58 AM -04:00', '0', '5/27/2015 9:28:58 AM -04:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [sevis].[Position] ON
INSERT INTO [sevis].[Position] ([PositionId], [PositionCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[PositionId], tmp.[PositionCode], tmp.[Description], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [sevis].[Position] tbl ON tbl.[PositionId] = tmp.[PositionId]
WHERE tbl.[PositionId] IS NULL
SET IDENTITY_INSERT [sevis].[Position] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[PositionCode] = tmp.[PositionCode],
LiveTable.[Description] = tmp.[Description],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [sevis].[Position] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[PositionId] = tmp.[PositionId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [sevis].[Position] FROM [sevis].[Position] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[PositionId] = tmp.[PositionId]
	WHERE tmp.[PositionId] IS NULL
END

PRINT 'Finished updating static data table [sevis].[Position]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO