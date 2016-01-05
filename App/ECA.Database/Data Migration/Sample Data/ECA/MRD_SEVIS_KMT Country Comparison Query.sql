/* Complete cross referenced list MRD v. Sevis.BirthCountry v. ECA.Location */
 Select dtAllPrids.comp_prid   
      --,m.[Country or Area ID]
      --,m.[Public View IND]
      ,m.[Country or Area Name]
      --,m.[Country or Area Name Full]
      ,m.[Country or Area Code GENC 2]
      ,m.[Country or Area Code GENC 3]
	  --,b.birthcountryid
	  ,b.countrycode
	  ,b.countryname
	  --,l.locationid
	  ,l.locationname
	  ,l.locationiso
	  ,l.[locationiso-2]
From
(
  Select [Country or Area Name] As comp_prid From [ECA_Data_Migration].[dbo].[MRDCountryAreasList]
  Union
  Select countryname As comp_prid From eca_kmt_dev.eca_kmt_dev.[sevis].[BirthCountry]
  Union
  Select locationname As comp_prid From eca_kmt_dev.eca_kmt_dev.dbo.location where locationtypeid = 3
) dtAllPrids
Left Join [ECA_Data_Migration].[dbo].[MRDCountryAreasList] As m On (m.[Country or Area Name] = dtAllPrids.comp_prid)
Left Join eca_kmt_dev.eca_kmt_dev.[sevis].[BirthCountry] As b On (b.countryname = dtAllPrids.comp_prid)
Left Join eca_kmt_dev.eca_kmt_dev.dbo.location As l On (l.locationname = dtAllPrids.comp_prid AND l.locationtypeid = 3)
WHERE 
 order by dtAllPrids.comp_prid,m.[Country or Area Name],b.countryname,l.locationname
 
 /* Differences */
 Select dtAllPrids.comp_prid   
      --,m.[Country or Area ID]
      --,m.[Public View IND]
      ,m.[Country or Area Name]
      --,m.[Country or Area Name Full]
      ,m.[Country or Area Code GENC 2]
      ,m.[Country or Area Code GENC 3]
	  --,b.birthcountryid
	  ,b.countrycode
	  ,b.countryname
	  --,l.locationid
	  ,l.locationname
	  ,l.locationiso
	  ,l.[locationiso-2]
From
(
  Select [Country or Area Name] As comp_prid From [ECA_Data_Migration].[dbo].[MRDCountryAreasList]
  Union
  Select countryname As comp_prid From eca_kmt_dev.eca_kmt_dev.[sevis].[BirthCountry]
  Union
  Select locationname As comp_prid From eca_kmt_dev.eca_kmt_dev.dbo.location where locationtypeid = 3
) dtAllPrids
Left Join [ECA_Data_Migration].[dbo].[MRDCountryAreasList] As m On (m.[Country or Area Name] = dtAllPrids.comp_prid)
Left Join eca_kmt_dev.eca_kmt_dev.[sevis].[BirthCountry] As b On (b.countryname = dtAllPrids.comp_prid)
Left Join eca_kmt_dev.eca_kmt_dev.dbo.location As l On (l.locationname = dtAllPrids.comp_prid AND l.locationtypeid = 3)
WHERE (m.[Country or Area Name] IS NULL OR b.countryname IS NULL OR l.locationname IS NULL)
  OR (m.[Country or Area Code GENC 2] <> b.countrycode or m.[Country or Area Code GENC 2] <> l.[locationiso-2] OR b.countrycode <> l.[locationiso-2])
  OR (m.[Country or Area Code GENC 3] <> l.locationiso)
 order by dtAllPrids.comp_prid,m.[Country or Area Name],b.countryname,l.locationname