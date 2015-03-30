/* Creates Addresses for Fulbright participants in Location Table */
/* Host,Home */
INSERT INTO eca_dev_test.dbo.location
 (LocationTypeId,Latitude,Longitude,
  Street1,Street2,Street3,City,Division,PostalCode,LocationName,LocationIso,
     History_createdby,history_createdon,history_revisedby,history_revisedon,
  Region_LocationId,Country_LocationId)
SELECT 9 LocationTypeId,
NULL Latitude,
NULL Longitude,
Street1,
Street2,
Street3,
City,
NULL Division,
PostalCode,
NULL  LocationName,
NULL  LocationISO,
0  History_CreatedBy, 
CAST(N'2015-03-19T00:00:00.0000000-05:00' AS DateTimeOffset)  History_CreatedOn,
0  History_RevisedBy, 
CAST(N'2015-03-19T00:00:00.0000000-05:00' AS DateTimeOffset)  History_RevisedOn,
Region_LocationId,
Country_Locationid
FROM (
SELECT 
	v.[Home Address 1]  Street1,
	v.[Home Address 2]  Street2,
	v.[Home Address 3]  Street3,
	v.[Home City]  City,
	v.[Home Zip or Postal Code] PostalCode,
	l.region_locationid Region_LocationId,
	v.[Home Country Name],
	c.ISOCode3,
	l.locationid Country_Locationid
 FROM VisitingScholarData v
 LEFT JOIN DataMigrationXREF.dbo.Vw_CountryCodeXREF c ON (c.ISOCode2 = v.[Home Country Name])
 LEFT JOIN eca_dev.eca_dev.dbo.location l ON (l.locationtypeid = 3 AND l.locationiso = c.ISOCode3)

UNION ALL

SELECT 
	v.[Host Address 1]  Street1,
	v.[Host Address 2]  Street2,
	v.[Host Address 3]  Street3,
	v.[Host City]  City,
	v.[Host Zip or Postal Code] PostalCode,
	l.region_locationid Region_LocationId,
	v.[Host Country Name],
	c.ISOCode3,
	l.locationid Country_Locationid
 FROM VisitingScholarData v
 LEFT JOIN DataMigrationXREF.dbo.Vw_CountryCodeXREF c ON (c.ISOCode2 = v.[Host Country Name])
 LEFT JOIN eca_dev.eca_dev.dbo.location l ON (l.locationtypeid = 3 AND l.locationiso = c.ISOCode3)

) T1

GO

/* Creates Addresses for Fulbright participants in Location Table */
/* Host Institution,Home Institution */
INSERT INTO eca_dev.eca_dev.dbo.location
 (LocationTypeId,Latitude,Longitude,
  Street1,Street2,Street3,City,Division,PostalCode,LocationName,LocationIso,
     History_createdby,history_createdon,history_revisedby,history_revisedon,
  Region_LocationId,Country_LocationId)
SELECT 9 LocationTypeId,
NULL Latitude,
NULL Longitude,
Street1,
Street2,
Street3,
City,
NULL Division,
PostalCode,
NULL  LocationName,
NULL  LocationISO,
0  History_CreatedBy, 
CAST(N'2015-03-19T00:00:00.0000000-05:00' AS DateTimeOffset)  History_CreatedOn,
0  History_RevisedBy, 
CAST(N'2015-03-19T00:00:00.0000000-05:00' AS DateTimeOffset)  History_RevisedOn,
Region_LocationId,
Country_Locationid
FROM (
SELECT 
	NULL  Street1,
	NULL  Street2,
	NULL  Street3,
	v.[Home Institution City]  City,
	v.[Home Institution Zip or Postal Code] PostalCode,
	l.region_locationid Region_LocationId,
	v.[Home Institution Country Name],
	c.ISOCode3,
	l.locationid Country_Locationid
 FROM VisitingScholarData v
 LEFT JOIN DataMigrationXREF.dbo.Vw_CountryCodeXREF c ON (c.ISOCode2 = v.[Home Institution Country Name])
 LEFT JOIN eca_dev.eca_dev.dbo.location l ON (l.locationtypeid = 3 AND l.locationiso = c.ISOCode3)

UNION ALL

SELECT 
	NULL  Street1,
	NULL  Street2,
	NULL  Street3,
	v.[Host Institution City]  City,
	v.[Host Institution Zip or Postal Code] PostalCode,
	l.region_locationid Region_LocationId,
	v.[Host Institution Country Name],
	c.ISOCode3,
	l.locationid Country_Locationid
 FROM VisitingScholarData v
 LEFT JOIN DataMigrationXREF.dbo.Vw_CountryCodeXREF c ON (c.ISOCode2 = v.[Host Institution Country Name])
 LEFT JOIN eca_dev.eca_dev.dbo.location l ON (l.locationtypeid = 3 AND l.locationiso = c.ISOCode3)

) T1



/* Create the Person and Organization mappings */
/* Home address */
INSERT INTO eca_dev.eca_dev.dbo.address
	(AddressTypeId,LocationId,DisplayName,PersonId,OrganizationId,
	History_createdby,history_createdon,history_revisedby,history_revisedon)
SELECT at.AddressTypeId,
       l.locationid,
	at.addressname + N' Address',
       p.PersonId,
       NULL organizationid,
       0,N'3/20/2015 12:00:00 AM -05:00',0, N'3/20/2015 12:00:00 AM -05:00'
  FROM VisitingScholarData vs
  JOIN eca_dev.eca_dev.dbo.person p ON (
	(vs.[First Name] IS NULL OR (p.firstname = vs.[First Name])) AND
  	(vs.[Last Name] IS NULL OR (p.lastname = vs.[Last Name])) AND
  	(vs.[Prefix] IS NULL OR (p.nameprefix = vs.[Prefix])) AND
  	(vs.[Suffix] IS NULL OR (p.namesuffix = vs.[Suffix])) AND
  	(vs.[Second Last Name] IS NULL OR (p.secondlastname = vs.[Second Last Name])) AND
  	(vs.[Middle Name] IS NULL OR (p.middlename = vs.[Middle Name])))
  JOIN eca_dev.eca_dev.dbo.location l ON (
  	(vs.[Home Address 1] IS NULL OR (l.street1 = vs.[Home Address 1])) AND
  	(vs.[Home Address 2] IS NULL OR (l.street2 = vs.[Home Address 2])) AND
  	(vs.[Home Address 3] IS NULL OR (l.street3 = vs.[Home Address 3])) AND
  	(vs.[Home City] IS NULL OR (l.city = vs.[Home City])) AND 
  	(vs.[Home Zip or Postal Code] IS NULL OR (l.postalcode = vs.[Home Zip or Postal Code])))
  JOIN eca_dev.eca_dev.dbo.addresstype at ON (at.addressname = 'Home')
  --LEFT JOIN eca_dev.eca_dev.dbo.organization o ON (o.name = vs.[Home Institution Name])
 ORDER BY vs.[Last Name],vs.[First Name]

GO

/* Host address */
INSERT INTO eca_dev.eca_dev.dbo.address
	(AddressTypeId,LocationId,DisplayName,PersonId,OrganizationId,
	History_createdby,history_createdon,history_revisedby,history_revisedon)
SELECT at.AddressTypeId,
       l.locationid,
	at.addressname + N' Address',
       p.PersonId,
       NULL organizationid,
       0,N'3/20/2015 12:00:00 AM -05:00',0, N'3/20/2015 12:00:00 AM -05:00'
  FROM VisitingScholarData vs
  JOIN eca_dev.eca_dev.dbo.person p ON (
	(vs.[First Name] IS NULL OR (p.firstname = vs.[First Name])) AND
  	(vs.[Last Name] IS NULL OR (p.lastname = vs.[Last Name])) AND
  	(vs.[Prefix] IS NULL OR (p.nameprefix = vs.[Prefix])) AND
  	(vs.[Suffix] IS NULL OR (p.namesuffix = vs.[Suffix])) AND
  	(vs.[Second Last Name] IS NULL OR (p.secondlastname = vs.[Second Last Name])) AND
  	(vs.[Middle Name] IS NULL OR (p.middlename = vs.[Middle Name])))
  JOIN eca_dev.eca_dev.dbo.location l ON (
  	(vs.[Host Address 1] IS NULL OR (l.street1 = vs.[Host Address 1])) AND
  	(vs.[Host Address 2] IS NULL OR (l.street2 = vs.[Host Address 2])) AND
  	(vs.[Host Address 3] IS NULL OR (l.street3 = vs.[Host Address 3])) AND
  	(vs.[Host City] IS NULL OR (l.city = vs.[Host City])) AND 
  	(vs.[Host Zip or Postal Code] IS NULL OR (l.postalcode = vs.[Host Zip or Postal Code])))
  JOIN eca_dev.eca_dev.dbo.addresstype at ON (at.addressname = 'Host')
  --LEFT JOIN eca_dev.eca_dev.dbo.organization o ON (o.name = vs.[Host Institution Name])
 ORDER BY vs.[Last Name],vs.[First Name]

GO


/* Home Institution address */
INSERT INTO eca_dev.eca_dev.dbo.address
	(AddressTypeId,LocationId,DisplayName,PersonId,OrganizationId,
	History_createdby,history_createdon,history_revisedby,history_revisedon)
SELECT at.AddressTypeId,
       l.locationid,
	at.addressname + N' Address',
       NULL PersonId,
       o.organizationid organizationid,
       0,N'3/20/2015 12:00:00 AM -05:00',0, N'3/20/2015 12:00:00 AM -05:00'
  FROM VisitingScholarData vs
  JOIN eca_dev.eca_dev.dbo.location l ON (
   	(vs.[Home Institution City] IS NULL OR (l.city = vs.[Home Institution City])) AND 
  	(vs.[Home Institution Zip or Postal Code] IS NULL OR (l.postalcode = vs.[Home Institution Zip or Postal Code])))
  JOIN eca_dev.eca_dev.dbo.addresstype at ON (at.addressname = 'Organization')
  LEFT JOIN eca_dev.eca_dev.dbo.organization o ON (o.name = vs.[Home Institution Name])
 ORDER BY vs.[Home Institution Name]
GO


/* Host and Home Institution address */
INSERT 
  INTO eca_dev.eca_dev.dbo.address
	(AddressTypeId,LocationId,DisplayName,PersonId,OrganizationId,
	History_createdby,history_createdon,history_revisedby,history_revisedon)
SELECT 	AddressTypeId,
       	locationid,
	DisplayName,
       	NULL PersonId,
       	organizationid,
       	0,N'3/20/2015 12:00:00 AM -05:00',0, N'3/20/2015 12:00:00 AM -05:00'
   FROM (
	SELECT DISTINCT addresstypeid,locationid,displayname,organizationid
	FROM (
		SELECT at.AddressTypeId,
       			l.locationid,
	 		o.name + ' ' + at.addressname + N' Address' DisplayName,
       			o.organizationid organizationid
  		FROM VisitingScholarData vs
  		JOIN eca_dev.eca_dev.dbo.location l ON (
   			l.city = vs.[Host Institution City] AND 
  			l.postalcode = vs.[Host Institution Zip or Postal Code] AND 
			l.locationtypeid = 9 AND l.street1 IS NULL)
  		JOIN eca_dev.eca_dev.dbo.addresstype at ON (at.addressname = 'Organization')
  		LEFT JOIN eca_dev.eca_dev.dbo.organization o ON (o.name = vs.[Host Institution Name])
  		UNION ALL
  		SELECT at.AddressTypeId,
       			l.locationid,
	 		o.name + ' ' + at.addressname + N' Address' DisplayName,
       			o.organizationid organizationid
  		FROM VisitingScholarData vs
  		JOIN eca_dev.eca_dev.dbo.location l ON (
   			l.city = vs.[Home Institution City] AND 
  			l.postalcode = vs.[Home Institution Zip or Postal Code] AND 
			l.locationtypeid = 9 AND l.street1 IS NULL)
  		JOIN eca_dev.eca_dev.dbo.addresstype at ON (at.addressname = 'Organization')
  		LEFT JOIN eca_dev.eca_dev.dbo.organization o ON (o.name = vs.[Home Institution Name])) T2
		
		) T1

GO


 



/* ************************************************************** */


/* Erroneous Country Codes are fixed */
UPDATE dbo.VisitingScholarData  
SET     [Home Country Name] =  CASE  
                       WHEN [Home Country Name] = 'RP' THEN 'PH' 
                        WHEN [Home Country Name] = 'EI' THEN 'IE'
			WHEN [Home Country Name] = 'BU' THEN 'BG'
			WHEN [Home Country Name] = 'YN' THEN 'YE'
			WHEN [Home Country Name] = 'IV' THEN 'CI'
			WHEN [Home Country Name] = 'CE' THEN 'LK'
			WHEN [Home Country Name] = 'KS' THEN 'KR'
			WHEN [Home Country Name] = 'JA' THEN 'JP'
			WHEN [Home Country Name] = 'PO' THEN 'PT'
			WHEN [Home Country Name] = 'UK' THEN 'GB'
			WHEN [Home Country Name] = 'SP' THEN 'ES'
			WHEN [Home Country Name] = 'TU' THEN 'TR'
			WHEN [Home Country Name] = 'UP' THEN 'UA'
			WHEN [Home Country Name] = 'LO' THEN 'SK'
                        ELSE [Home Country Name]
                    END 
FROM VisitingScholarData
WHERE   [Home Country Name] IN ('RP','EI','BU','YN','IV','CE','KS','JA','PO','UK','SP','TU','UP','LO')
GO

UPDATE dbo.VisitingScholarData  
SET     [Birth Country Name] =  CASE  
                       WHEN [Birth Country Name] = 'RP' THEN 'PH' 
                        WHEN [Birth Country Name] = 'EI' THEN 'IE'
			WHEN [Birth Country Name] = 'BU' THEN 'BG'
			WHEN [Birth Country Name] = 'YN' THEN 'YE'
			WHEN [Birth Country Name] = 'IV' THEN 'CI'
			WHEN [Birth Country Name] = 'CE' THEN 'LK'
			WHEN [Birth Country Name] = 'KS' THEN 'KR'
			WHEN [Birth Country Name] = 'JA' THEN 'JP'
			WHEN [Birth Country Name] = 'PO' THEN 'PT'
			WHEN [Birth Country Name] = 'UK' THEN 'GB'
			WHEN [Birth Country Name] = 'SP' THEN 'ES'
			WHEN [Birth Country Name] = 'TU' THEN 'TR'
			WHEN [Birth Country Name] = 'UP' THEN 'UA'
			WHEN [Birth Country Name] = 'LO' THEN 'SK'
                        ELSE [Birth Country Name]
                    END 
FROM VisitingScholarData
WHERE   [Birth Country Name] IN ('RP','EI','BU','YN','IV','CE','KS','JA','PO','UK','SP','TU','UP','LO')
GO

UPDATE dbo.VisitingScholarData  
SET     [Citizenship Country Name] =  CASE  
                       WHEN [Citizenship Country Name] = 'RP' THEN 'PH' 
                        WHEN [Citizenship Country Name] = 'EI' THEN 'IE'
			WHEN [Citizenship Country Name] = 'BU' THEN 'BG'
			WHEN [Citizenship Country Name] = 'YN' THEN 'YE'
			WHEN [Citizenship Country Name] = 'IV' THEN 'CI'
			WHEN [Citizenship Country Name] = 'CE' THEN 'LK'
			WHEN [Citizenship Country Name] = 'KS' THEN 'KR'
			WHEN [Citizenship Country Name] = 'JA' THEN 'JP'
			WHEN [Citizenship Country Name] = 'PO' THEN 'PT'
			WHEN [Citizenship Country Name] = 'UK' THEN 'GB'
			WHEN [Citizenship Country Name] = 'SP' THEN 'ES'
			WHEN [Citizenship Country Name] = 'TU' THEN 'TR'
			WHEN [Citizenship Country Name] = 'UP' THEN 'UA'
			WHEN [Citizenship Country Name] = 'LO' THEN 'SK'
                        ELSE [Citizenship Country Name]
                    END 
FROM VisitingScholarData
WHERE   [Citizenship Country Name] IN ('RP','EI','BU','YN','IV','CE','KS','JA','PO','UK','SP','TU','UP','LO')
GO

UPDATE dbo.VisitingScholarData  
SET     [Dual Citizenship Country Name] =  CASE  
                       WHEN [Dual Citizenship Country Name] = 'RP' THEN 'PH' 
                        WHEN [Dual Citizenship Country Name] = 'EI' THEN 'IE'
			WHEN [Dual Citizenship Country Name] = 'BU' THEN 'BG'
			WHEN [Dual Citizenship Country Name] = 'YN' THEN 'YE'
			WHEN [Dual Citizenship Country Name] = 'IV' THEN 'CI'
			WHEN [Dual Citizenship Country Name] = 'CE' THEN 'LK'
			WHEN [Dual Citizenship Country Name] = 'KS' THEN 'KR'
			WHEN [Dual Citizenship Country Name] = 'JA' THEN 'JP'
			WHEN [Dual Citizenship Country Name] = 'PO' THEN 'PT'
			WHEN [Dual Citizenship Country Name] = 'UK' THEN 'GB'
			WHEN [Dual Citizenship Country Name] = 'SP' THEN 'ES'
			WHEN [Dual Citizenship Country Name] = 'TU' THEN 'TR'
			WHEN [Dual Citizenship Country Name] = 'UP' THEN 'UA'
			WHEN [Dual Citizenship Country Name] = 'LO' THEN 'SK'
                        ELSE [Dual Citizenship Country Name]
                    END 
FROM VisitingScholarData
WHERE   [Dual Citizenship Country Name] IN ('RP','EI','BU','YN','IV','CE','KS','JA','PO','UK','SP','TU','UP','LO')
GO

UPDATE dbo.VisitingScholarData  
SET     [Spouse Citizenship Country Name] =  CASE  
                       WHEN [Spouse Citizenship Country Name] = 'RP' THEN 'PH' 
                        WHEN [Spouse Citizenship Country Name] = 'EI' THEN 'IE'
			WHEN [Spouse Citizenship Country Name] = 'BU' THEN 'BG'
			WHEN [Spouse Citizenship Country Name] = 'YN' THEN 'YE'
			WHEN [Spouse Citizenship Country Name] = 'IV' THEN 'CI'
			WHEN [Spouse Citizenship Country Name] = 'CE' THEN 'LK'
			WHEN [Spouse Citizenship Country Name] = 'KS' THEN 'KR'
			WHEN [Spouse Citizenship Country Name] = 'JA' THEN 'JP'
			WHEN [Spouse Citizenship Country Name] = 'PO' THEN 'PT'
			WHEN [Spouse Citizenship Country Name] = 'UK' THEN 'GB'
			WHEN [Spouse Citizenship Country Name] = 'SP' THEN 'ES'
			WHEN [Spouse Citizenship Country Name] = 'TU' THEN 'TR'
			WHEN [Spouse Citizenship Country Name] = 'UP' THEN 'UA'
			WHEN [Spouse Citizenship Country Name] = 'LO' THEN 'SK'
                        ELSE [Spouse Citizenship Country Name]
                    END 
FROM VisitingScholarData
WHERE   [Spouse Citizenship Country Name] IN ('RP','EI','BU','YN','IV','CE','KS','JA','PO','UK','SP','TU','UP','LO')
GO

UPDATE dbo.VisitingScholarData  
SET     [Home Institution Country Name] =  CASE  
                       WHEN [Home Institution Country Name] = 'RP' THEN 'PH' 
                        WHEN [Home Institution Country Name] = 'EI' THEN 'IE'
			WHEN [Home Institution Country Name] = 'BU' THEN 'BG'
			WHEN [Home Institution Country Name] = 'YN' THEN 'YE'
			WHEN [Home Institution Country Name] = 'IV' THEN 'CI'
			WHEN [Home Institution Country Name] = 'CE' THEN 'LK'
			WHEN [Home Institution Country Name] = 'KS' THEN 'KR'
			WHEN [Home Institution Country Name] = 'JA' THEN 'JP'
			WHEN [Home Institution Country Name] = 'PO' THEN 'PT'
			WHEN [Home Institution Country Name] = 'UK' THEN 'GB'
			WHEN [Home Institution Country Name] = 'SP' THEN 'ES'
			WHEN [Home Institution Country Name] = 'TU' THEN 'TR'
			WHEN [Home Institution Country Name] = 'UP' THEN 'UA'
			WHEN [Home Institution Country Name] = 'LO' THEN 'SK'
                        ELSE [Home Institution Country Name]
                    END 
FROM VisitingScholarData
WHERE   [Home Institution Country Name] IN ('RP','EI','BU','YN','IV','CE','KS','JA','PO','UK','SP','TU','UP','LO')
GO

UPDATE dbo.VisitingScholarData  
SET     [Host Institution Country Name] =  CASE  
                       WHEN [Host Institution Country Name] = 'RP' THEN 'PH' 
                        WHEN [Host Institution Country Name] = 'EI' THEN 'IE'
			WHEN [Host Institution Country Name] = 'BU' THEN 'BG'
			WHEN [Host Institution Country Name] = 'YN' THEN 'YE'
			WHEN [Host Institution Country Name] = 'IV' THEN 'CI'
			WHEN [Host Institution Country Name] = 'CE' THEN 'LK'
			WHEN [Host Institution Country Name] = 'KS' THEN 'KR'
			WHEN [Host Institution Country Name] = 'JA' THEN 'JP'
			WHEN [Host Institution Country Name] = 'PO' THEN 'PT'
			WHEN [Host Institution Country Name] = 'UK' THEN 'GB'
			WHEN [Host Institution Country Name] = 'SP' THEN 'ES'
			WHEN [Host Institution Country Name] = 'TU' THEN 'TR'
			WHEN [Host Institution Country Name] = 'UP' THEN 'UA'
			WHEN [Host Institution Country Name] = 'LO' THEN 'SK'
                        ELSE [Host Institution Country Name]
                    END 
FROM VisitingScholarData
WHERE   [Host Institution Country Name] IN ('RP','EI','BU','YN','IV','CE','KS','JA','PO','UK','SP','TU','UP','LO')
GO


/* remove duplicates that crept in */
SELECT * FROM location where city = 'Cambridge' AND postalcode = '02138' AND street1 is null
DELETE FROM location where locationid in (1918,1925,1947)

SELECT * FROM location where city = 'New York' AND postalcode = '10027' AND street1 is null
DELETE FROM location where locationid in (1928,1956,1957,1983)

SELECT * FROM location where city = 'Ithaca' AND postalcode = '14853' AND street1 is null
DELETE FROM location where locationid in (1974,1984,1985)

SELECT * FROM location where city = 'Washington' AND postalcode = '20004-3027' AND street1 is null
DELETE FROM location where locationid in (1955)

SELECT * FROM location where city = 'Atlanta' AND postalcode = '30332' AND street1 is null
DELETE FROM location where locationid in (1951)

SELECT * FROM location where city = 'Gainesville' AND postalcode = '32611' AND street1 is null
DELETE FROM location where locationid in (1968)

SELECT * FROM location where city = 'Faisalabad' AND postalcode = '38040' AND street1 is null
DELETE FROM location where locationid in (1841,1842)

SELECT * FROM location where city = 'Los Banos' AND postalcode = '4031' AND street1 is null
DELETE FROM location where locationid in (1848)

SELECT * FROM location where city = 'Columbus' AND postalcode = '43210' AND street1 is null
DELETE FROM location where locationid in (1980)

SELECT * FROM location where city = 'Ames' AND postalcode = '50011' AND street1 is null
DELETE FROM location where locationid in (1987)

SELECT * FROM location where city = 'Chicago' AND postalcode = '60637' AND street1 is null
DELETE FROM location where locationid in (1946,1949)

SELECT * FROM location where city = 'Manhattan' AND postalcode = '66506' AND street1 is null
DELETE FROM location where locationid in (1988)

SELECT * FROM location where city = 'Lincoln' AND postalcode = '68583' AND street1 is null
DELETE FROM location where locationid in (1969)

SELECT * FROM location where city = 'Austin' AND postalcode = '78712' AND street1 is null
DELETE FROM location where locationid in (1933)

SELECT * FROM location where city = 'Los Angeles' AND postalcode = '90089' AND street1 is null
DELETE FROM location where locationid in (1926)

SELECT * FROM location where city = 'Berkeley' AND postalcode = '94720' AND street1 is null
DELETE FROM location where locationid in (1939)

SELECT * FROM location where city = 'Davis' AND postalcode = '95616-8571' AND street1 is null
DELETE FROM location where locationid in (1966,1976)
