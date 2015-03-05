USE VisitingScholar
GO

/* Create all the Participant Data from VisitingScholarData table */
INSERT INTO eca_dev.eca_dev.dbo.ParticipantProject
    (ParticipantId,ProjectId)
SELECT --vs.[Last Name],vs.[First Name],p.lastname,p.firstname,
       pt.ParticipantId,
       pj.projectid--,
       --p.PersonId--,
       --vs.[Program Description],
	   --pj.name
  FROM VisitingScholarData vs
  LEFT JOIN eca_dev.eca_dev.dbo.project pj ON (pj.name = vs.[Program Description])
  LEFT JOIN eca_dev.eca_dev.dbo.person p ON ((vs.[First Name] IS NULL OR (p.firstname = vs.[First Name])) AND
  (vs.[Last Name] IS NULL OR (p.lastname = vs.[Last Name])) AND
  (vs.[Prefix] IS NULL OR (p.nameprefix = vs.[Prefix])) AND
  (vs.[Suffix] IS NULL OR (p.namesuffix = vs.[Suffix])) AND
  (vs.[Second Last Name] IS NULL OR (p.secondlastname = vs.[Second Last Name])) AND
  (vs.[Middle Name] IS NULL OR (p.middlename = vs.[Middle Name])))
  LEFT JOIN eca_dev.eca_dev.dbo.participant pt ON (pt.personid = p.personid)
 ORDER BY pt.participantid

GO
