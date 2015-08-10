/* Creates a Xref mapping CE Persons to ECA Persons */
SELECT 
  	cep.*, p.*

FROM CE_person cep 

LEFT JOIN eca_dev.eca_dev.dbo.location birthcountry 
  ON (birthcountry.locationtypeid = 3 AND birthcountry.[locationname] = cep.[Birth_Country])

LEFT JOIN eca_dev.eca_dev.dbo.location birthcity
  ON (birthcity.locationtypeid = 5 AND 
     ((cep.BIRTH_CITY IS NULL AND birthcity.locationname IS NULL AND birthcity.country_locationid = birthcountry.locationid) OR
      (birthcity.locationname = cep.[Birth_City] AND (birthcity.country_locationid = birthcountry.locationid OR birthcity.country_locationid IS NULL))))

LEFT JOIN ECA_Dev_Local_Copy.dbo.gender g 
  ON (g.GenderName = CASE 
                         WHEN cep.GENDER_CD = 'M' THEN 'Male' 
                         WHEN cep.GENDER_CD = 'F' Then 'Female' 
                         WHEN cep.GENDER_CD IS NULL THEN 'Not Specified' 
                         ELSE 'Other'
		     END)

LEFT JOIN ECA_Dev_Local_Copy.dbo.maritalstatus m 
  ON (m.status = CASE 
                    WHEN cep.MARITAL_STATUS IS NULL THEN 'N' 
		    ELSE cep.marital_status 
		 END )

LEFT JOIN eca_dev.eca_dev.dbo.person p 
  ON (
  	((cep.[First_Name] IS NULL AND p.firstname IS NULL) OR (p.firstname = cep.[First_Name])) AND
  	((cep.[Last_Name] IS NULL AND p.lastname IS NULL) OR (p.lastname = cep.[Last_Name])) AND
  	((cep.[Middle_Name] IS NULL AND p.middlename IS NULL) OR (p.middlename = cep.[Middle_Name])) AND
  	((cep.[Prefix_cd] IS NULL AND p.nameprefix IS NULL) OR (p.nameprefix = cep.[Prefix_cd])) AND
  	((cep.[Suffix_cd] IS NULL AND p.namesuffix IS NULL) OR (p.namesuffix = cep.[Suffix_cd])) AND
  	((cep.[Birth_date] IS NULL AND p.dateofbirth IS NULL) OR (CONVERT(DATE,cep.[Birth_date],1) = CONVERT(DATE,p.dateofbirth,1))) AND
  	(p.maritalstatusid = m.maritalstatusid) AND
  	(p.genderid = g.genderid) AND
  	((cep.birth_city IS NULL AND cep.birth_country IS NULL AND p.placeofbirth_locationid IS NULL) OR
   	((cep.birth_city IS NOT NULL OR cep.birth_country IS NOT NULL) AND (p.placeofbirth_locationid = birthcity.locationid OR p.placeofbirth_locationid IS NULL)))
     )

WHERE p.personid IS NOT NULL

ORDER BY p.personid