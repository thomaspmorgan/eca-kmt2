USE VisitingScholar
GO

/* Create all the Participant Data from VisitingScholarData table */
INSERT INTO eca_kmt_dev.eca_kmt_dev.[dbo].[Participant]
           ([OrganizationId]
           ,[PersonId]
           ,[ParticipantTypeId]
           ,[History_CreatedBy]
           ,[History_CreatedOn]
           ,[History_RevisedBy]
           ,[History_RevisedOn]
           ,[ParticipantStatusId]
           ,[StatusDate]
           ,[ProjectId]
           ,[IVLP_ParticipantId])
SELECT NULL,
       p.PersonId,
       pt.ParticipantTypeId,
       0,
	   N'3/2/2015 12:00:00 AM -05:00',
	   0, 
	   N'3/2/2015 12:00:00 AM -05:00',
	   1,
	   CASE 
	   WHEN vs.[Fiscal Year] = '2010' THEN N'09/30/2010 12:00:00 AM -00:00'
	   ELSE N'09/30/2012 12:00:00 AM -00:00'
	   END,
	   CASE 
	   WHEN vs.[Fiscal Year] = '2010' THEN 7373
	   ELSE 7372
	   END,
	   NULL
  FROM FulbrightDataset2_Import vs
  LEFT OUTER JOIN eca_kmt_dev.eca_kmt_dev.dbo.gender g ON (g.sevisgendercode = vs.[Gender ])
  JOIN eca_kmt_dev.eca_kmt_dev.dbo.person p ON (
  (vs.[First Name] IS NULL AND p.firstname IS NULL OR UPPER(p.firstname) = UPPER(vs.[First Name])) AND
  (vs.[Last Name] IS NULL AND p.lastname IS NULL OR UPPER(p.lastname) = UPPER(vs.[Last Name])) AND
  (vs.[Prefix] IS NULL AND p.nameprefix IS NULL  OR UPPER(p.nameprefix) = UPPER(vs.[Prefix])) AND
  (vs.[Suffix] IS NULL AND p.namesuffix IS NULL OR UPPER(p.namesuffix) = UPPER(vs.[Suffix])) AND
  (vs.[Second Last Name] IS NULL AND p.secondlastname IS NULL OR UPPER(p.secondlastname) = UPPER(vs.[Second Last Name])) AND
  (vs.[Middle Name] IS NULL AND p.middlename IS NULL OR UPPER(p.middlename) = UPPER(vs.[Middle Name])) AND
  (vs.[Date of Birth] IS NULL AND p.Dateofbirth IS NULL OR CONVERT(DATE,vs.[Date of birth]) = CONVERT(DATE,p.dateofbirth)) AND 
  (vs.[Gender ] IS NULL AND p.genderid IS NULL OR g.genderid = p.genderid)
  )
  JOIN eca_kmt_dev.eca_kmt_dev.dbo.participanttype pt ON (pt.name = 'Individual')
  --LEFT JOIN eca_kmt_dev.eca_kmt_dev.dbo.organization o ON (o.description = vs.[Home Institution Name])
  WHERE vs.subcategory = 'Specialist'
 ORDER BY vs.[Last Name],vs.[First Name]

GO
