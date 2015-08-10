
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