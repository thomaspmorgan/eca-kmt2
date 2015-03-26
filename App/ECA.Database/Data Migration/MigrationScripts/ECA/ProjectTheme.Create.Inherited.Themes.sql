/* This will insert "inherited themes" for all projects from the program */

INSERT INTO dbo.projecttheme
(projectid,themeid)
--SELECT pt.programid,pt.themeid,p.projectid,p.programid,pp.projectid,pp.themeid,
SELECT p.projectid,pt.themeid
FROM programtheme pt 
JOIN project p ON (p.programid = pt.programid)
LEFT OUTER JOIN projecttheme pp ON (pp.projectid = p.projectid AND pp.themeid = pt.themeid)
WHERE pp.projectid IS NULL    /* THIS IS IMPORTANT */
ORDER BY p.projectid,pt.themeid

GO


/* This is for programs, but don't think it applies */
INSERT INTO dbo.programtheme
(programid,themeid)
--SELECT pt.programid,pt.themeid,p.parentprogram_programid,p.programid,pp.programid,pp.themeid,
SELECT p.programid,pt.themeid
FROM programtheme pt 
JOIN program p ON (p.parentprogram_programid = pt.programid)
LEFT OUTER JOIN programtheme pp ON (pp.programid = p.programid AND pp.themeid = pt.themeid)
WHERE pp.programid IS NULL
ORDER BY p.programid,pt.themeid

GO

