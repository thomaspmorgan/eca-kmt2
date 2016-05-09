/* update latitude and longitude */

UPDATE eca_local.dbo.location 
SET latitude = (SELECT TOP 1 m.[ latitude] 
                  FROM [dbo].[WorldCities] m 
                 WHERE m.[ name] = eca_local.dbo.location.locationname),
longitude = (SELECT TOP 1 m.[ longitude] 
               FROM [dbo].[WorldCities] m 
              WHERE m.[ name] = eca_local.dbo.location.locationname)
WHERE locationtypeid = 5 --and locationname like 'B%'


/* Try again */
update eca_kmt_dev.eca_kmt_dev.dbo.location 
set latitude = (select TOP 1 m.[ latitude] 
                  from [dbo].[WorldCities] m 
                 where m.[ name] = [locationname]),
longitude = (select TOP 1 m.[ longitude] 
               from [dbo].[WorldCities] m 
              where m.[ name] = [locationname])
where locationtypeid = 5 and locationname IS NOT NULL --like 'B%'


/* We have to match Division in the location table, so here we go */
select 'UPDATE eca_local.dbo.location SET latitude = '+CAST(m.[ latitude] AS NVARCHAR)+',longitude = '+CAST(m.[ longitude] AS NVARCHAR)+' WHERE locationtypeid = 5 AND locationid = '+CAST(l.locationid AS NVARCHAR)+' AND division_locationid = '+CAST(div.locationid AS NVARCHAR)--,
--* 
from eca_local.dbo.location l
join eca_local.dbo.location div ON (div.locationid = l.division_locationid)
join [ECA_Data_Migration].[dbo].[WorldCities] m ON (LTRIM(m.[ name]) = LTRIM(l.locationname) AND m.[Full_DivisionCode] = div.[locationiso-2])
where l.locationtypeid = 5 AND l.locationname IS NOT NULL
order by l.locationname,m.[ gns fd]

/* Update the locationiso-2 in location table to be division (FIPS 5-2) */
update eca_local.dbo.location 
set [LocationISO-2] = (select DivisionCodeFinal 
                         from [ECA_Data_Migration].[dbo].[FIPS_5-2_Division] m 
                        where m.statecode = [locationiso])
where locationtypeid = 4 and country_locationid = 193


select 'UPDATE dbo.location SET latitude = '+CAST(m.[ latitude] AS NVARCHAR)+',longitude = '+CAST(m.[ longitude] AS NVARCHAR)+' WHERE locationtypeid = 5 AND locationname = '''+l.locationname+''' AND division_locationid = '+CAST(div.locationid AS NVARCHAR)
--,l.*,div.*,m.*
from eca_local.dbo.location l
left join eca_local.dbo.location div ON (div.locationid = l.division_locationid)
join [ECA_Data_Migration].[dbo].[WorldCities] m ON (m.[ name] = l.locationname AND m.[Full_DivisionCode] = div.[locationiso-2])
where l.locationtypeid = 5 and l.locationname LIKE 'A%'--and m.[ GNS FD] like 'PPL%'
order by l.locationname
