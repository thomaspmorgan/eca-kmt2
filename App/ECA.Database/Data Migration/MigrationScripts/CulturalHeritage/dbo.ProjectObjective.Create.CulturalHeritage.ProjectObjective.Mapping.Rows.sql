/* Create Project-Objective Mapping Data for AFCP */
USE AFCP
GO

/* Create the Project-Objectives Mapping */
INSERT INTO eca_dev.eca_dev.dbo.ProjectObjective
  (ProjectId,ObjectiveID)
--SELECT ar.*,a.id,a.Title,p.projectid,p.name,r.ID,r.Rationale,r.Rationale_Number,o.objectiveid,o.objectivename
SELECT p.projectid,o.objectiveid
FROM AFCPRationales ar
JOIN AFCP a ON (a.id = ar.AFCP_ID)
JOIN eca_dev.eca_dev.dbo.project p ON (p.name = a.title)
JOIN Rationales r ON (r.ID = ar.Rationales_ID)
JOIN eca_dev.eca_dev.dbo.objective o ON (o.objectivename = r.rationale)
WHERE ar.Rationales_ID IS NOT NULL
ORDER BY p.projectid,o.objectiveid

GO

/* Create the Program-Objectives Mapping */
INSERT INTO eca_dev.eca_dev.dbo.ProgramObjective
  (ProgramId,ObjectiveID)
--select ar.*,a.id,a.Title,p.projectid,p.name,p.programid,r.ID,r.Rationale,r.Rationale_Number,o.objectiveid,o.objectivename
--select p.projectid,o.objectiveid
select p.programid,o.objectiveid
from AFCPRationales ar
JOIN AFCP a ON (a.id = ar.AFCP_ID)
JOIN eca_dev.eca_dev.dbo.project p ON (p.name = a.title)
JOIN Rationales r ON (r.ID = ar.Rationales_ID)
JOIN eca_dev.eca_dev.dbo.objective o ON (o.objectivename = r.rationale)
WHERE ar.Rationales_ID IS NOT NULL
GROUP BY p.programid,o.objectiveid

GO

/* Create the ParentProgram-Objectives Mapping */
INSERT INTO eca_dev.eca_dev.dbo.ProgramObjective
  (ProgramId,ObjectiveID)
SELECT pg.parentprogram_programid,o.objectiveid
FROM AFCPRationales ar
JOIN AFCP a ON (a.id = ar.AFCP_ID)
JOIN eca_dev.eca_dev.dbo.project p ON (p.name = a.title)
JOIN eca_dev.eca_dev.dbo.program pg ON (pg.programid = p.programid)
JOIN Rationales r ON (r.ID = ar.Rationales_ID)
JOIN eca_dev.eca_dev.dbo.objective o ON (o.objectivename = r.rationale)
WHERE ar.Rationales_ID IS NOT NULL
GROUP BY pg.parentprogram_programid,o.objectiveid

GO