USE CE
GO

/* Create Person rows for CE */
INSERT INTO eca_dev_local_copy.dbo.Person
	(FirstName,LastName,NamePrefix,NameSuffix,MiddleName,GenderId,DateOfBirth,
	History_CreatedBy,History_CreatedOn,History_RevisedBy,History_RevisedOn,
	MaritalStatusId,PlaceOfBirth_LocationId)

SELECT p.FIRST_NAME,p.LAST_NAME,SUBSTRING(p.PREFIX_CD,1,10),SUBSTRING(p.SUFFIX_CD,1,10),p.MIDDLE_NAME,g.genderid,ISNULL(p.BIRTH_DATE,CAST(N'2015-04-11T00:00:00.0000000-05:00' AS DateTimeOffset)),--p.birth_Country,birthcountry.locationid,birthcountry.locationname,
	0, CAST(N'2015-04-11T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-04-11T00:00:00.0000000-05:00' AS DateTimeOffset),
	m.maritalstatusid,l.locationid
FROM ce_person p
LEFT JOIN eca_dev.eca_dev.dbo.gender g 
       ON (g.gendername = CASE 
			    WHEN p.GENDER_CD = 'M' THEN 'Male'
			    WHEN p.GENDER_CD = 'F' Then 'Female'
			    WHEN p.GENDER_CD IS NULL THEN 'NotSpecified'
			    ELSE 'Other'
			  END)
LEFT JOIN eca_dev.eca_dev.dbo.maritalstatus m 
       ON (m.status = CASE WHEN p.MARITAL_STATUS IS NULL THEN 'N' ELSE p.marital_status END )
LEFT JOIN eca_dev_local_copy.dbo.location birthcountry 
       ON (birthcountry.locationtypeid = 3 AND birthcountry.locationname = cp.birth_country)
LEFT JOIN eca_dev.eca_dev.dbo.location l 
       ON ((l.locationtypeid = 5 AND l.locationname = CASE WHEN p.BIRTH_CITY IS NULL THEN p.BIRTH_COUNTRY ELSE p.BIRTH_CITY END)
	    AND ((p.BIRTH_country IS NULL AND birthcountry.Country_LocationId IS NULL) OR 
		((l.Country_locationId IS NULL AND birthcountry.LocationId IS NULL) OR (l.Country_LocationId = birthcountry.LocationId))))
GROUP BY p.FIRST_NAME,p.LAST_NAME,p.PREFIX_CD,p.SUFFIX_CD,p.MIDDLE_NAME,g.genderid,p.birth_date,m.maritalstatusid,l.locationid

GO



/*   This one is not correct - was used for first build, but corrected now */
/*
SELECT p.FIRST_NAME,p.LAST_NAME,SUBSTRING(p.PREFIX_CD,1,10),SUBSTRING(p.SUFFIX_CD,1,10),p.MIDDLE_NAME,g.genderid,ISNULL(p.BIRTH_DATE,CAST(N'2015-04-11T00:00:00.0000000-05:00' AS DateTimeOffset)),
	0, CAST(N'2015-04-11T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-04-11T00:00:00.0000000-05:00' AS DateTimeOffset),
	m.maritalstatusid,l.locationid
FROM ce_person p
LEFT JOIN eca_dev.eca_dev.dbo.gender g 
  ON (g.gendername = 	CASE 
			WHEN p.GENDER_CD = 'M' THEN 'Male'
			WHEN p.GENDER_CD = 'F' Then 'Female'
			WHEN p.GENDER_CD IS NULL THEN 'NotSpecified'
			ELSE 'Other'
			END)
LEFT JOIN eca_dev.eca_dev.dbo.maritalstatus m ON (m.status = CASE
								WHEN p.MARITAL_STATUS IS NULL THEN 'N'
 								ELSE p.marital_status
								END )
LEFT JOIN eca_dev.eca_dev.dbo.location l 
  	ON (l.locationtypeid = CASE WHEN p.BIRTH_CITY IS NULL THEN 3 ELSE 5 END 
 		AND l.locationname = CASE WHEN p.BIRTH_CITY IS NULL THEN p.BIRTH_COUNTRY ELSE p.BIRTH_CITY END )
GROUP BY p.FIRST_NAME,p.LAST_NAME,p.PREFIX_CD,p.SUFFIX_CD,p.MIDDLE_NAME,g.genderid,p.birth_date,m.maritalstatusid,l.locationid
*/



GO



