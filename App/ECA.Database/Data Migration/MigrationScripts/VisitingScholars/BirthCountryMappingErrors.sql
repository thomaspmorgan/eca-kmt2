/****** Script for SelectTopNRows command from SSMS  ******/
SELECT 'UPDATE dbo.location set country_locationid = ' + CAST(l3.locationid AS NVARCHAR) + ' WHERE locationid = ' + CAST(l.locationid as NVARCHAR) + ';',  
f.[Birth City],f.[Birth Country Name]
,b2.BirthCountryId,b2.CountryCode,b2.CountryName
,l.locationid,l.locationname,l.Country_LocationId
,l2.locationid,l2.locationname,l2.locationiso,l2.[LocationISO-2],l2.SEVISCountryCodeId
,l3.locationid,l3.locationname,l3.locationiso,l3.[LocationISO-2],l3.SEVISCountryCodeId
,b.BirthCountryId,b.CountryCode,b.CountryName
,f.*
,p.*
  FROM [Fulbright].[dbo].[FulbrightDataset2_Import] f
  LEFT OUTER JOIN eca_local.dbo.gender g ON (g.SevisGenderCode = f.[Gender ])
  LEFT OUTER JOIN eca_local.dbo.person p ON ((p.FirstName IS NULL AND f.[First Name] is null or p.FirstName = f.[First Name])
  AND (p.LastName IS NULL AND f.[Last Name] is null or p.LastName = f.[Last Name])
  AND (p.MiddleName IS NULL AND f.[Middle Name] is null or p.MiddleName = f.[Middle Name])
  AND (p.NamePrefix IS NULL AND f.Prefix is null or p.NamePrefix = f.Prefix)
  AND (p.NameSuffix IS NULL AND f.Suffix is null or p.NameSuffix = f.Suffix)
  AND (p.SecondLastName is null and f.[Second Last Name] is null or p.SecondLastName = f.[Second Last Name])
  AND (p.GenderId is null and f.[Gender ] is null or p.genderid = g.GenderId))
  LEFT OUTER JOIN eca_local.dbo.location l ON (l.locationid = p.PlaceOfBirth_LocationId)
  LEFT OUTER JOIN eca_local.dbo.location l2 ON (l2.locationid = l.Country_LocationId)
  LEFT OUTER JOIN eca_local.sevis.birthcountry b ON (b.birthcountryid = l2.SEVISCountryCodeId)
  LEFT OUTER JOIN eca_local.sevis.birthcountry b2 ON (b2.CountryCode = f.[Birth Country Name])
  LEFT OUTER JOIN eca_local.dbo.location l3 ON (l3.locationtypeid = 3 AND l3.SEVISCountryCodeId = b2.BirthCountryId)
  where subcategory <> 'Specialist'
  and b2.CountryName <> l2.locationname





/* Missing mapping to Sevis.BirthCountry */
SELECT TOP 1000 [BirthCountryId]
      ,[CountryCode]
      ,[CountryName]
      ,l.locationid,l.locationname,l.locationiso,l.[LocationISO-2],l.SEVISCountryCodeId
      ,'INSERT INTO [dbo].[Location]([LocationTypeId],[LocationName],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_RevisedOn],[IsActive],[SEVISCountryCodeId]) VALUES (3,'''+[CountryName]+''',1,SYSDATETIMEOFFSET(),1,SYSDATETIMEOFFSET(),0,'+CAST([BirthCountryId] as NVARCHAR)+');'
  FROM [ECA_Local].[sevis].[BirthCountry] b
  left outer join eca_local.dbo.location l ON (l.locationtypeid = 3 and l.SEVISCountryCodeId = b.BirthCountryId)
  where locationid is null
  order by CountryName