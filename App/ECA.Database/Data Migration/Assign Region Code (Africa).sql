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

/* Get the correct Region Location ID for Africa */
SELECT @regionLocationId = locationid
  FROM location
 WHERE locationtypeid = @regionLocationTypeID AND locationiso = 'AF'

/* output to be sure */
SELECT @regionLocationTypeID AS 'Region Location Type ID'
SELECT @regionLocationID AS 'Region Location ID'
SELECT @CountryLocationTypeID AS 'Country Location Type ID'

/* Update the region code for these countries */
UPDATE location 
   SET region_locationid = @regionLocationId 
 WHERE locationtypeid = @CountryLocationTypeId 
   AND locationname IN ( 
 
'Angola',
 
'Benin',
 
'Botswana',
 
'Burkina Faso',
 
'Burundi',
 
'Cabo Verde',
 
'Cameroon',
 
'Central African Republic',
 
'Chad',
 
'Comoros',
 
'Congo, Democratic Republic of the (D.R.C.)',
 
'Congo, Republic of the',
 
'Cote d''Ivoire',
 
'Djibouti',
 
'Equatorial Guinea',
 
'Eritrea',
 
'Ethiopia',
 
'Gabon',
 
'Gambia, The',
 
'Ghana',
 
'Guinea',
 
'Guinea-Bissau',
 
'Kenya',
 
'Lesotho',
 
'Liberia',
'Madagascar',
 
'Malawi',
 
'Mali',
 
'Mauritania',
 
'Mauritius',
 
'Mozambique',
 
'Namibia',
 
'Niger',
 
'Nigeria',
 
'Rwanda',
 
'Sao Tome and Principe',
 
'Senegal',
 
'Seychelles',
 
'Sierra Leone',
 
'Somalia',
 
'South Africa',
 
'South Sudan',
 
'Sudan',
 
'Swaziland',
 
'Tanzania',
 
'Uganda',
 
'Togo',
 
'Zambia',
 
'Zimbabwe')


/* These get missed for some reason? */
UPDATE location 
   SET region_locationid = @regionLocationID 
 WHERE locationtypeid = @CountryLocationTypeID 
   AND locationiso IN ('CF','CG','MI','IV')

GO
 







