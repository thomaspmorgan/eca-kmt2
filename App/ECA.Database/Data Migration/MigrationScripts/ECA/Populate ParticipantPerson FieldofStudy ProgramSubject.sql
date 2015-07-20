SELECT 
  cps.participant_sevis_id,cps.participant_id,
  cp.participant_id,cp.person_id,
  cep.*,
  birthcountry.locationid,birthcountry.locationname,
  birthcity.locationid,birthcity.locationname,birthcity.country_locationid,
  p.*,
  pt.*


FROM ce_participant_sevis cps
  LEFT JOIN ce_participant cp 
    ON (cp.participant_id = cps.participant_id)
  LEFT JOIN CE_person cep 
    ON (cep.person_id = cp.person_id)
  LEFT JOIN eca_dev.eca_dev.dbo.location birthcountry 
    ON (birthcountry.locationtypeid = 3 AND birthcountry.[locationname] = ce.[Birth_Country])
  LEFT JOIN eca_dev.eca_dev.dbo.location birthcity
    ON (birthcity.locationtypeid = 5 AND birthcity.locationname = vs.[Birth_City] AND birthcity.country_locationid = birthcountry.locationid)
  
  LEFT JOIN eca_dev.eca_dev.dbo.person p 
    ON 
  (
  ((cep.[First_Name] IS NULL AND p.firstname IS NULL) OR (p.firstname = cep.[First_Name])) AND
  ((cep.[Last_Name] IS NULL AND p.lastname IS NULL) OR (p.lastname = cep.[Last_Name])) AND
  ((cep.[Middle_Name] IS NULL AND p.middlename IS NULL) OR (p.middlename = cep.[Middle_Name])) AND
  ((cep.[Prefix_cd] IS NULL AND p.nameprefix IS NULL) OR (p.nameprefix = cep.[Prefix_cd])) AND
  ((cep.[Suffix_cd] IS NULL AND p.namesuffix IS NULL) OR (p.namesuffix = cep.[Suffix_cd])) AND
  ((cep.[Birth_date] IS NULL AND p.dateofbirth IS NULL) OR (CONVERT(DATE,cep.[Birth_date],1) = CONVERT(DATE,p.dateofbirth,1))) AND
  (p.placeofbirth_locationid = birthcity.locationid)
  )
  LEFT JOIN eca_dev.eca_dev.dbo.participant pt 
  ON (pt.personid = p.personid )

 --LEFT JOIN eca_dev.eca_dev.sevis.fieldofstudy f ON (f.description = vs.[Area of Specialization])
 WHERE pt.participantid IS NOT NULL
 ORDER BY pt.participantid