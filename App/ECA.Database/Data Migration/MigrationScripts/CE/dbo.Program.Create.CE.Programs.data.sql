/* Create CE Programs for Pilot data - these don't exist in Pilot data dump.  */
/* Related info will be added at a later time.                                */
USE CE
GO

INSERT INTO [dbo].[Program] 
	([ProgramStatusId],[Name],[Description],[FocusId],[Website],[StartDate],[EndDate], 
	[History_CreatedBy],[History_CreatedOn],[History_RevisedBy],[History_RevisedOn], 
	[ParentProgram_ProgramId],[Owner_OrganizationId]) 
SELECT ps.programstatusid,cp.program,'Description for '+cp.program,f.focusid,NULL,MIN(start_date),MAX(end_date),
       0, N'4/10/2015 12:00:00 AM -05:00', 0, N'4/10/2015 12:00:00 AM -05:00',
       /*cp.parent_program,*/p.programid,/*p.name,*/
	CASE 
	WHEN cp.program = 'Sports, Special Programs' THEN 1404
	WHEN cp.program = 'CBYX' THEN 1405
	WHEN cp.program = 'Professional Open Competition' THEN 65
	WHEN cp.program = 'TPPP' THEN 65
	ELSE p.owner_organizationid
	END
  FROM ce_project cp
  JOIN eca_dev.eca_dev.dbo.programstatus ps ON (ps.status = 'Active')
  JOIN eca_dev.eca_dev.dbo.focus f ON (f.FocusName = 'None Selected')
  LEFT JOIN eca_dev.eca_dev.dbo.program p ON (p.Name = cp.parent_program)
  GROUP BY ps.programstatusid,cp.program,f.focusid,/*cp.parent_program,*/p.programid,/*p.name,*/p.owner_organizationid


GO


/* Assign the missing Owner Organizations */
/* Like 88 */
UPDATE eca_dev.eca_dev.dbo.program SET parentprogram_programid = NULL, owner_organizationid = 45 ,
description = 'At the request of U.S. missions, the Arts Envoy Program sends professional American artists, performing groups, arts specialists, educators, managers, and practitioners abroad.  This “on demand” program supports projects by arts professionals who can spend five days to six weeks in a country or region working with priority groups and arts professionals to accomplish post’s mission objectives and address the Department’s foreign policy priorities.  An Arts Envoy’s primary purpose is to engage local communities, fellow professionals, and strategic audiences to share the vibrancy of the American arts, impart expertise, and achieve clearly defined public diplomacy programs.  The Cultural Programs Division directly programs Arts Envoys, in collaboration with U.S. missions abroad.'
WHERE programid = 1048

/* Like 82 */
UPDATE eca_dev.eca_dev.dbo.program SET parentprogram_programid = NULL, owner_organizationid = 1405,
description = 'Jointly funded by the U.S. Congress and the German Bundestag, and administered in the U.S. by ECA since 1983, the program celebrates German-American friendship based on common values of democracy and conveys lasting personal and institutional relationships through an academic year school and home-stay experience.  German and American secondary school students live with host families, attend school, and participate in community life.  Another part of the exchange is dedicated to young (undergraduate) professionals and vocational school graduates to study and receive practical training.' 
WHERE programid = 1049

/* like 83 */
UPDATE eca_dev.eca_dev.dbo.program SET parentprogram_programid = NULL, owner_organizationid = 1405,
description = 'The FLEX program promotes mutual understanding by providing secondary school students from Eurasia with the opportunity to live with host families and attend high school in cities across the United States for approximately eleven months.  The program supports academic year students from Armenia, Azerbaijan, Georgia, Kazakhstan, Kyrgyzstan, Moldova, Russia, Tajikistan, Turkmenistan and Ukraine.  The program also includes an integrated component for students with disabilities as well as a D.C.-based Civic Education Workshop for FLEX students selected through an open merit-based essay contest.  While in the United States, FLEX students expose U.S. citizens to the culture, traditions and lifestyles of their home countries; back home, these students introduce American ideals and values to their friends, neighbors and classmates. The program also provides funding for the robust and dynamic network of FLEX alumni.'
WHERE programid = 1050

UPDATE eca_dev.eca_dev.dbo.program SET parentprogram_programid = NULL, owner_organizationid = 45 WHERE programid = 1051
UPDATE eca_dev.eca_dev.dbo.program SET parentprogram_programid = NULL, owner_organizationid = 1402 WHERE programid = 1052
UPDATE eca_dev.eca_dev.dbo.program SET parentprogram_programid = NULL, owner_organizationid = 1404 WHERE programid = 1053
UPDATE eca_dev.eca_dev.dbo.program SET parentprogram_programid = NULL, owner_organizationid = 1402 WHERE programid = 1054
UPDATE eca_dev.eca_dev.dbo.program SET parentprogram_programid = NULL, owner_organizationid = 1402 WHERE programid = 1055

UPDATE eca_dev.eca_dev.dbo.program SET parentprogram_programid = 3, owner_organizationid = 1407,
description = 'Exchanges between 25 countries in the Western Hemisphere and the United States.'
WHERE programid = 1056

GO


/* Add other program information where possible */







