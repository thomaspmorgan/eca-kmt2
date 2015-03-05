USE ECA_DEV
GO

/* Assign FocusId to project based on Focus table values */
UPDATE project 
   SET focusid = f.focusid
  FROM project p
  JOIN focus f ON f.FocusName = p.focusarea

GO
