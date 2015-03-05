USE AFCP
GO

/* Select all non-individual participants */
INSERT INTO eca_dev.eca_dev.dbo.ParticipantProject
(ParticipantId,ProjectId)
SELECT p2.participantid,p.projectid --,count(*)
/*o.description,p1.institution,
       a.id,a.title,a.summary,
       p.projectid,p.name,p.description,
       ot.organizationtypeid,ot.organizationtypename,
       o.organizationid,o.description,
       p2.participantid,p2.organizationid,p2.personid,p2.participantTypeId,
       p1.* */
  FROM afcp a
  LEFT OUTER JOIN eca_dev.eca_dev.dbo.project p ON (p.name = a.title AND p.description = a.summary)
  LEFT OUTER JOIN participants p1 ON (p1.Project_ID = a.id)
  LEFT OUTER JOIN eca_dev.eca_dev.dbo.organizationtype ot ON (ot.organizationtypename = p1.reciptype)
  LEFT OUTER JOIN eca_dev.eca_dev.dbo.organization o ON (o.organizationtypeid = ot.organizationtypeid AND o.description = p1.institution)
  LEFT OUTER JOIN eca_dev.eca_dev.dbo.participant p2 ON (p2.organizationid = o.organizationid AND p2.personid IS NULL) 
 WHERE p1.reciptype <> 'Individual'
 GROUP BY p2.participantid,p.projectid
 ORDER BY p.projectid,p2.participantid

GO


USE AFCP
GO

/* Select all non-individual participants */
INSERT INTO eca_dev.eca_dev.dbo.ParticipantProject
(ParticipantId,ProjectId)
SELECT p2.participantid,p.projectid --,count(*)--,
/*--o.description,
   -- p1.institution,
       a.id,a.title,a.summary,
       p.projectid,p.name,p.description,
	   c.personid,c.lastname,c.firstname,c.prefix,c.suffix,
 --      ot.organizationtypeid,ot.organizationtypename,
 --      o.organizationid,o.description,
       p2.participantid,p2.organizationid,p2.personid,p2.participantTypeId,
       p1.* */ 
  FROM afcp a
  LEFT OUTER JOIN eca_dev.eca_dev.dbo.project p ON (p.name = a.title AND p.description = a.summary)
  LEFT OUTER JOIN participants p1 ON (p1.Project_ID = a.id)
  LEFT OUTER JOIN eca_dev.eca_dev.dbo.combinednamevw c 
               ON ((c.lastname = p1.[Name Last] OR (c.lastname IS NULL AND p1.[Name Last] IS NULL)) AND  
                   (c.firstname = p1.[Name First] OR (c.firstname IS NULL AND p1.[Name First] IS NULL)) AND
		   (c.prefix = p1.prefix OR (c.prefix IS NULL AND p1.prefix IS NULL)) AND 
		   (c.suffix = p1.suffix OR (c.suffix IS NULL AND p1.suffix IS NULL)))
  --LEFT OUTER JOIN eca_dev.eca_dev.dbo.organizationtype ot ON (ot.organizationtypename = p1.reciptype)
  --LEFT OUTER JOIN eca_dev.eca_dev.dbo.organization o ON (o.organizationtypeid = ot.organizationtypeid AND o.description = p1.institution)
  LEFT OUTER JOIN eca_dev.eca_dev.dbo.participant p2 ON (p2.personid = c.personid ) 
 WHERE p1.reciptype = 'Individual' AND p2.personid IS NOT NULL
 GROUP BY p2.participantid,p.projectid
 ORDER BY p.projectid,p2.participantid

GO