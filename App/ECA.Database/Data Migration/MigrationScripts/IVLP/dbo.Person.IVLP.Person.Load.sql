USE IVLP
GO

/* Insert Person rows for IVLP */

INSERT INTO eca_dev_test.dbo.Person
	(FirstName,LastName,NamePrefix,NameSuffix,MiddleName,GenderId,DateOfBirth,
	History_CreatedBy,History_CreatedOn,History_RevisedBy,History_RevisedOn,
	MaritalStatusId,PlaceOfBirth_LocationId)

SELECT p.FIRST_NAME,p.LAST_NAME,p.PREFIX_CD,p.SUFFIX_CD,p.MIDDLE_NAME,g.genderid,ISNULL(p.BIRTH_DATE,CAST(N'2015-04-04T00:00:00.0000000-05:00' AS DateTimeOffset)),
	0, CAST(N'2015-04-04T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-04-04T00:00:00.0000000-05:00' AS DateTimeOffset),
	m.maritalstatusid,l.locationid
FROM ivlp_person p
LEFT JOIN eca_dev.eca_dev.dbo.gender g 
  ON (g.gendername = 	CASE 
			WHEN p.GENDER_CD = 'M' THEN 'Male'
			WHEN p.GENDER_CD = 'F' Then 'Female'
			WHEN p.GENDER_CD IS NULL THEN 'NotSpecified'
			ELSE 'Other'
			END)
LEFT JOIN eca_dev.eca_dev.dbo.maritalstatus m ON (m.status = p.MARITAL_STATUS)
LEFT JOIN eca_dev.eca_dev.dbo.location l 
  ON (l.locationtypeid = CASE WHEN p.BIRTH_CITY IS NULL THEN 3 ELSE 5 END 
 	AND l.locationname = CASE WHEN p.BIRTH_CITY IS NULL THEN p.BIRTH_COUNTRY ELSE p.BIRTH_CITY END) 
ORDER BY p.LAST_NAME,p.FIRST_NAME


GO



