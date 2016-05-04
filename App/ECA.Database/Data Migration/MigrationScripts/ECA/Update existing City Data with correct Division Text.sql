
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

/* Added 4-21-2016 to update division field to "standardize" with US and non-US cities */
UPDATE L 
SET l.division  =
(
SELECT top 1 m.[locationiso-2] 
FROM eca_kmt_dev.eca_kmt_dev.dbo.location m 
WHERE m.locationtypeid = 4  and 
m.[locationid] = l.division_locationid 
)
FROM eca_kmt_dev.eca_kmt_dev.dbo.location l
WHERE l.locationtypeid = 5 and l.locationname is not null and l.division_locationid is not null

/* Added 3-3-2016 this updates division ids in a different db from dev */
UPDATE  b
SET     b.division_locationid = a.division_locationid
FROM    eca_kmt_dev.eca_kmt_dev.dbo.location a
        INNER JOIN eca_kmt_qa.eca_kmt_qa.dbo.location b
            ON (a.locationtypeid = b.locationtypeid AND a.locationname = b.locationname and a.country_locationid = b.country_locationid and a.division = b.division)
WHERE b.locationtypeid = 5 AND b.country_locationid = 193 and b.division IS NOT NULL AND b.locationname is not null


/* Update foreign divisions from worldcities */
/* DivisionID is not yet assigned */
select 'UPDATE dbo.location SET division_locationid = '+cast(div.locationid as nvarchar)+ ',division = '''+div.[locationiso-2]+''' WHERE locationid = '+cast(l.locationid as nvarchar)
,l.* ,
l1.*,
c.*,
div.*
from eca_kmt_dev.eca_kmt_dev.dbo.location l
join eca_kmt_dev.eca_kmt_dev.dbo.location l1 on (l1.locationid = l.country_locationid)
join [ECA_Data_Migration].[dbo].[WorldCities] c on (c.[ISO 3166-1 country code] = l1.[LocationISO-2] and LTRIM(c.[ name]) = LTRIM(l.locationname))
join eca_kmt_dev.eca_kmt_dev.dbo.location div on (div.locationtypeid = 4 and div.[LocationISO-2] = c.[Full_DivisionCode])
where l.locationtypeid = 5 and l.locationname IS NOT NULL and l.country_locationid <> 193 and (l.division IS NULL AND l.division_locationid IS NULL OR l.division_locationid <> div.locationid OR l.division <> div.[locationiso-2])
--group by l.locationid,div.[locationiso-2],div.locationid
--order by l.locationid,c.[ GNS FD],div.locationid

/* Division has been assigned */
select 'UPDATE dbo.location SET division_locationid = '+cast(div.locationid as nvarchar)+ ',division = '''+div.[locationiso-2]+''' WHERE locationid = '+cast(l.locationid as nvarchar)
--,l.* ,
--l1.*,
--c.*,
--div.*
from eca_kmt_dev.eca_kmt_dev.dbo.location l
join eca_kmt_dev.eca_kmt_dev.dbo.location l1 on (l1.locationid = l.country_locationid)
join [ECA_Data_Migration].[dbo].[WorldCities] c on (c.[ISO 3166-1 country code] = l1.[LocationISO-2] and LTRIM(c.[ name]) = LTRIM(l.locationname))
join eca_kmt_dev.eca_kmt_dev.dbo.location div on (div.locationtypeid = 4 and div.[LocationISO-2] = c.[Full_DivisionCode])
where l.locationtypeid = 5 and l.locationname is not null and l.country_locationid <> 193 and (l.division IS NULL AND l.division_locationid IS NOT NULL OR l.division_locationid <> div.locationid)
group by l.locationid,div.[locationiso-2],div.locationid
order by l.locationid,div.locationid


/* Update divisions of us cities */
select 'UPDATE dbo.location SET division_locationid = '+cast(div.locationid as nvarchar)+ ',division = '''+div.[locationiso-2]+''' WHERE locationid = '+cast(l.locationid as nvarchar)
,l.* ,
l1.*,
l2.*,
c.*,
div.*
from eca_kmt_dev.eca_kmt_dev.dbo.location l
join eca_kmt_dev.eca_kmt_dev.dbo.location l1 on (l1.locationid = l.country_locationid)
join eca_kmt_dev.eca_kmt_dev.dbo.location l2 on (l2.locationid = l.division_locationid)
join [ECA_Data_Migration].[dbo].[WorldCities] c on (c.[ISO 3166-1 country code] = l1.[LocationISO-2] and c.[ name] = l.locationname AND c.full_divisioncode = l2.[locationiso-2])
join eca_kmt_dev.eca_kmt_dev.dbo.location div on (div.locationtypeid = 4 and div.[LocationISO-2] = c.[Full_DivisionCode] /*AND div.locationiso = l2.LocationIso*/)
where l.locationtypeid = 5 and l.locationname LIKE 'A%' and l.country_locationid = 193 and (l.division IS NULL AND l.division_locationid IS NOT NULL OR l.division_locationid <> div.locationid)
--group by l.locationid,div.[locationiso],div.locationid
order by l.locationid,div.locationid

/* Update us division iso-2 fields to match worldcities */
update eca_kmt_prod.eca_kmt_prod.dbo.location 
set [LocationISO-2] = (select DivisionCodeFinal from [ECA_Data_Migration].[dbo].[FIPS_5-2_Division] m where m.statecode = [locationiso])
where locationtypeid = 4 and country_locationid = 193
