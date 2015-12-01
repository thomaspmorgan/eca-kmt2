/* Reconcile Countries w/ MRD */

/* Identify rows that should change */
SELECT m.[public view ind],m.[country or area name],m.[Country or Area Code GENC 3],m.[Country or Area Code GENC 2],
	l.locationiso,l.[LocationISO-2],l.locationname,l.locationid
  FROM MRDCountryAreasList m
  left outer join eca_kmt_dev.eca_kmt_dev.dbo.location l ON (l.locationtypeid = 3 and l.locationiso = m.[Country or Area Code GENC 3] 
  and l.[LocationISO-2] =  m.[Country or Area Code GENC 2])
  where l.locationname <> m.[Country or Area Name]
  order by m.[Country or Area Code GENC 3]


/* Update ISO 3 and 2 codes for Kosovo - incorrect in KMT DB */
update eca_local.dbo.location 
set locationiso = 'XKS',
    [LocationISO-2] = 'XK' 
where locationtypeid = 3 and locationname = 'Kosovo'


/* Updates location names for countries to match mrd data */
UPDATE
    l
SET
    l.locationname = m.[Country or Area Name]
FROM
    eca_local.dbo.location l   -- eca_kmt_dev.eca_kmt_dev.dbo.location l
INNER JOIN
    dbo.MRDCountryAreasList m 
ON
    m.[Country or Area Code GENC 3] = l.locationiso AND l.[LocationISO-2] = m.[Country or Area Code GENC 2] 
WHERE 
    l.locationtypeid = 3 AND l.locationname <> m.[country or Area Name]