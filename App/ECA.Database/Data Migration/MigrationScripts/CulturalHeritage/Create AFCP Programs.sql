/*SET IDENTITY_INSERT [dbo].[Program] ON*/
DECLARE @CHOrgID int

/* Get the correct organization */
SELECT @CHOrgID = organizationid
FROM organization
WHERE description = 'Cultural Heritage Center'

/* Add the programs */
INSERT INTO [dbo].[Program] 
([ProgramStatusId],
 [Name], 
 [Description], 
 [FocusId], 
 [StartDate], 
 [EndDate], 
 [History_CreatedBy], 
 [History_CreatedOn], 
 [History_RevisedBy], 
 [History_RevisedOn], 
 [ParentProgram_ProgramId], 
 [Owner_OrganizationId]) 
VALUES 
(1,
  N'Ambassadors Fund for Cultural Preservation (AFCP)', 
  N'The U.S. Ambassadors Fund for Cultural Preservation (AFCP) supports the preservation of cultural heritage in developing countries. U.S. ambassadors may apply annually for funds through AFCP to support pressing cultural heritage preservation needs. Projects funded through AFCP advance U.S. foreign policy objectives and show respect for other cultures.', 
  2,
  N'1/1/2015 12:00:00 AM -05:00', 
  N'1/1/2016 12:00:00 AM -05:00', 
  0, 
  N'3/24/2015 12:00:00 AM -05:00',
  0, 
  N'3/24/2015 12:00:00 AM -05:00', 
  NULL, 
  @CHOrgID),
 (1,
  N'Ambassadors Fund for Cultural Preservation (AFCP)  - Small  Grants Competition', 
  N'The AFCP Competition supports projects to conserve ancient and historic buildings and archaeological sites, cultural objects, and forms of traditional cultural expression, such as building crafts and indigenous languages. This program awards approximately 50 grants per year ranging from $10,000 to $200,000 through U.S. embassies serving more than 140 countries. Projects funded through this program advance U.S. foreign policy objectives and demonstrate U.S. respect for other cultures.', 
  2,
  N'1/1/2015 12:00:00 AM -05:00', 
  N'1/1/2016 12:00:00 AM -05:00', 
  0, 
  N'1/14/2015 12:00:00 AM -05:00',
  0, 
  N'1/14/2015 12:00:00 AM -05:00', 
  NULL, 
  @CHOrgID),
(1,
  N'Ambassadors Fund for Cultural Preservation (AFCP) - Large Grants Program', 
  N'The AFCP Large Grants Program supports the preservation of major ancient archaeological sites, historic buildings and monuments, and museum collections that are accessible to the public and protected by law in the host country. This limited competition awards between 3 and 5 grants per year ranging from $500,000 to $1 million. Projects funded through this program advance U.S. foreign policy objectives and demonstrate U.S. respect for other cultures.', 
  2,
  N'1/1/2015 12:00:00 AM -05:00', 
  N'1/1/2016 12:00:00 AM -05:00', 
  0, 
  N'1/14/2015 12:00:00 AM -05:00',
  0, 
  N'1/14/2015 12:00:00 AM -05:00', 
  NULL, 
  @CHOrgID),
(1,
  N'Ambassadors Fund for Cultural Preservation (AFCP) – Other Projects', 
  N'AFCP occasionally receives supplemental funds from Congress and extra-budgetary contributions from other State Department bureaus to support cultural heritage preservation projects outside the annual Small Grants Competition and Large Grants Program cycles. Projects either funded or reviewed and approved through this program advance U.S. foreign policy objectives and demonstrate U.S. respect for other cultures.', 
  2,
  N'1/1/2015 12:00:00 AM -05:00', 
  N'1/1/2016 12:00:00 AM -05:00', 
  0, 
  N'1/14/2015 12:00:00 AM -05:00',
  0, 
  N'1/14/2015 12:00:00 AM -05:00', 
  NULL, 
  @CHOrgID)
GO

/* Assign the MAIN programid to subprograms */
UPDATE dbo.program p
SET p.parentprogram_programid = (SELECT programid FROM dbo.program
                               WHERE name = 'Ambassadors Fund for Cultural Preservation (AFCP)')
WHERE programid IN (1008,1009,1010)
