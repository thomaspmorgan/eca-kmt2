/* Update region locationid based on countryid */
UPDATE dbo.location 
SET region_locationid = 
(SELECT l.region_locationid 
FROM dbo.location l 
WHERE l.locationid = location.country_locationid)
WHERE country_locationid IS NOT NULL


/* ******************************************** */
/* Determine if country Data is in sync */
SELECT l.locationname,l2.locationname,
	l.region_locationid,l2.region_locationid,
	l.locationiso,l2.locationiso,
	* 
FROM eca_kmt_dev.eca_kmt_dev.dbo.location l
LEFT OUTER JOIN eca_kmt_pre.eca_kmt_pre.dbo.location l2 ON (l2.locationtypeid = l.locationtypeid 
        AND ((l2.locationname = l.locationname AND l2.locationiso IS NULL AND l.locationiso IS NULL) OR l2.locationiso = l.locationiso) 
	AND ((l2.locationname = l.locationname AND l2.[locationiso-2] IS NULL AND l.[locationiso-2] IS NULL) OR l2.[locationiso-2] = l.[locationiso-2]))
WHERE l.locationtypeid = 3 --and l.locationname <> l2.locationname
--and l2.locationname is null 
ORDER BY l.locationname


/* Update country region id based on DEV data */
UPDATE eca_kmt_qa.eca_kmt_qa.dbo.location 
   SET region_locationid = 
      (SELECT l.region_locationid 
         FROM eca_kmt_dev.eca_kmt_dev.dbo.location l 
        WHERE l.locationtypeid = location.locationtypeid 
		AND ((l.locationname = location.locationname AND l.locationiso IS NULL AND location.locationiso IS NULL) OR l.locationiso = location.locationiso) 
		AND ((l.locationname = location.locationname AND l.[locationiso-2] IS NULL AND location.[locationiso-2] IS NULL) OR l.[locationiso-2] = location.[locationiso-2]))
 WHERE locationtypeid = 3  