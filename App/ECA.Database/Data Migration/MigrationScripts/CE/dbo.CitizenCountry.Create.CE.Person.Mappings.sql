USE CE
GO

/* This builds the citizenship country table for CE data */

/*
SELECT c.person_id,c.country,
	citizencountry.locationid,citizencountry.locationname,citizencountry.locationiso,
	cp.*,
	birthcountry.locationid,birthcountry.locationname,birthcountry.locationiso,
	g.genderid,g.gendername,
	m.maritalstatusid,m.Status,
	city.locationid,city.locationname,city.country_locationid,
	ep.*
*/

/* Local copy first */
INSERT INTO eca_dev_local_copy.dbo.CitizenCountry
	(PersonId,LocationId)
SELECT ep.personid,citizencountry.LocationId
FROM ce_person_citizenship c
JOIN eca_dev_local_copy.dbo.location citizencountry 
  ON (citizencountry.locationtypeid = 3 AND citizencountry.locationname = c.country)
JOIN ce_person cp 
  ON (cp.person_id = c.person_id)
LEFT JOIN eca_dev_local_copy.dbo.location birthcountry 
  ON (birthcountry.locationtypeid = 3 AND birthcountry.locationname = cp.birth_country)
LEFT JOIN ECA_Dev_Local_Copy.dbo.gender g 
  ON (g.GenderName = 	CASE 
			WHEN cp.GENDER_CD = 'M' THEN 'Male'
			WHEN cp.GENDER_CD = 'F' Then 'Female'
			WHEN cp.GENDER_CD IS NULL THEN 'NotSpecified'
			ELSE 'Other'
			END)
LEFT JOIN ECA_Dev_Local_Copy.dbo.maritalstatus m 
  ON (m.status = CASE WHEN cp.MARITAL_STATUS IS NULL THEN 'N'
 		 ELSE cp.marital_status	END )
LEFT JOIN ECA_Dev_Local_Copy.dbo.location city 
       ON ((city.locationtypeid = 5 AND city.locationname = CASE WHEN cp.BIRTH_CITY IS NULL THEN cp.BIRTH_COUNTRY 
								 ELSE cp.BIRTH_CITY END)
		AND ((cp.BIRTH_country IS NULL AND birthcountry.Country_LocationId IS NULL) OR 
		((city.Country_locationId IS NULL AND birthcountry.LocationId IS NULL) OR (city.Country_LocationId = birthcountry.LocationId))))
LEFT JOIN eca_dev_local_copy.dbo.person ep 
  ON 
    (((cp.[First_Name] IS NULL AND ep.firstname IS NULL) OR (ep.firstname = cp.[First_Name])) AND
    ((cp.[Last_Name] IS NULL AND ep.lastname IS NULL) OR (ep.lastname = cp.[Last_Name])) AND
    ((cp.[PREFIX_CD] IS NULL AND ep.nameprefix IS NULL) OR (ep.nameprefix = SUBSTRING(cp.[PREFIX_CD],1,10))) AND
    ((cp.[SUFFIX_CD] IS NULL AND ep.namesuffix IS NULL) OR (ep.namesuffix = SUBSTRING(cp.[SUFFIX_CD],1,10))) AND
    ((cp.[Middle_Name] IS NULL AND ep.middlename IS NULL) OR (ep.middlename = cp.[Middle_Name])) AND
    ((cp.BIRTH_DATE IS NULL AND ep.dateofbirth = CAST(N'2015-04-11' AS Date)) OR (CONVERT(date,ep.dateofbirth,1) = CONVERT(date,cp.BIRTH_DATE,1))) AND
    ((cp.birth_city IS NULL AND cp.birth_country IS NULL AND ep.PlaceOfBirth_LocationId IS NULL) OR (ep.PlaceOfBirth_LocationId = city.locationid)) AND
    (g.genderid = ep.genderid)) AND 
    (m.MaritalStatusId = ep.MaritalStatusId)
WHERE ep.personid IS NOT NULL AND citizencountry.locationid IS NOT NULL
GROUP BY ep.personid,citizencountry.locationid

GO

/* NOW THE ECA_DEV DB */

/* Local copy first */
INSERT INTO eca_dev.eca_dev.dbo.CitizenCountry
	(PersonId,LocationId)
SELECT ep.personid,citizencountry.LocationId
FROM ce_person_citizenship c
JOIN eca_dev.eca_dev.dbo.location citizencountry 
  ON (citizencountry.locationtypeid = 3 AND citizencountry.locationname = c.country)
JOIN ce_person cp 
  ON (cp.person_id = c.person_id)
LEFT JOIN eca_dev.eca_dev.dbo.location birthcountry 
  ON (birthcountry.locationtypeid = 3 AND birthcountry.locationname = cp.birth_country)
LEFT JOIN eca_dev.eca_dev.dbo.gender g 
  ON (g.GenderName = 	CASE 
			WHEN cp.GENDER_CD = 'M' THEN 'Male'
			WHEN cp.GENDER_CD = 'F' Then 'Female'
			WHEN cp.GENDER_CD IS NULL THEN 'NotSpecified'
			ELSE 'Other'
			END)
LEFT JOIN eca_dev.eca_dev.dbo.maritalstatus m 
  ON (m.status = CASE WHEN cp.MARITAL_STATUS IS NULL THEN 'N'
 		 ELSE cp.marital_status	END )
LEFT JOIN eca_dev.eca_dev.dbo.location city 
       ON ((city.locationtypeid = 5 AND city.locationname = CASE WHEN cp.BIRTH_CITY IS NULL THEN cp.BIRTH_COUNTRY 
								 ELSE cp.BIRTH_CITY END)
		AND ((cp.BIRTH_country IS NULL AND birthcountry.Country_LocationId IS NULL) OR 
		((city.Country_locationId IS NULL AND birthcountry.LocationId IS NULL) OR (city.Country_LocationId = birthcountry.LocationId))))
LEFT JOIN eca_dev.eca_dev.dbo.person ep 
  ON 
    (((cp.[First_Name] IS NULL AND ep.firstname IS NULL) OR (ep.firstname = cp.[First_Name])) AND
    ((cp.[Last_Name] IS NULL AND ep.lastname IS NULL) OR (ep.lastname = cp.[Last_Name])) AND
    ((cp.[PREFIX_CD] IS NULL AND ep.nameprefix IS NULL) OR (ep.nameprefix = SUBSTRING(cp.[PREFIX_CD],1,10))) AND
    ((cp.[SUFFIX_CD] IS NULL AND ep.namesuffix IS NULL) OR (ep.namesuffix = SUBSTRING(cp.[SUFFIX_CD],1,10))) AND
    ((cp.[Middle_Name] IS NULL AND ep.middlename IS NULL) OR (ep.middlename = cp.[Middle_Name])) AND
    ((cp.BIRTH_DATE IS NULL AND ep.dateofbirth = CAST(N'2015-04-11' AS Date)) OR (CONVERT(date,ep.dateofbirth,1) = CONVERT(date,cp.BIRTH_DATE,1))) AND
    ((cp.birth_city IS NULL AND cp.birth_country IS NULL AND city.locationid IS NULL) OR (ep.PlaceOfBirth_LocationId = city.locationid)) AND
    (g.genderid = ep.genderid)) AND 
    (m.MaritalStatusId = ep.MaritalStatusId)
WHERE ep.personid IS NOT NULL AND citizencountry.locationid IS NOT NULL
GROUP BY ep.personid,citizencountry.locationid

GO