USE VisitingScholar
GO

/* Create all the Person Data from VisitingScholarData table */
INSERT INTO eca_dev.eca_dev.dbo.Person
(FirstName,LastName,SecondLastName,NamePrefix,NameSuffix,GivenName,FamilyName,MiddleName,Patronym,Alias,
 Gender,DateOfBirth,Ethnicity,PermissionToContact,EvaluationRetention,
 History_CreatedBy,History_CreatedOn,History_RevisedBy,History_RevisedOn,
 Location_LocationId,MedicalConditions,Awards)
SELECT vs.[First Name],
       vs.[Last Name],
       vs.[Second Last Name],
       vs.Prefix,
       vs.Suffix,
       NULL,NULL,
       vs.[Middle Name],
       NULL,NULL,
       g.genderid,
       [Date of Birth],
       Ethnicity,
       1,NULL,
       0,N'3/2/2015 12:00:00 AM -05:00',0, N'3/2/2015 12:00:00 AM -05:00',
       NULL,NULL,NULL
  FROM VisitingScholarData vs
  LEFT JOIN eca_dev.eca_dev.dbo.gender g ON (g.Gendername = vs.[Gender ])
 ORDER BY [Last Name],[First Name]

GO
