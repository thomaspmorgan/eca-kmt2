/* Update region locationid based on countryid */
UPDATE dbo.location 
SET region_locationid = 
(SELECT l.region_locationid 
FROM dbo.location l 
WHERE l.locationid = location.country_locationid)
WHERE country_locationid IS NOT NULL



/* Update country region id based on DEV data */
UPDATE eca_kmt_qa.eca_kmt_qa.dbo.location 
   SET region_locationid = 
      (SELECT l.region_locationid 
         FROM eca_kmt_dev.eca_kmt_dev.dbo.location l 
        WHERE l.locationtypeid = location.locationtypeid 
		AND ((l.locationname = location.locationname AND l.locationiso IS NULL AND location.locationiso IS NULL) OR l.locationiso = location.locationiso) 
		AND ((l.locationname = location.locationname AND l.[locationiso-2] IS NULL AND location.[locationiso-2] IS NULL) OR l.[locationiso-2] = location.[locationiso-2]))
 WHERE locationtypeid = 3  