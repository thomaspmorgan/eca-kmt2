select iep.IVLP_Person_Id,iep.ECA_PersonId,
p.person_id,p.birth_city,p.birth_country,
ep.personid,ep.placeofbirth_locationid,
l1.locationid,l1.locationname,
l.locationid,l.locationname,l.country_locationid

--select iep.*,p.*,ep.*,l1.*,l.*
SELECT 'UPDATE eca_dev.eca_dev.dbo.person SET placeofbirth_locationid = '+CAST(l.locationid as NVARCHAR)+' WHERE personid = '+CAST(iep.ECA_PersonId AS NVARCHAR)
from IVLP_ECA_Person_XRef iep
JOIN ivlp_person p ON (p.person_id = iep.ivlp_person_id)
JOIN eca_dev.eca_dev.dbo.person ep ON (ep.personid = iep.ECA_PersonId)
LEFT JOIN eca_dev.eca_dev.dbo.location l1 
  ON (l1.locationtypeid = 3 AND l1.locationname = p.birth_country)
LEFT JOIN ECA_Dev.eca_dev.dbo.location l ON (l.locationtypeid = 5 AND 
        /* Birth City Null and birth country Exists */
     	((l.locationname IS NULL AND p.birth_city IS NULL AND l.country_locationid = l1.locationid) OR
        /* Birth City Exists and Birth Country does not exist OR Birth Country DOES exist */      
	(l.locationname = p.BIRTH_CITY AND ((l.country_locationid IS NULL AND l1.locationid IS NULL) OR l.country_locationid = l1.locationid))))
WHERE (p.birth_city IS NOT NULL OR p.birth_country IS NOT NULL) --AND ep.placeofbirth_locationid <> l.locationid
