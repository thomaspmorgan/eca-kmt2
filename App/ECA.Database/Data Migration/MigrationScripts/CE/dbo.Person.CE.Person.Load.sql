USE CE
GO

/* Create Person rows for CE */
INSERT INTO eca_dev.eca_dev.dbo.Person
	(FirstName,LastName,NamePrefix,NameSuffix,MiddleName,GenderId,DateOfBirth,
	History_CreatedBy,History_CreatedOn,History_RevisedBy,History_RevisedOn,
	MaritalStatusId,PlaceOfBirth_LocationId)

SELECT p.FIRST_NAME,p.LAST_NAME,SUBSTRING(p.PREFIX_CD,1,10),SUBSTRING(p.SUFFIX_CD,1,10),p.MIDDLE_NAME,g.genderid,p.BIRTH_DATE,
	0, CAST(N'2015-05-11T00:00:00.0000000-00:00' AS DateTimeOffset), 0, CAST(N'2015-05-11T00:00:00.0000000-00:00' AS DateTimeOffset),
	m.maritalstatusid,l.locationid
FROM ce_person p
LEFT JOIN eca_dev.eca_dev.dbo.gender g 
       ON (g.gendername = CASE 
			    WHEN p.GENDER_CD = 'M' THEN 'Male'
			    WHEN p.GENDER_CD = 'F' Then 'Female'
			    WHEN p.GENDER_CD IS NULL THEN 'Not Specified'
			    ELSE 'Other'
			  END)
LEFT JOIN eca_dev.eca_dev.dbo.maritalstatus m 
       ON (m.status = CASE WHEN p.MARITAL_STATUS IS NULL THEN 'N' ELSE p.marital_status END )
LEFT JOIN eca_dev.eca_dev.dbo.location l1 
       ON (l1.locationtypeid = 3 AND l1.locationname = cp.birth_country)
LEFT JOIN ECA_Dev.eca_dev.dbo.location l ON (l.locationtypeid = 5 AND 
        /* Birth City Null and birth country Exists */
     	((l.locationname IS NULL AND p.birth_city IS NULL AND l.country_locationid = l1.locationid) OR
        /* Birth City Exists and Birth Country does not exist OR Birth Country DOES exist */      
	(l.locationname = p.BIRTH_CITY AND ((l.country_locationid IS NULL AND l1.locationid IS NULL) OR l.country_locationid = l1.locationid))))
GROUP BY p.FIRST_NAME,p.LAST_NAME,p.PREFIX_CD,p.SUFFIX_CD,p.MIDDLE_NAME,g.genderid,p.birth_date,m.maritalstatusid,l.locationid

GO






