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

/* Get the correct Region Location ID for South and Central Asia */
SELECT @regionLocationId = locationid
  FROM location
 WHERE locationtypeid = @regionLocationTypeID AND locationiso = 'SCA'

/* output to be sure */
SELECT @regionLocationTypeID AS 'Region Location Type ID'
SELECT @regionLocationID AS 'Region Location ID'
SELECT @CountryLocationTypeID AS 'Country Location Type ID'

/* Update the region code for these countries */
UPDATE location 
   SET region_locationid = @regionLocationId 
 WHERE locationtypeid = @CountryLocationTypeId 
   AND locationname IN (

'Afghanistan'
 ,
'Bangladesh'
 ,
'Bhutan'
 ,
'India'
 ,
'Kazakhstan'
 ,
'Kyrgyzstan'
 ,
'Maldives'
 ,
'Nepal'
 ,
'Pakistan'
 ,
'Sri Lanka'
 ,
'Tajikistan'
 ,
'Turkmenistan'
 ,
'Uzbekistan'



)
commit tran t1
 GO
