/* Reconcile Program themes */

/* Insert missing Program themes - rollup from Project themes */
INSERT INTO eca_dev.eca_dev.dbo.programtheme
	(programid,themeid)
SELECT programid,themeid
  FROM (SELECT p.ProgramId,pg.themeid
  	  FROM eca_dev.eca_dev.dbo.projecttheme pg
  	  LEFT OUTER JOIN eca_dev.eca_dev.dbo.project p ON (p.projectid = pg.projectid)) A
 	  WHERE NOT EXISTS (SELECT programid,themeid 
		              FROM programtheme A2 
		    	     WHERE A2.programid = A.programid AND A2.themeid = A.themeid)
 GROUP BY a.ProgramId,a.themeid

GO

/* Insert missing Parent Program themes - rollup from Program themes */
INSERT INTO eca_dev.eca_dev.dbo.programtheme
	(programid,themeid)
SELECT programid,themeid
  FROM (SELECT p.parentprogram_ProgramId programid,pg.themeid
  	  FROM eca_dev.eca_dev.dbo.programtheme pg
  	  LEFT OUTER JOIN eca_dev.eca_dev.dbo.program p	ON (p.programid = pg.programid)) A
	 WHERE NOT EXISTS (SELECT programid,themeid 
			     FROM programtheme A2 
			     WHERE A2.programid = A.programid AND A2.themeid = A.themeid)
	   AND a.programid IS NOT NULL
GROUP BY a.ProgramId,a.themeid

GO
 


