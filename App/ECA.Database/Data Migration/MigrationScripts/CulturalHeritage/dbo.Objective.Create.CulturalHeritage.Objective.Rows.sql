/* Create Objective Data */
USE AFCP
GO

/* Create the Objectives */
INSERT INTO eca_dev.eca_dev.dbo.objective
  (JustificationId,ObjectiveName,history_createdon,history_createdby,history_revisedon,history_revisedby)
SELECT  j.justificationid,r.Rationale,
  	N'4/9/2015 12:00:00 AM -05:00',0,N'4/9/2015 12:00:00 AM -05:00',0
  FROM [AFCP].[dbo].[Rationales] r
  JOIN [AFCP].[dbo].[ratcat] rc ON (rc.ID = r.RatCat_ID)
  JOIN eca_dev.eca_dev.dbo.justification j ON (j.justifcationname = rc.RatCat)
  ORDER BY j.justificationid,r.rationale

GO