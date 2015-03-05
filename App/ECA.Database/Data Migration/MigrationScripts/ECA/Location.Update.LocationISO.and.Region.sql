USE VisitingScholar
GO
Declare @CountryName nvarchar(255),@CountryISO nvarchar(255),@CountryMission nvarchar(255), @CountryRegion nvarchar(255)
Declare @RegionLocationId int,@RegionName nvarchar(max),@RegionISO nvarchar(max)
Declare @LocationName nvarchar(max),@LocationISO nvarchar(max),@LocationRegionId int,@LocationId int
Declare @cursorCountry CURSOR

/* Define the cursor */
set @cursorCountry = CURSOR FOR
SELECT c.locationname,c.locationiso,c.mission,c.regionname,
       Region.locationid,Region.locationname,Region.locationiso,
       ISO.locationid,ISO.locationname,ISO.locationiso,ISO.region_locationid 
  FROM countryisomissionregion c
LEFT JOIN eca_dev_backup.dbo.location ISO ON (ISO.locationname = c.locationname AND ISO.locationtypeid = 3)
LEFT JOIN eca_dev_backup.dbo.location Region ON (Region.locationname = c.regionname AND Region.locationtypeid = 2)
--  LEFT JOIN eca_dev.eca_dev.dbo.location ISO ON (ISO.locationname = c.locationname AND ISO.locationtypeid = 3)
--  LEFT JOIN eca_dev.eca_dev.dbo.location Region ON (Region.locationname = c.regionname AND Region.locationtypeid = 2)

/* Open the cursor */
OPEN @cursorCountry

/* Fetch the first project */
FETCH NEXT FROM @cursorCountry INTO @CountryName,@CountryISO,@CountryMission,@CountryRegion,
                                    @RegionLocationId,@RegionName,@RegionISO,
                                    @LocationId,@LocationName,@LocationISO,@LocationRegionId

/* Loop thru all projects (staging) - should match DB */
WHILE @@FETCH_STATUS = 0
BEGIN

/* update the Country ISO and Region */
IF @LocationId IS NOT NULL
--  UPDATE eca_dev.eca_dev.dbo.location 
  UPDATE eca_dev_backup.dbo.location
     SET locationiso = @CountryISO,
         region_locationid = @RegionLocationId 
   WHERE locationid = @LocationId
--ELSE
--  INSERT...

/* Fetch the next project */
FETCH NEXT FROM @cursorCountry INTO @CountryName,@CountryISO,@CountryMission,@CountryRegion,
                                    @RegionLocationId,@RegionName,@RegionISO,
                                    @LocationId,@LocationName,@LocationISO,@LocationRegionId
END

/* Cleanup */
CLOSE @cursorCountry
DEALLOCATE @cursorCountry
GO




'ABW',	'Aruba'
'AFG',	'Afghanistan'
'AGO',	'Angola'
'AIA',	'Anguilla'
'ALA',	'Åland Islands'
'ALB',	'Albania'
'AND',	'Andorra'
'ARE',	'United Arab Emirates'
'ARG',	'Argentina'
'ARM',	'Armenia'
'ASM',	'American Samoa'
'ATA',	'Antarctica'
'ATF',	'French Southern Territories'
'ATG',	'Antigua and Barbuda'
'AUS',	'Australia'
'AUT',	'Austria'
'AZE',	'Azerbaijan'
'BDI',	'Burundi'
'BEL',	'Belgium'
'BEN',	'Benin'
'BES',	'Bonaire, Sint Eustatius and Saba'
'BFA',	'Burkina Faso'
'BGD',	'Bangladesh'
'BGR',	'Bulgaria'
'BHR',	'Bahrain'
'BHS',	'Bahamas'
'BIH',	'Bosnia and Herzegovina'
'BLM',	'Saint Barthélemy'
'BLR',	'Belarus'
'BLZ',	'Belize'
'BMU',	'Bermuda'
'BOL',	'Bolivia, Plurinational State of'
'BRA',	'Brazil'
'BRB',	'Barbados'
'BRN',	'Brunei Darussalam'
'BTN',	'Bhutan'
'BVT',	'Bouvet Island'
'BWA',	'Botswana'
'CAF',	'Central African Republic'
'CAN',	'Canada'
'CCK',	'Cocos (Keeling) Islands'
'CHE',	'Switzerland'
'CHL',	'Chile'
'CHN',	'China'
'CIV',	'Côte d'Ivoire'
'CMR',	'Cameroon'
'COD',	'Congo, the Democratic Republic of the'
'COG',	'Congo'
'COK',	'Cook Islands'
'COL',	'Colombia'
'COM',	'Comoros'
'CPV',	'Cabo Verde'
'CRI',	'Costa Rica'
'CUB',	'Cuba'
'CUW',	'Curaçao'
'CXR',	'Christmas Island'
'CYM',	'Cayman Islands'
'CYP',	'Cyprus'
'CZE',	'Czech Republic'
'DEU',	'Germany'
'DJI',	'Djibouti'
'DMA',	'Dominica'
'DNK',	'Denmark'
'DOM',	'Dominican Republic'
'DZA',	'Algeria'
'ECU',	'Ecuador'
'EGY',	'Egypt'
'ERI',	'Eritrea'
'ESH',	'Western Sahara'
'ESP',	'Spain'
'EST',	'Estonia'
'ETH',	'Ethiopia'
'FIN',	'Finland'
'FJI',	'Fiji'
'FLK',	'Falkland Islands (Malvinas)'
'FRA',	'France'
'FRO',	'Faroe Islands'
'FSM',	'Micronesia, Federated States of'
'GAB',	'Gabon'
'GBR',	'United Kingdom'
'GEO',	'Georgia'
'GGY',	'Guernsey'
'GHA',	'Ghana'
'GIB',	'Gibraltar'
'GIN',	'Guinea'
'GLP',	'Guadeloupe'
'GMB',	'Gambia'
'GNB',	'Guinea-Bissau'
'GNQ',	'Equatorial Guinea'
'GRC',	'Greece'
'GRD',	'Grenada'
'GRL',	'Greenland'
'GTM',	'Guatemala'
'GUF',	'French Guiana'
'GUM',	'Guam'
'GUY',	'Guyana'
'HKG',	'Hong Kong'
'HMD',	'Heard Island and McDonald Islands'
'HND',	'Honduras'
'HRV',	'Croatia'
'HTI',	'Haiti'
'HUN',	'Hungary'
'IDN',	'Indonesia'
'IMN',	'Isle of Man'
'IND',	'India'
'IOT',	'British Indian Ocean Territory'
'IRL',	'Ireland'
'IRN',	'Iran, Islamic Republic of'
'IRQ',	'Iraq'
'ISL',	'Iceland'
'ISR',	'Israel'
'ITA',	'Italy'
'JAM',	'Jamaica'
'JEY',	'Jersey'
'JOR',	'Jordan'
'JPN',	'Japan'
'KAZ',	'Kazakhstan'
'KEN',	'Kenya'
'KGZ',	'Kyrgyzstan'
'KHM',	'Cambodia'
'KIR',	'Kiribati'
'KNA',	'Saint Kitts and Nevis'
'KOR',	'Korea, Republic of'
'KWT',	'Kuwait'
'LAO',	'Lao People's Democratic Republic'
'LBN',	'Lebanon'
'LBR',	'Liberia'
'LBY',	'Libya'
'LCA',	'Saint Lucia'
'LIE',	'Liechtenstein'
'LKA',	'Sri Lanka'
'LSO',	'Lesotho'
'LTU',	'Lithuania'
'LUX',	'Luxembourg'
'LVA',	'Latvia'
'MAC',	'Macao'
'MAF',	'Saint Martin (French part)'
'MAR',	'Morocco'
'MCO',	'Monaco'
'MDA',	'Moldova, Republic of'
'MDG',	'Madagascar'
'MDV',	'Maldives'
'MEX',	'Mexico'
'MHL',	'Marshall Islands'
'MKD',	'Macedonia, the former Yugoslav Republic of'
'MLI',	'Mali'
'MLT',	'Malta'
'MMR',	'Myanmar'
'MNE',	'Montenegro'
'MNG',	'Mongolia'
'MNP',	'Northern Mariana Islands'
'MOZ',	'Mozambique'
'MRT',	'Mauritania'
'MSR',	'Montserrat'
'MTQ',	'Martinique'
'MUS',	'Mauritius'
'MWI',	'Malawi'
'MYS',	'Malaysia'
'MYT',	'Mayotte'
'NAM',	'Namibia'
'NCL',	'New Caledonia'
'NER',	'Niger'
'NFK',	'Norfolk Island'
'NGA',	'Nigeria'
'NIC',	'Nicaragua'
'NIU',	'Niue'
'NLD',	'Netherlands'
'NOR',	'Norway'
'NPL',	'Nepal'
'NRU',	'Nauru'
'NZL',	'New Zealand'
'OMN',	'Oman'
'PAK',	'Pakistan'
'PAN',	'Panama'
'PCN',	'Pitcairn'
'PER',	'Peru'
'PHL',	'Philippines'
'PLW',	'Palau'
'PNG',	'Papua New Guinea'
'POL',	'Poland'
'PRI',	'Puerto Rico'
'PRK',	'Korea, Democratic People's Republic of'
'PRT',	'Portugal'
'PRY',	'Paraguay'
'PSE',	'Palestine, State of'
'PYF',	'French Polynesia'
'QAT',	'Qatar'
'REU',	'Réunion'
'ROU',	'Romania'
'RUS',	'Russian Federation'
'RWA',	'Rwanda'
'SAU',	'Saudi Arabia'
'SDN',	'Sudan'
'SEN',	'Senegal'
'SGP',	'Singapore'
'SGS',	'South Georgia and the South Sandwich Islands'
'SHN',	'Saint Helena, Ascension and Tristan da Cunha'
'SJM',	'Svalbard and Jan Mayen'
'SLB',	'Solomon Islands'
'SLE',	'Sierra Leone'
'SLV',	'El Salvador'
'SMR',	'San Marino'
'SOM',	'Somalia'
'SPM',	'Saint Pierre and Miquelon'
'SRB',	'Serbia'
'SSD',	'South Sudan'
'STP',	'Sao Tome and Principe'
'SUR',	'Suriname'
'SVK',	'Slovakia'
'SVN',	'Slovenia'
'SWE',	'Sweden'
'SWZ',	'Swaziland'
'SXM',	'Sint Maarten (Dutch part)'
'SYC',	'Seychelles'
'SYR',	'Syrian Arab Republic'
'TCA',	'Turks and Caicos Islands'
'TCD',	'Chad'
'TGO',	'Togo'
'THA',	'Thailand'
'TJK',	'Tajikistan'
'TKL',	'Tokelau'
'TKM',	'Turkmenistan'
'TLS',	'Timor-Leste'
'TON',	'Tonga'
'TTO',	'Trinidad and Tobago'
'TUN',	'Tunisia'
'TUR',	'Turkey'
'TUV',	'Tuvalu'
'TWN',	'Taiwan, Province of China'
'TZA',	'Tanzania, United Republic of'
'UGA',	'Uganda'
'UKR',	'Ukraine'
'UMI',	'United States Minor Outlying Islands'
'URY',	'Uruguay'
'USA',	'United States'
'UZB',	'Uzbekistan'
'VAT',	'Holy See (Vatican City State)'
'VCT',	'Saint Vincent and the Grenadines'
'VEN',	'Venezuela, Bolivarian Republic of'
'VGB',	'Virgin Islands, British'
'VIR',	'Virgin Islands, U.S.'
'VNM',	'Viet Nam'
'VUT',	'Vanuatu'
'WLF',	'Wallis and Futuna'
'WSM',	'Samoa'
'YEM',	'Yemen'
'ZAF',	'South Africa'
'ZMB',	'Zambia'
'ZWE',	'Zimbabwe'


('ABW',	
'AFG',	
'AGO',	
'AIA',	
'ALA',	
'ALB',	
'AND',	
'ARE',	
'ARG',	
'ARM',	
'ASM',	
'ATA',	
'ATF',	
'ATG',	
'AUS',	
'AUT',	
'AZE',	
'BDI',	
'BEL',	
'BEN',	
'BES',	
'BFA',	
'BGD',	
'BGR',	
'BHR',	
'BHS',	
'BIH',	
'BLM',	
'BLR',	
'BLZ',	
'BMU',	
'BOL',	
'BRA',	
'BRB',	
'BRN',	
'BTN',	
'BVT',	
'BWA',	
'CAF',	
'CAN',	
'CCK',	
'CHE',	
'CHL',	
'CHN',	
'CIV',	
'CMR',	
'COD',	
'COG',	
'COK',	
'COL',	
'COM',	
'CPV',	
'CRI',	
'CUB',	
'CUW',	
'CXR',	
'CYM',	
'CYP',	
'CZE',	
'DEU',	
'DJI',	
'DMA',	
'DNK',	
'DOM',	
'DZA',	
'ECU',	
'EGY',	
'ERI',	
'ESH',	
'ESP',	
'EST',	
'ETH',	
'FIN',	
'FJI',	
'FLK',	
'FRA',	
'FRO',	
'FSM',	
'GAB',	
'GBR',	
'GEO',	
'GGY',	
'GHA',	
'GIB',	
'GIN',	
'GLP',	
'GMB',	
'GNB',	
'GNQ',	
'GRC',	
'GRD',	
'GRL',	
'GTM',	
'GUF',	
'GUM',	
'GUY',	
'HKG',	
'HMD',	
'HND',	
'HRV',	
'HTI',	
'HUN',	
'IDN',	
'IMN',	
'IND',	
'IOT',	
'IRL',	
'IRN',	
'IRQ',	
'ISL',	
'ISR',	
'ITA',	
'JAM',	
'JEY',	
'JOR',	
'JPN',	
'KAZ',	
'KEN',	
'KGZ',	
'KHM',	
'KIR',	
'KNA',	
'KOR',	
'KWT',	
'LAO',	
'LBN',	
'LBR',	
'LBY',	
'LCA',	
'LIE',	
'LKA',	
'LSO',	
'LTU',	
'LUX',	
'LVA',	
'MAC',	
'MAF',	
'MAR',	
'MCO',	
'MDA',	
'MDG',	
'MDV',	
'MEX',	
'MHL',	
'MKD',	
'MLI',	
'MLT',	
'MMR',	
'MNE',	
'MNG',	
'MNP',	
'MOZ',	
'MRT',	
'MSR',	
'MTQ',	
'MUS',	
'MWI',	
'MYS',	
'MYT',	
'NAM',	
'NCL',	
'NER',	
'NFK',	
'NGA',	
'NIC',	
'NIU',	
'NLD',	
'NOR',	
'NPL',	
'NRU',	
'NZL',	
'OMN',	
'PAK',	
'PAN',	
'PCN',	
'PER',	
'PHL',	
'PLW',	
'PNG',	
'POL',	
'PRI',	
'PRK',	
'PRT',	
'PRY',	
'PSE',	
'PYF',	
'QAT',	
'REU',	
'ROU',	
'RUS',	
'RWA',	
'SAU',	
'SDN',	
'SEN',	
'SGP',	
'SGS',	
'SHN',	
'SJM',	
'SLB',	
'SLE',	
'SLV',	
'SMR',	
'SOM',	
'SPM',	
'SRB',	
'SSD',	
'STP',	
'SUR',	
'SVK',	
'SVN',	
'SWE',	
'SWZ',	
'SXM',	
'SYC',	
'SYR',	
'TCA',	
'TCD',	
'TGO',	
'THA',	
'TJK',	
'TKL',	
'TKM',	
'TLS',	
'TON',	
'TTO',	
'TUN',	
'TUR',	
'TUV',	
'TWN',	
'TZA',	
'UGA',	
'UKR',	
'UMI',	
'URY',	
'USA',	
'UZB',	
'VAT',	
'VCT',	
'VEN',	
'VGB',	
'VIR',	
'VNM',	
'VUT',	
'WLF',	
'WSM',	
'YEM',	
'ZAF',	
'ZMB',	
'ZWE')	





