/* Inserts a list of recipient organizations into Organization table */
/* OrganizationTypes must have already been loaded into OrganizationType table */
INSERT INTO eca_dev.eca_dev.dbo.organization
   (organizationtypeid,
    description,
    status,
    name,
    website,
    history_createdby,
    History_CreatedOn,
    History_RevisedBy,
    History_RevisedOn)
SELECT o.OrganizationTypeId ,
       a.Recipient ,
       'Active', 
       a.recipient, 
       NULL, 
       0,
       N'2/2/2015 12:00:00 AM -05:00',
       0,
       N'2/2/2015 12:00:00 AM -05:00'
  FROM afcp a
  JOIN eca_dev.eca_dev.dbo.organizationtype o
    ON (o.organizationtypeName = a.[Recipient Type])
 WHERE a.recipient IS NOT NULL
 GROUP BY a.Recipient ,
          o.OrganizationTypeId 
 ORDER BY o.OrganizationTypeId ,
          a.Recipient



/* Using the participants table */
INSERT INTO eca_dev.eca_dev.dbo.organization
   (organizationtypeid,
    description,
    status,
    name,
    website,
    history_createdby,
    History_CreatedOn,
    History_RevisedBy,
    History_RevisedOn)
SELECT o.OrganizationTypeId ,
       a.institution ,
       'Active', 
       a.institution, 
       NULL, 
       0,
       N'2/12/2015 12:00:00 AM -05:00',
       0,
       N'2/12/2015 12:00:00 AM -05:00'
  FROM participants a
  JOIN eca_dev.eca_dev.dbo.organizationtype o
    ON (o.organizationtypeName = a.reciptype)
  LEFT OUTER JOIN eca_dev.eca_dev.dbo.organization o2 ON (o2.name = a.institution and o2.organizationtypeid = o.organizationtypeid)
 WHERE a.institution IS NOT NULL and a.reciptype <> 'Individual'
      AND o2.organizationtypeid IS NULL AND o2.name IS NULL
 GROUP BY a.institution ,
          o.OrganizationTypeId 
 ORDER BY o.OrganizationTypeId ,
          a.institution



   