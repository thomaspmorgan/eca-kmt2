/* See if any countries have missing ISO values */
select locationname from location where locationtypeid = 3 and ([locationiso-2] is null or locationiso is null)

/* Update the missing ones */
UPDATE location set [locationISO-2] = 'PS' where locationtypeid = 3 and locationname = 'Palestine, State of'
UPDATE location set [locationISO-2] = 'AX' where locationtypeid = 3 and locationname = 'Aland Islands'
UPDATE location set [locationISO-2] = 'SJ' where locationtypeid = 3 and locationname = 'Svalbard and Jan Mayen Islands'
UPDATE location set [locationISO-2] = 'UM' where locationtypeid = 3 and locationname = 'United States Minor Outlying Islands'
UPDATE location set [locationISO-2] = 'AN' where locationtypeid = 3 and locationname = 'Netherlands Antilles'
UPDATE location set [locationISO] = 'ANT' where locationtypeid = 3 and locationname = 'Netherlands Antilles'

/* See which do not have matching Birth Countries */
SELECT [LocationName]
      ,[LocationIso]
      ,[LocationISO-2]
      ,[SEVISCountryCodeId]
      ,b.BirthCountryId
      ,b.CountryCode
      ,b.CountryName
  FROM [dbo].[Location] l
  left outer join sevis.BirthCountry b on (b.BirthCountryId = l.SEVISCountryCodeId)
  where locationtypeid = 3 and l.SEVISCountryCodeId is null
  order by locationname

/* Update all where there is a name match */
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = location.locationname) where locationtypeid = 3	

/* update based on mappings */
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'CAPE VERDE') where locationtypeid = 3 and locationname = 'Cabo Verde'	
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'CANTON AND ENDERBURY') where locationtypeid = 3 and locationname = 'Canton and Enderbury Islands'	
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'COTE D IVOIRE') where locationtypeid = 3 and locationname = 'Cote d''Ivoire'	
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'FRENCH TERRITORY OF THE AFARS AND ISSAS') where locationtypeid = 3 and locationname = 'French Afar and Issas'	
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'FRENCH SOUTHERN AND ANTARCTIC LANDS') where locationtypeid = 3 and locationname = 'French Southern and Antarctic Territories'	
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'HEARD ISLAND AND MCDONALD SLANDS') where locationtypeid = 3 and locationname = 'Heard Island and Mcdonald Islands'	
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'JOHNSTON ATOLL') where locationtypeid = 3 and locationname = 'Johnston Island'	
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'NORTH KOREA') where locationtypeid = 3 and locationname = 'Korea, North'	
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'SOUTH KOREA') where locationtypeid = 3 and locationname = 'Korea, South'	
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'TRUST TERRITORY OF THE PACIFIC ISLANDS') where locationtypeid = 3 and locationname = 'Pacific Islands, Trust Territory of the'	
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'CANAL ZONE') where locationtypeid = 3 and locationname = 'Panama Canal Zone'	
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'SVALBARD AND JAN MAYEN') where locationtypeid = 3 and locationname = 'Svalbard and Jan Mayen Islands'	
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'UNITED STATES MISC. PACIFIC ISLANDS') where locationtypeid = 3 and locationname = 'U.S. Miscellaneous Pacific Islands'	
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'VIRGIN ISLANDS') where locationtypeid = 3 and locationname = 'U.S. Virgin Islands'	
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'DEMOCRATIC REPUBLIC OF VIET-NAM') where locationtypeid = 3 and locationname = 'Viet-Nam, Democratic Republic of'	
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'YEMEN') where locationtypeid = 3 and locationname = 'Yemen, Democratic'
update location set SEVISCountryCodeId = (select b.BirthCountryId from sevis.BirthCountry b where CountryName = 'YEMAN (SANAA)') where locationtypeid = 3 and locationname = 'Yemen'

