/* Build the list of program themes */
USE ECA_Dev_Local_Copy
GO

INSERT INTO eca_dev.eca_dev.dbo.programtheme
       (programid,themeid)
SELECT p.ProgramId,
       pt.themeid
  FROM eca_dev.eca_dev.dbo.projecttheme pt
  LEFT OUTER JOIN eca_dev.eca_dev.dbo.project p 
    ON (p.projectid = pt.projectid)
WHERE p.projectid > 1549
 GROUP BY p.ProgramId,pt.themeid
 ORDER BY p.ProgramId,pt.themeid

GO


