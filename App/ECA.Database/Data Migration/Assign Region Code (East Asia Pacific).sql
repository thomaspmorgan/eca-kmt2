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

/* Get the correct Region Location ID for East Asia Pacific */
SELECT @regionLocationId = locationid
  FROM location
 WHERE locationtypeid = @regionLocationTypeID AND locationiso = 'EAP'

/* output to be sure */
SELECT @regionLocationTypeID AS 'Region Location Type ID'
SELECT @regionLocationID AS 'Region Location ID'
SELECT @CountryLocationTypeID AS 'Country Location Type ID'

/* Update the region code for these countries */
UPDATE location 
   SET region_locationid = @regionLocationId 
 WHERE locationtypeid = @CountryLocationTypeId 
   AND locationname IN ( 
 

'Australia'
,
'Brunei'
, 
'Burma'
, 
'Cambodia'
, 
'China'
, 
'Fiji'
, 
'Indonesia'
, 
'Japan'
, 
'Kiribati'
, 
'Korea, North'
, 
'Korea, South'
, 
'Laos'
, 
'Malaysia'
, 
'Marshall Islands'
, 
'Micronesia, Federated States of'
, 
'Mongolia'
, 
'Nauru'
, 
'New Zealand'
, 
'Palau'
, 
'Papua New Guinea'
, 
'Philippines'
, 
'Samoa'
, 
'Singapore'
, 
'Solomon Islands'
, 
'Taiwan'
, 
'Thailand'
, 
'Timor-Leste'
, 
'Tonga'
, 
'Tuvalu'
, 
'Vanuatu'
, 
'Vietnam'
)
commit tran t1
GO
 
