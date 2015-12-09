/***************************************
***   Static data management script  ***
***************************************/

-- This script will manage the static data from
-- your Team Database project for [sevis].[InternationalOrganization].

PRINT 'Updating static data table [sevis].[InternationalOrganization]'

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
[OrganizationId] int,
[OrganizationCode] nvarchar(10),
[Description] nvarchar(250),
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
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('1', 'ECA', 'U.N. ECONOMIC COMMISSION FOR AFRICA', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('2', 'ECE', 'U.N. ECONOMIC COMMISSION FOR EUROPE', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('3', 'ECLA', 'U.N. ECONOMIC COMMISSION FOR LATIN AMERICA AND THE CARIBBEAN', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('4', 'ECOSOC', 'U.N. ECONOMIC AND SOCIAL COUNCIL', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('5', 'EEC', 'EUROPEAN ECONOMIC COMMUNITY (COMMON MARKET)', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('6', 'ESCAP', 'U.N. ECONOMIC COMMISSION FOR ASIA AND FAR EAST', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('7', 'FAO', 'U.N. FOOD AND AGRICULTURAL ORGANIZATION', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('8', 'IAEA', 'INTERNATIONAL ATOMIC ENERGY', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('9', 'ICAO', 'INTERNATIONAL CIVIL AVIATION ORGANIZATION', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('10', 'ILO', 'INTERNATIONAL LABOR ORGANIZATION', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('11', 'IMF', 'INTERNATIONAL MONETARY FUND', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('12', 'IMO', 'INTERNATIONAL MARITIME ORGANIZATION', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('13', 'ITU', 'INTERNATIONAL TELECOMMUNICATIONS UNION', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('14', 'NATO', 'NORTH ATLANTIC TREATY ORGANIZATION', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('15', 'OAS', 'ORGANIZATION OF AMERICAN STATES', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('16', 'OAU', 'ORGANIZATION OF AFRICAN UNITY', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('17', 'OECD', 'ORGANIZATION FOR ECONOMIC COOPERATION AND DEVELOPMENT', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('18', 'OTHER', 'OTHER', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('19', 'PAHO', 'PAN AMERICAN HEALTH ORGANIZATION', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('20', 'UN', 'UNITED NATIONS', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('21', 'UNCTAD', 'U.N. CONFERENCE OF TRADE AND DEVELOPMENT', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('22', 'UNDP', 'U.N. DEVELOPMENT PROGRAM', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('23', 'UNESCO', 'U.N. EDUCATIONAL, SCIENTIFIC AND CULTURAL ORGANIZATION', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('24', 'UNICEF', 'U.N. CHILDREN''S FUND', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('25', 'UNIDO', 'U.N. INDUSTRIAL DEVELOPMENT ORGANIZATION', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('26', 'WB', 'WORLD BANK', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('27', 'WHO', 'WORLD HEALTH ORGANIZATION', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')
INSERT INTO @tblTempTable ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES ('28', 'WMO', 'WORLD METEOROLOGICAL ORGANIZATION', '1', '10/9/2015 8:44:22 PM -04:00', '1', '10/9/2015 8:44:22 PM -04:00')


-- 3: Insert any new items into the table from the table variable
SET IDENTITY_INSERT [sevis].[InternationalOrganization] ON
INSERT INTO [sevis].[InternationalOrganization] ([OrganizationId], [OrganizationCode], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
SELECT tmp.[OrganizationId], tmp.[OrganizationCode], tmp.[Description], tmp.[History_CreatedBy], tmp.[History_CreatedOn], tmp.[History_RevisedBy], tmp.[History_RevisedOn]
FROM @tblTempTable tmp
LEFT JOIN [sevis].[InternationalOrganization] tbl ON tbl.[OrganizationId] = tmp.[OrganizationId]
WHERE tbl.[OrganizationId] IS NULL
SET IDENTITY_INSERT [sevis].[InternationalOrganization] OFF

-- 4: Update any modified values with the values from the table variable
UPDATE LiveTable SET
LiveTable.[OrganizationCode] = tmp.[OrganizationCode],
LiveTable.[Description] = tmp.[Description],
LiveTable.[History_CreatedBy] = tmp.[History_CreatedBy],
LiveTable.[History_CreatedOn] = tmp.[History_CreatedOn],
LiveTable.[History_RevisedBy] = tmp.[History_RevisedBy],
LiveTable.[History_RevisedOn] = tmp.[History_RevisedOn]
FROM [sevis].[InternationalOrganization] LiveTable 
INNER JOIN @tblTempTable tmp ON LiveTable.[OrganizationId] = tmp.[OrganizationId]

-- 5: Delete any missing records from the target
IF @DeleteMissingRecords = 1
BEGIN
	DELETE FROM [sevis].[InternationalOrganization] FROM [sevis].[InternationalOrganization] LiveTable
	LEFT JOIN @tblTempTable tmp ON LiveTable.[OrganizationId] = tmp.[OrganizationId]
	WHERE tmp.[OrganizationId] IS NULL
END

PRINT 'Finished updating static data table [sevis].[InternationalOrganization]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO