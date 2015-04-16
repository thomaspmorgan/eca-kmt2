/* Create Project to Region mappings for IVLP Pilot */
USE IVLP
GO

/* Could only use Funding Source to create these mappings - no country or region data in source */

/* insert */
INSERT INTO eca_dev.eca_dev.dbo.ProjectRegion
	(ProjectId,LocationId)
VALUES (1533,1),
	(1534,1),
	(1535,3),
	(1536,3),
	(1537,2),
	(1538,2),
	(1539,4),
	(1540,5),
	(1541,6)        

GO

