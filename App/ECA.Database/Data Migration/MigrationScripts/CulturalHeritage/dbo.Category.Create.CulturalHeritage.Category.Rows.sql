/* Create Category Data */
USE AFCP
GO

/* Create the categories */
INSERT INTO eca_dev.eca_dev.dbo.category
  (Focusid,categoryname,history_createdon,history_createdby,history_revisedon,history_revisedby)
  SELECT f.focusid,c.category,
  	N'4/9/2015 12:00:00 AM -05:00',0,N'4/9/2015 12:00:00 AM -05:00',0
  FROM [AFCP].[dbo].[Categories] c
  JOIN [AFCP].[dbo].[FocusAreas] fa ON (fa.id = c.focusareaid)
  JOIN eca_dev.eca_dev.dbo.focus f ON (f.focusname = fa.focusarea)

GO