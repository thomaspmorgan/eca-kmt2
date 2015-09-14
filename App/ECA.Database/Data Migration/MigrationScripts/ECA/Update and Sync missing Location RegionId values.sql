/* Update region locationid based on countryid */
UPDATE dbo.location 
SET region_locationid = 
(SELECT l.region_locationid 
FROM dbo.location l 
WHERE l.locationid = location.country_locationid)
WHERE country_locationid IS NOT NULL 