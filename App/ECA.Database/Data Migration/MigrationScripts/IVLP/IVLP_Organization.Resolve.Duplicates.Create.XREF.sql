/* resolve IVLP Duplicate organizations */
UPDATE ivlp_organization_duplicate
SET resolved_org_id = '04E498D0D3DC43A58A44C906DE0DC33D',
    resolved_org_name = 'A & J Tours, Inc.'
WHERE org_id IN ('04E498D0D3DC43A58A44C906DE0DC33D',
'5F7CAF6EE2CD44089C700244C1A3707B',
'A6E8BEAAD37E459D997A0A30C092BC08',
'0C647CE25B6346B488C682DAC0449091',
'6E61C11D96C14EB691276FBA703A789A',
'B5945C70E28E46C9A94F74C6914BB27B')

GO

UPDATE ivlp_organization_duplicate
SET resolved_org_id = N'8527E158DE8B47298626BABDF273929F',
    resolved_org_name = N'Awards Limousine Service, Inc.',
    web_address = N'www.awardslimo.com'
WHERE org_id IN ('8527E158DE8B47298626BABDF273929F'
,'78AD7BB5D7114A539B35358AE1CAC764'
,'3F1FA1E67D9F463AA767650DECFB692C'
,'C3AC759D59204EABA8B21469B3D9DBB5'
,'66CAB2AFE89546AB93DA2707912D383B'
,'9B3D18AC583D4A828CB39A20BECFF85B')

GO


UPDATE ivlp_organization_duplicate
SET resolved_org_id = N'BF2FEE4291E44901B9F757D419669CA7',
    resolved_org_name = N'Carey Executive Limousine'
WHERE org_id IN ('BF2FEE4291E44901B9F757D419669CA7'
,'7DB0BE27FE75480E92C61A5EAA0A45BC'
,'3459DC20005349028457FC7CDB4B2183')

GO

/*F0D9ACF277F811D6B8A200B0D07F2F65	ECA/PE - Professional Exchanges  */
UPDATE ivlp_organization_duplicate
SET resolved_org_id = org_id,
    resolved_org_name = N'Directorate of Professional & Cultural Exchanges',
    resolved_office_symbol = N'ECA/PE',
    web_address = 'http://eca.state.gov'
WHERE org_id = N'F0D9ACF277F811D6B8A200B0D07F2F65'

GO

/*F0D9A4E477F811D6B8A200B0D07F2F65	ECA/PE/V - Office of International Visitors */
UPDATE ivlp_organization_duplicate
SET resolved_org_id = org_id,
    resolved_org_name = N'Office of International Visitors',
    resolved_office_symbol = N'ECA/PE/V',
    web_address = 'http://eca.state.gov'
WHERE org_id = N'F0D9A4E477F811D6B8A200B0D07F2F65'

GO

/*F0D9A4F977F811D6B8A200B0D07F2F65	ECA/PE/V/C - Community Resources Division*/
UPDATE ivlp_organization_duplicate
SET resolved_org_id = org_id,
    resolved_org_name = N'Community Resources Division',
    resolved_office_symbol = N'ECA/PE/V/C',
    web_address = 'http://eca.state.gov'
WHERE org_id = N'F0D9A4F977F811D6B8A200B0D07F2F65'

GO


/*F0D9AC9577F811D6B8A200B0D07F2F65	ECA/PE/V/C/N - New York Program Branch*/
UPDATE ivlp_organization_duplicate
SET resolved_org_id = org_id,
    resolved_org_name = N'New York Program Branch',
    resolved_office_symbol = N'ECA/PE/V/C/N',
    web_address = 'http://eca.state.gov'
WHERE org_id = N'F0D9AC9577F811D6B8A200B0D07F2F65'

GO

/*F0D9A4E677F811D6B8A200B0D07F2F65	ECA/PE/V/R - IVLP Division*/
UPDATE ivlp_organization_duplicate
SET resolved_org_id = org_id,
    resolved_org_name = N'Regional Programs Division',
    resolved_office_symbol = N'ECA/PE/V/R',
    web_address = 'http://eca.state.gov'
WHERE org_id = N'F0D9A4E677F811D6B8A200B0D07F2F65'

GO

/*F0D9A4EA77F811D6B8A200B0D07F2F65	ECA/PE/V/R/A - Africa Branch*/
UPDATE ivlp_organization_duplicate
SET resolved_org_id = org_id,
    resolved_org_name = N'AF Branch',
    resolved_office_symbol = N'ECA/PE/V/R/A',
    web_address = 'http://eca.state.gov'
WHERE org_id = N'F0D9A4EA77F811D6B8A200B0D07F2F65'

GO

/*F0D9A4EE77F811D6B8A200B0D07F2F65	ECA/PE/V/R/E - Europe Branch*/
UPDATE ivlp_organization_duplicate
SET resolved_org_id = org_id,
    resolved_org_name = N'EUR Branch',
    resolved_office_symbol = N'ECA/PE/V/R/E',
    web_address = 'http://eca.state.gov'
WHERE org_id = N'F0D9A4EE77F811D6B8A200B0D07F2F65'

GO

/*F0D9A4ED77F811D6B8A200B0D07F2F65	ECA/PE/V/R/F - East Asia Branch*/
UPDATE ivlp_organization_duplicate
SET resolved_org_id = org_id,
    resolved_org_name = N'EAP Branch',
    resolved_office_symbol = N'ECA/PE/V/R/F',
    web_address = 'http://eca.state.gov'
WHERE org_id = N'F0D9A4ED77F811D6B8A200B0D07F2F65'

GO

/*F0D9A4EC77F811D6B8A200B0D07F2F65	ECA/PE/V/R/N - Near East and North Africa Branch*/
UPDATE ivlp_organization_duplicate
SET resolved_org_id = org_id,
    resolved_org_name = N'NEA Branch',
    resolved_office_symbol = N'ECA/PE/V/R/N',
    web_address = 'http://eca.state.gov'
WHERE org_id = N'F0D9A4EC77F811D6B8A200B0D07F2F65'

GO

/*1F3FE98995A84C1A80D389965D25D760	ECA/PE/V/R/S - South and Central Asia Branch*/
UPDATE ivlp_organization_duplicate
SET resolved_org_id = org_id,
    resolved_org_name = N'SCA Branch',
    resolved_office_symbol = N'ECA/PE/V/R/S',
    web_address = 'http://eca.state.gov'
WHERE org_id = N'1F3FE98995A84C1A80D389965D25D760'

GO


/*F0D9A4EB77F811D6B8A200B0D07F2F65	ECA/PE/V/R/W - Western Hemisphere Branch*/
/*F0D9A4F777F811D6B8A200B0D07F2F65	ECA/PE/V/R/W - Western Hemisphere Branch- bad record*/
UPDATE ivlp_organization_duplicate
SET resolved_org_id = N'F0D9A4EB77F811D6B8A200B0D07F2F65',
    resolved_org_name = N'WHA Branch',
    resolved_office_symbol = N'ECA/PE/V/R/W',
    web_address = 'http://eca.state.gov'
WHERE org_id IN ( N'F0D9A4EB77F811D6B8A200B0D07F2F65',N'F0D9A4F777F811D6B8A200B0D07F2F65')

GO

/* Park Central Hotel */
UPDATE ivlp_organization_duplicate
SET resolved_org_id = N'F0D9A4EB77F811D6B8A200B0D07F2F65',
    resolved_org_name = N'Park Central Hotel',
    resolved_office_symbol = NULL,
    web_address = N'www.parkcentralny.com'
WHERE org_id IN ( N'25F03DA816184FE199675D82E06A6A80'
,N'F0C98E30D1C74DB3B6D024BEDF3933E6'
,N'4E5A018A2C274FC384DEBBE31356CA1E'
,N'372588770F2641758554187DB10363E7'
,N'EA7273FD6F164CDAB51756FB19F1BF90'
,N'F52E8C56BF944CD9A6195644446152DD')

GO

/*Prime Transportation Services*/
UPDATE ivlp_organization_duplicate
SET resolved_org_id = N'0163B50CF08C46BD9DC3831742235AC3',
    resolved_org_name = N'Prime Transportation Services',
    resolved_office_symbol = NULL
WHERE org_id IN ( N'0163B50CF08C46BD9DC3831742235AC3'
,N'67A0472BFE1441CEB7DA5C3F9B5F04E4')

GO

/*The Mayflower Renaissance Hotel*/
UPDATE ivlp_organization_duplicate
SET resolved_org_id = N'407734CCB9AE4E79970E1CBB14FC530C',
    resolved_org_name = N'The Mayflower Renaissance Hotel',
    resolved_office_symbol = NULL
WHERE org_id IN ( N'407734CCB9AE4E79970E1CBB14FC530C'
,N'2636DCD0ED0D449CAECAAF34A13714E5')

GO


/*U.S. Department of State*/
UPDATE ivlp_organization_duplicate
SET resolved_org_id = N'35EBBBADEBAE4BAA903D4E0CF90438AD',
    resolved_org_name = N'U.S. Department of State',
    resolved_office_symbol = NULL,
    web_address = N'http://www.state.gov'
WHERE org_id IN ( N'35EBBBADEBAE4BAA903D4E0CF90438AD'
,N'0006FCE6C82A4C448F48D0C1DA3E284D'
,N'B334561426A84B289E548F2DDDCC78DA')

GO


/* Now create the Organization rows */
INSERT INTO eca_dev.eca_dev.dbo.organization
  (OrganizationTypeId,OfficeSymbol,Description,Status,Name,Website,History_CreatedBy,
   History_CreatedOn,History_RevisedBy,History_RevisedOn)
SELECT 7,resolved_office_symbol,resolved_org_name,N'Active',resolved_org_name,web_address ,
       0, CAST(N'2015-03-30T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-30T00:00:00.0000000-05:00' AS DateTimeOffset)
  FROM ivlp_organization_duplicate 
 WHERE resolved_org_name NOT IN (SELECT DISTINCT name 
                                   FROM eca_dev.eca_dev.dbo.organization)
 GROUP BY resolved_org_name,resolved_office_symbol,web_address
GO






