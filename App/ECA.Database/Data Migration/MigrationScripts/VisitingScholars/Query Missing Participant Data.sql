SELECT p.personid,*,
p1.*,
p2.* 
from dbo.FulbrightDataset2_Import f
LEFT OUTER JOIN eca_kmt_dev.eca_kmt_dev.dbo.gender g ON (UPPER(g.SevisGenderCode) = UPPER(f.[Gender ]))
LEFT OUTER JOIN eca_kmt_dev.eca_kmt_dev.dbo.Person p ON
((p.[NamePrefix] IS NULL AND f.prefix IS NULL OR UPPER(p.[NamePrefix]) = UPPER(f.prefix)) 
and (p.[LastName] IS NULL AND f.[last name] IS NULL OR UPPER(p.[LastName]) = UPPER(f.[last name])) 
and (p.[FirstName] IS NULL AND f.[first name] IS NULL OR UPPER(p.[FirstName]) = UPPER(f.[first name])) 
and (p.[SecondLastName] IS NULL AND f.[Second Last Name] IS NULL OR UPPER(p.[SecondLastName]) = UPPER(f.[Second Last Name])) 
and (p.[MiddleName] IS NULL AND f.[middle name] IS NULL OR UPPER(p.[MiddleName]) = UPPER(f.[middle name])) 
and (p.[NameSuffix] IS NULL AND f.Suffix IS NULL OR UPPER(p.[NameSuffix]) = UPPER(f.Suffix)) 
and (p.[DateOfBirth] IS NULL AND f.[Date of Birth] IS NULL OR CONVERT(DATE,p.[DateOfBirth]) = CONVERT(DATE,f.[Date of Birth]))
and (p.GenderId IS NULL AND g.GenderId IS NULL OR p.GenderId = g.GenderId))
LEFT OUTER JOIN eca_kmt_dev.eca_kmt_dev.dbo.participant p1 ON (p1.personid = p.personid)
LEFT OUTER JOIN eca_kmt_dev.eca_kmt_dev.dbo.participantperson p2 ON (p2.participantid = p1.participantid)
--where p.personid is null
where p2.participantid is null
order by f.category,f.subcategory,f.[Fiscal Year], p.FullName,  p.personid