USE VisitingScholar
GO

/* Create all the Participant Data from VisitingScholarData table */
INSERT INTO eca_dev.eca_dev.dbo.EmailAddress
  (Address,EmailAddressTypeID,Contact_ContactId,Person_PersonId)
SELECT --vs.[Last Name],vs.[First Name],p.lastname,p.firstname,
       vs.[Home Email],
	   et.emailaddresstypeid,
	--   et.emailaddresstypename,
	   NULL,
       p.PersonId
  FROM VisitingScholarData vs
  JOIN eca_dev.eca_dev.dbo.person p ON ((vs.[First Name] IS NULL OR (p.firstname = vs.[First Name])) AND
  (vs.[Last Name] IS NULL OR (p.lastname = vs.[Last Name])) AND
  (vs.[Prefix] IS NULL OR (p.nameprefix = vs.[Prefix])) AND
  (vs.[Suffix] IS NULL OR (p.namesuffix = vs.[Suffix])) AND
  (vs.[Second Last Name] IS NULL OR (p.secondlastname = vs.[Second Last Name])) AND
  (vs.[Middle Name] IS NULL OR (p.middlename = vs.[Middle Name])))
  JOIN eca_dev.eca_dev.dbo.emailaddresstype et ON (et.emailaddresstypename = 'Home')
 WHERE vs.[Home Email] IS NOT NULL
 ORDER BY vs.[Last Name],vs.[First Name]

GO
