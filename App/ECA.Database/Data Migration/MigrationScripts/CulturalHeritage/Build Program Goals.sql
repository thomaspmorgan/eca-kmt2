/* Build the list of program goals */
USE AFCP
GO

INSERT INTO eca_dev.eca_dev.dbo.programgoal
       (programid,goalid)
SELECT p.ParentProgram_ProgramId,
       pg.goalid
  FROM eca_dev.eca_dev.dbo.projectgoal pg
  LEFT OUTER JOIN eca_dev.eca_dev.dbo.project p 
    ON (p.projectid = pg.projectid)
 GROUP BY p.ParentProgram_ProgramId,pg.goalid
 ORDER BY p.ParentProgram_ProgramId,pg.goalid
GO


