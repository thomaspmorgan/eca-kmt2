/* Create Project to Region mappings for Visiting Scholars */
USE VisitingScholar
GO

/* DataMigrationXREF.dbo.Vw_CountryCodeXREF is a view that maps 2-char Country Codes to 3-char codes */
/* The source data contains only 2-char, but DB codes are 3-char */

/* insert */
INSERT INTO eca_dev.eca_dev.dbo.ProjectRegion
	(ProjectId,LocationId)
SELECT --[Home Institution Country Name],
	--vs.[Program Description],
	--p.name, 
	--lt.locationtypeid,
	--l.locationname,
	--l.locationiso,
	--cx.countryname,
	--cx.ISOCode2,
	--cx.ISOCode3,
	--l1.locationiso,
	--l1.locationname,
	p.projectid,
	l.region_locationid
  FROM VisitingScholarData vs
  JOIN eca_dev.eca_dev.dbo.locationtype lt ON (lt.locationtypename = 'Country')
  JOIN eca_dev.eca_dev.dbo.project p ON (p.name = vs.[Program Description])
  JOIN DataMigrationXREF.dbo.Vw_CountryCodeXREF cx ON (cx.ISOCode2 = vs.[Home Institution Country Name])
  LEFT JOIN eca_dev.eca_dev.dbo.location l ON (l.locationtypeid = lt.locationtypeid AND l.locationiso = cx.ISOCode3)
--  LEFT JOIN join eca_dev.eca_dev.dbo.location l1 ON (l1.locationid = l.region_locationid)

GO