USE VisitingScholar
GO

/* Create all the Participant Data from VisitingScholarData table */
INSERT INTO eca_dev.eca_dev.dbo.Participant
  (OrganizationId,PersonId,ParticipantTypeId,
   History_CreatedBy,History_CreatedOn,History_RevisedBy,History_RevisedOn)
SELECT o.organizationid,
       p.PersonId,
       pt.ParticipantTypeId,
       0,N'3/2/2015 12:00:00 AM -05:00',0, N'3/2/2015 12:00:00 AM -05:00'
  FROM VisitingScholarData vs
  JOIN eca_dev.eca_dev.dbo.person p ON ((vs.[First Name] IS NULL OR (p.firstname = vs.[First Name])) AND
  (vs.[Last Name] IS NULL OR (p.lastname = vs.[Last Name])) AND
  (vs.[Prefix] IS NULL OR (p.nameprefix = vs.[Prefix])) AND
  (vs.[Suffix] IS NULL OR (p.namesuffix = vs.[Suffix])) AND
  (vs.[Second Last Name] IS NULL OR (p.secondlastname = vs.[Second Last Name])) AND
  (vs.[Middle Name] IS NULL OR (p.middlename = vs.[Middle Name])))
  JOIN eca_dev.eca_dev.dbo.participanttype pt ON (pt.name = 'Individual')
  LEFT JOIN eca_dev.eca_dev.dbo.organization o ON (o.description = vs.[Home Institution Name])
 ORDER BY vs.[Last Name],vs.[First Name]

GO
