/* Update office description */

/* Update all the names and descriptions */
UPDATE  b
SET     b.name = a.[OFFICE NAME],
        b.description = a.[NEW TEXT TO BE INCORPORATED INTO KMT]
FROM    [ECA_Data_Migration].[dbo].[DOS_Offices] a
        INNER JOIN eca_local.dbo.organization b
            ON a.[OFFICE CODE] = b.officesymbol
WHERE b.officesymbol IS NOT NULL AND b.organizationtypeid = 1

  /* Make all active */
  update eca_local.dbo.organization
  set status = 'Active' 
  where officesymbol is not null and organizationtypeid = 1

  /* Set those that are marked to delete as Inactive */
  update eca_local.dbo.organization
  set status = 'Inactive' 
  where officesymbol IS NOT NULL and organizationtypeid = 1 and (description = 'DELETE' OR description = 'DELETE/REPLACE') 



/* Add missing */
INSERT INTO [dbo].[Organization] 
([OrganizationTypeId], [Website], [OfficeSymbol], [Description], [Status], [Name], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn], [ParentOrganization_OrganizationId])
VALUES 
(1, N'http://eca.state.gov', N'ECA-IIP/EX/HR',  N'The Human Resources Division is responsible for a comprehensive human resource program, including: recruitment of qualified applicants into a diverse workforce; employment and orientation services; retirement processing; administration of employee benefits; pre-payroll administration and processes for students and employees; policy development and administration; job classification, compensation, employee relations; employee assistance programs; maintenance of personnel records; and the administration of performance evaluation programs and the staff employee grievance process.', 
N'Active', N'Human Resources Division', 0, N'2/23/2016 12:00:00 AM -00:00', 0, N'2/23/2016 12:00:00 AM -00:00', 1)
,(1, N'http://eca.state.gov', N'ECA-IIP/EX/IT', N'The Information Technology Division supports over 1,000 public diplomacy customers by managing projects and providing operational and advisory support on a broad spectrum of information technology activities, including networks, IT infrastructure, applications development, operations and maintenance, data management, change management, and IT systems security.  This work is accomplished through four branches comprised of more than 60 employees, both direct hire and contractors: Application Development; Change Management; Network Operations; and Release Management. The Information Systems Security Officer (ISSO) position resides in EX/IT.', 
N'Active', N'Information Technology Division', 0, N'2/23/2016 12:00:00 AM -00:00', 0, N'2/23/2016 12:00:00 AM -00:00', 1)
, (1, N'http://eca.state.gov', N'ECA-IIP/EX/P', N'The Procurement Division provides customer support by facilitating the efficient acquisition of goods and services.',
N'Active', N'Procurement Division', 0, N'2/23/2016 12:00:00 AM -00:00', 0, N'2/23/2016 12:00:00 AM -00:00', 1)


UPDATE [dbo].[Organization]
SET [ParentOrganization_OrganizationId] = (SELECT organizationid FROM [dbo].[Organization] WHERE officesymbol = 'ECA-IIP/EX' AND organizationtypeid = 1) 
WHERE [OfficeSymbol] IN
('ECA-IIP/EX/G'
,'ECA-IIP/EX/HR'
,'ECA-IIP/EX/IT'
,'ECA-IIP/EX/PM'
,'ECA-IIP/EX/P'
,'ECA-IIP/EX/S'
,'ECA-IIP/EX/PER'
,'ECA-IIP/EX/PR'
,'ECA-IIP/EX/RM'
)
AND organizationtypeid = 1

UPDATE [dbo].[Organization]
SET [ParentOrganization_OrganizationId] = (SELECT organizationid FROM [dbo].[Organization] WHERE officesymbol = 'ECA-IIP/EX/PER' AND organizationtypeid = 1) 
WHERE [OfficeSymbol] IN
('ECA-IIP/EX/PER/OD'
,'ECA-IIP/EX/PER/OPS'
)
AND organizationtypeid = 1

UPDATE [dbo].[Organization]
SET [ParentOrganization_OrganizationId] = (SELECT organizationid FROM [dbo].[Organization] WHERE officesymbol = 'ECA-IIP/EX/RM' AND organizationtypeid = 1) 
WHERE [OfficeSymbol] IN
('ECA-IIP/EX/RM/ECE'
,'ECA-IIP/EX/RM/PD'
)
AND organizationtypeid = 1


UPDATE [dbo].[Organization]
SET description = name
WHERE officesymbol = 'ECA/NOC'



























                                                                                                                                                                                                                                                                                                                                                                           