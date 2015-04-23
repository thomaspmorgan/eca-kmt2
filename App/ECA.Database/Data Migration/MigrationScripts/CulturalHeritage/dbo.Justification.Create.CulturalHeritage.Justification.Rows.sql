/* Create Justification Data */
USE AFCP
GO

/* Create the Justification */
INSERT INTO eca_dev.eca_dev.dbo.justification
  (JustifcationName,history_createdon,history_createdby,history_revisedon,history_revisedby,
   officeid)
SELECT r.ratcat,
  	N'4/9/2015 12:00:00 AM -05:00',0,N'4/9/2015 12:00:00 AM -05:00',0,
	o.organizationid
  FROM [AFCP].[dbo].[ratcat] r
  JOIN eca_dev.eca_dev.dbo.organization o ON (o.name = 'Cultural Heritage Center')
GO