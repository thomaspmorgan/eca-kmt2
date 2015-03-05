begin tran t1
DECLARE @regionLocationTypeID int = 0;
DECLARE @regionLocationID int = 0; 
DECLARE @CountryLocationTypeID int = 0;
DECLARE @CountryLocationID int = 0; 

/* get the correct ID for the region location type */ 
SELECT @RegionLocationTypeID = locationtypeid 
  FROM locationtype 
 WHERE locationTypeName = 'Region'

/* get the correct ID for the Country location type */ 
SELECT @CountryLocationTypeID = locationtypeid 
  FROM locationtype 
 WHERE locationTypeName = 'Country'

/* Get the correct Region Location ID for Europe and Eurasia */
SELECT @regionLocationId = locationid
  FROM location
 WHERE locationtypeid = @regionLocationTypeID AND locationiso = 'EUR'

/* output to be sure */
SELECT @regionLocationTypeID AS 'Region Location Type ID'
SELECT @regionLocationID AS 'Region Location ID'
SELECT @CountryLocationTypeID AS 'Country Location Type ID'

/* Update the region code for these countries */
UPDATE location 
   SET region_locationid = @regionLocationId 
 WHERE locationtypeid = @CountryLocationTypeId 
   AND locationname IN (
 
'Albania'
 ,
'Andorra'
 ,
'Armenia'
, 
'Austria'
 ,
'Azerbaijan'
, 
'Belarus'
, 
'Belgium'
, 
'Bosnia and Herzegovina'
 ,
'Bulgaria'
 ,
'Croatia'
 ,
'Cyprus'
, 
'Czech Republic'
 ,
'Denmark'
, 
'Estonia'
, 
'European Union'
, 
'Finland'
, 
'France'
 ,
'Georgia'
, 
'Germany'
 ,
'Greece'
, 
'Holy See'
 ,
'Hungary'
, 
'Iceland'
 ,
'Ireland'
, 
'Italy'
 ,
'Kosovo'
, 
'Latvia'
 ,
'Liechtenstein'
, 
'Lithuania'
 ,
'Luxembourg'
, 
'Macedonia'
 ,
'Malta'
, 
'Moldova'
 ,
'Monaco'
, 
'Montenegro'
 ,
'Netherlands'
 ,
'Norway'
 ,
'Poland'
 ,
'Portugal'
, 
'Romania'
 ,
'Russia'
, 
'San Marino'
 ,
'Serbia'
 ,
'Slovakia'
 ,
'Slovenia'
 ,
'Spain'
 ,
'Sweden'
 ,
'Switzerland'
 ,
'Turkey'
 ,
'Ukraine'
 ,
'United Kingdom'

)
commit tran t1
 
