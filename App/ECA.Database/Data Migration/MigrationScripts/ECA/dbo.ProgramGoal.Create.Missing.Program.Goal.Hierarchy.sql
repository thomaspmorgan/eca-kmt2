/* Reconcile Program Goals */

/* Insert missing Program Goals - rollup from Project Goals */
INSERT INTO eca_dev.eca_dev.dbo.programgoal
	(programid,goalid)
SELECT programid,goalid
  FROM (SELECT p.ProgramId,pg.goalid
  	  FROM eca_dev.eca_dev.dbo.projectgoal pg
  	  LEFT OUTER JOIN eca_dev.eca_dev.dbo.project p ON (p.projectid = pg.projectid)) A
 	  WHERE NOT EXISTS (SELECT programid,goalid 
		              FROM programgoal A2 
		    	     WHERE A2.programid = A.programid AND A2.goalid = A.goalid)
 GROUP BY a.ProgramId,a.goalid

GO

/* Insert missing Parent Program Goals - rollup from Program Goals */
INSERT INTO eca_dev.eca_dev.dbo.programgoal
	(programid,goalid)
SELECT programid,goalid
  FROM (SELECT p.parentprogram_ProgramId programid,pg.goalid
  	  FROM eca_dev.eca_dev.dbo.programgoal pg
  	  LEFT OUTER JOIN eca_dev.eca_dev.dbo.program p	ON (p.programid = pg.programid)) A
	 WHERE NOT EXISTS (SELECT programid,goalid 
			     FROM programgoal A2 
			     WHERE A2.programid = A.programid AND A2.goalid = A.goalid)
	   AND a.programid IS NOT NULL
GROUP BY a.ProgramId,a.goalid

GO
 


