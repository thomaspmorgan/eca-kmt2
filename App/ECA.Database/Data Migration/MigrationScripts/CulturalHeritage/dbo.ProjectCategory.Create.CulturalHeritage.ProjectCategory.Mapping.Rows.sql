/* Create Project-Category Mapping Data for AFCP */
USE AFCP
GO

/* Create the Project-Category Mapping */
INSERT INTO eca_dev.eca_dev.dbo.ProjectCategory
  (ProjectId,CategoryID)
--SELECT a.id,a.title,a.CategoryID,p.projectid,p.name,c.ID,c.Category,c.FocusAreaID,c1.categoryid,c1.categoryname
SELECT p.projectid,c1.categoryid
FROM AFCP a
JOIN eca_dev.eca_dev.dbo.project p ON (p.name = a.title AND (p.description = a.summary OR (p.description = 'No Description' AND a.summary IS NULL)))
JOIN Categories c ON (c.ID = a.CategoryID)
JOIN eca_dev.eca_dev.dbo.category c1 ON (c1.Categoryname = c.Category)
GROUP BY p.projectid,c1.categoryid
ORDER BY p.projectid,c1.categoryid

GO

/* Create the Program-Category Mapping */
INSERT INTO eca_dev.eca_dev.dbo.ProgramCategory
  (ProjectId,CategoryID)
SELECT p.programid,c1.categoryid
FROM AFCP a
JOIN eca_dev.eca_dev.dbo.project p ON (p.name = a.title AND (p.description = a.summary OR (p.description = 'No Description' AND a.summary IS NULL)))
JOIN Categories c ON (c.ID = a.CategoryID)
JOIN eca_dev.eca_dev.dbo.category c1 ON (c1.Categoryname = c.Category)
GROUP BY p.programid,c1.categoryid
ORDER BY p.programid,c1.categoryid

GO

/* Create the ParentProgram-Category Mapping */
INSERT INTO eca_dev.eca_dev.dbo.ProgramCategory
  (ProjectId,CategoryID)
SELECT pg.parentprogram_programid,c1.categoryid
FROM AFCP a
JOIN eca_dev.eca_dev.dbo.project p ON (p.name = a.title AND (p.description = a.summary OR (p.description = 'No Description' AND a.summary IS NULL)))
JOIN Categories c ON (c.ID = a.CategoryID)
JOIN eca_dev.eca_dev.dbo.category c1 ON (c1.Categoryname = c.Category)
JOIN eca_dev.eca_dev.dbo.program pg ON (pg.programid = p.programid)
GROUP BY pg.parentprogram_programid,c1.categoryid
ORDER BY pg.parentprogram_programid,c1.categoryid

GO