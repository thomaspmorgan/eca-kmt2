
/* Update Divisions for existing cities using 1st entry from MRD database */
/* Step 2 will be adding missing city+division entries */
/* Step 3 will be reassigning cityID based on Location text data */
/* Step 4 will be assigning divisionID */
UPDATE L 
SET l.division  =
(
SELECT top 1 [US State Abbrev] 
FROM mrdcitylocationsdata m 
WHERE m.[domestic or foreign ind] = 'D' and 
m.[Location Name] = l.locationname 
)
FROM eca_dev.eca_dev.dbo.location l
WHERE l.locationtypeid = 5 and l.locationname is not null and l.division is null


/* Added 3-3-2016 this updates division ids in a different db from dev */
UPDATE  b
SET     b.division_locationid = a.division_locationid
FROM    eca_kmt_dev.eca_kmt_dev.dbo.location a
        INNER JOIN eca_kmt_qa.eca_kmt_qa.dbo.location b
            ON (a.locationtypeid = b.locationtypeid AND a.locationname = b.locationname and a.country_locationid = b.country_locationid and a.division = b.division)
WHERE b.locationtypeid = 5 AND b.country_locationid = 193 and b.division IS NOT NULL AND b.locationname is not null