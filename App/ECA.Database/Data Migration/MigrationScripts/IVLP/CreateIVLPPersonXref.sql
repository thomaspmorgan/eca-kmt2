/* Create Person XRef */


SELECT cp.person_ID,ep.personid
FROM ivlp_person cp 
LEFT JOIN ECA_Dev_Local_Copy.dbo.gender g 
  ON (g.GenderName = 	CASE 
			WHEN cp.GENDER_CD = 'M' THEN 'Male'
			WHEN cp.GENDER_CD = 'F' Then 'Female'
			WHEN cp.GENDER_CD IS NULL THEN 'Not Specified'
			ELSE 'Other'
			END)
LEFT JOIN ECA_Dev_Local_Copy.dbo.maritalstatus m 
  ON (m.status = CASE WHEN cp.MARITAL_STATUS IS NULL THEN 'N' ELSE cp.marital_status END )
LEFT JOIN eca_dev_local_copy.dbo.location birthcountry 
  ON (birthcountry.locationtypeid = 3 AND birthcountry.locationname = cp.birth_country)
LEFT JOIN ECA_Dev_Local_Copy.dbo.location city 
  ON (city.locationtypeid = 5 AND city.locationname = cp.BIRTH_CITY AND
     (city.Country_LocationId = birthcountry.LocationId OR (city.Country_LocationId IS NULL AND birthcountry.locationid IS NULL))) 
LEFT JOIN eca_dev_local_copy.dbo.person ep 
  ON 
    (((cp.[First_Name] IS NULL AND ep.firstname IS NULL) OR (ep.firstname = cp.[First_Name])) AND
    ((cp.[Last_Name] IS NULL AND ep.lastname IS NULL) OR (ep.lastname = cp.[Last_Name])) AND
    ((cp.[PREFIX_CD] IS NULL AND ep.nameprefix IS NULL) OR (ep.nameprefix = cp.[PREFIX_CD])) AND
    ((cp.[SUFFIX_CD] IS NULL AND ep.namesuffix IS NULL) OR (ep.namesuffix = cp.[SUFFIX_CD])) AND
    ((cp.[Middle_Name] IS NULL AND ep.middlename IS NULL) OR (ep.middlename = cp.[Middle_Name])) AND
/* This is corrected code that works following removal of not null constraint on Person.DateofBirth column */
    ((cp.BIRTH_DATE IS NULL AND ep.dateofbirth IS NULL) OR (CONVERT(date,ep.dateofbirth,101) = CONVERT(date,cp.BIRTH_DATE,101))) AND

 -- wrong  ((cp.BIRTH_DATE IS NULL AND ep.dateofbirth = CAST(N'2015-04-04' AS Date)) OR (CONVERT(date,ep.dateofbirth,1) = CONVERT(date,cp.BIRTH_DATE,1))) AND

    ((cp.birth_city IS NULL AND cp.birth_country IS NULL AND ep.PlaceOfBirth_LocationId IS NULL) OR (ep.PlaceOfBirth_LocationId = city.locationid)) AND

    ((g.genderid IS NULL AND ep.genderid is null) OR (g.genderid = ep.genderid))) AND 
    ((m.maritalstatusid is null and ep.maritalstatusid is null) OR (m.MaritalStatusId = ep.MaritalStatusId))

ORDER BY ep.personid
