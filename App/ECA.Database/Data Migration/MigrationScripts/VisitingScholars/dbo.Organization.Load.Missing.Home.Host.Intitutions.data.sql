USE VisitingScholar
GO

/* Inserts missing foreign educational Orgs from visitingscholardata */
INSERT 
INTO Eca_dev.eca_dev.[dbo].[Organization] 
	([OrganizationTypeId], 
	[Description], 
	[Status], 
	[Name], 
	[Website], 
	[History_CreatedBy], 
	[History_CreatedOn], 
	[History_RevisedBy], 
	[History_RevisedOn],
	ParentOrganization_organizationid)
SELECT o.organizationtypeid,  -- all are foreign educational institutions - this may change from db to db */
	vs.[Home Institution Name],
 	N'Active',
	vs.[Home Institution Name],
	NULL,
	0, 
	N'3/2/2015 12:00:00 AM -05:00', 
	0, N'3/2/2015 12:00:00 AM -05:00',
	NULL
   FROM VisitingScholarData vs
   JOIN eca_dev.eca_dev.dbo.organizationtype o ON (o.organizationtypename = 'Foreign Educational Institution')
  WHERE [Home Institution Name] NOT IN
		(SELECT o.description 
                   FROM eca_dev.eca_dev.dbo.organizationtype ot
                   JOIN eca_dev.eca_dev.dbo.organization o ON (o.organizationtypeid = ot.organizationtypeid) 
                  WHERE ot.organizationtypename = 'Foreign Educational Institution')
GROUP BY o.organizationtypeid,
	vs.[Home Institution Name]
ORDER BY vs.[Home Institution Name] 


/* Need to add the missing Host Institutions also */
INSERT 
INTO eca_dev.eca_dev.[dbo].[Organization] 
	([OrganizationTypeId], 
	[Description], 
	[Status], 
	[Name], 
	[Website], 
	[History_CreatedBy], 
	[History_CreatedOn], 
	[History_RevisedBy], 
	[History_RevisedOn],
	ParentOrganization_organizationid)
SELECT o.organizationtypeid,  -- all are US educational institutions - this may change from db to db */
	vs.[Host Institution Name],
 	N'Active',
	vs.[Host Institution Name],
	NULL,
	0, 
	N'3/2/2015 12:00:00 AM -05:00', 
	0, N'3/2/2015 12:00:00 AM -05:00',
	NULL
   FROM VisitingScholarData vs
   JOIN eca_dev.eca_dev.dbo.organizationtype o ON (o.organizationtypename = 'U.S. Educational Institution')
  WHERE [Host Institution Name] NOT IN
		(SELECT o.description 
                   FROM eca_dev.eca_dev.dbo.organizationtype ot
                   JOIN eca_dev.eca_dev.dbo.organization o ON (o.organizationtypeid = ot.organizationtypeid) 
                  WHERE ot.organizationtypename = 'U.S. Educational Institution')
GROUP BY o.organizationtypeid,
	vs.[Host Institution Name]
ORDER BY vs.[Host Institution Name] 



GO



 

