USE VisitingScholar
GO

/* Create all the Participant Data from VisitingScholarData table */
INSERT INTO eca_dev.eca_dev.dbo.PhoneNumber
  (Number,PhoneNumberTypeId,Contact_ContactId,Person_PersonId)
SELECT --vs.[Last Name],vs.[First Name],p.lastname,p.firstname,
       vs.[Home Phone],
	   pt.phonenumbertypeid,
	--   pt.phonenumbertypename,
	   NULL,
       p.PersonId
  FROM VisitingScholarData vs
  JOIN eca_dev.eca_dev.dbo.person p ON ((vs.[First Name] IS NULL OR (p.firstname = vs.[First Name])) AND
  (vs.[Last Name] IS NULL OR (p.lastname = vs.[Last Name])) AND
  (vs.[Prefix] IS NULL OR (p.nameprefix = vs.[Prefix])) AND
  (vs.[Suffix] IS NULL OR (p.namesuffix = vs.[Suffix])) AND
  (vs.[Second Last Name] IS NULL OR (p.secondlastname = vs.[Second Last Name])) AND
  (vs.[Middle Name] IS NULL OR (p.middlename = vs.[Middle Name])))
  JOIN eca_dev.eca_dev.dbo.phonenumbertype pt ON (pt.phonenumbertypename = 'Home')
 WHERE vs.[Home Phone] IS NOT NULL
 ORDER BY vs.[Last Name],vs.[First Name]

GO
