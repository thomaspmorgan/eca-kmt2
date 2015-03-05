/* This script moves Branch programs to Projects */
/*   NOT USED!!!   */
INSERT 
  INTO eca_dev.eca_dev.dbo.project
       (name,description,projecttype,status,focusarea,startdate,enddate,language,audiencereach,eventid,
        history_createdby,history_createdon,history_revisedby,history_revisedon,nominationsource_organizationid,
        parentprogram_programid)
SELECT p.name,
       ISNULL(fps.Description,fps.[Program Name]),
       0,
       0,
       NULL,
       p.startdate,
       p.enddate,
       NULL,
       0,
       NULL,
       p.History_CreatedBy,
       p.History_CreatedOn,
       p.History_RevisedBy,
       p.History_RevisedOn, 
       NULL,
       p2.ProgramId
  FROM eca_dev.eca_dev.dbo.program p
  JOIN FullProgramStaging fps on (fps.[Program Name] = p.name)
  JOIN eca_dev.eca_dev.dbo.program p2 on (p2.Name = fps.[Parent Programs])
 WHERE fps.type = 'branch'
GO

/* Move existing data from ProgramRegion to ProjectRegion */
INSERT INTO eca_dev.eca_dev.dbo.projectregion
SELECT p.programid,p.locationid
  FROM eca_dev.eca_dev.dbo.programregion p
  JOIN FullProgramStaging fps on (fps.[Program Name] = p.name)
  JOIN eca_dev.eca_dev.dbo.program p2 on (p2.Name = fps.[Parent Programs])  /* must be here!!! */
 WHERE fps.type = 'branch')

/* Now delete those same rows from the Program table */
DELETE FROM eca_dev.eca_dev.dbo.program
 WHERE programid IN (      
SELECT p.programid
  FROM eca_dev.eca_dev.dbo.program p
  JOIN FullProgramStaging fps on (fps.[Program Name] = p.name)
  JOIN eca_dev.eca_dev.dbo.program p2 on (p2.Name = fps.[Parent Programs])  /* must be here!!! */
 WHERE fps.type = 'branch')
GO