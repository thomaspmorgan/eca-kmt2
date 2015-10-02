/* This script will update existing programs for a selected office or insert the ones that don't exist */

/* These offices are included:  */
/* Academic Exchange Programs - ECA/A/E */
/* Office Of Citizen Exchanges - Office of Private Sector Exchange ECA/EC */
/* ECA/A/S - Office of Global Educational Programs */
/* Office Of Citizen Exchanges - Office of English Language Programs ECA/A/L */
/* Office Of Citizen Exchanges - Professional Fellows Division ECA/PE/C/PF */
/* Office Of Citizen Exchanges - Youth Programs Division ECA/PE/C/PY */
/* Office Of Citizen Exchanges - Cultural Programs Division ECA/PE/C/CU */
/* Office Of Citizen Exchanges - Sports United Division ECA/PE/C/SU */



/* Get the OrganizationId for the office */
--DECLARE @OfficeId int
--SELECT @OfficeId = organizationid FROM organization WHERE officesymbol = 'ECA/A/E'

/* Get the 'Active' program status ID */
DECLARE @ActiveStatusId int
SELECT @ActiveStatusId = programstatusid 
FROM dbo.programstatus 
WHERE status = 'Active'

/* Get the 'Completed' program status ID */
DECLARE @CompletedStatusId int
SELECT @CompletedStatusId = programstatusid 
FROM dbo.programstatus 
WHERE status = 'Completed'

/* Get the 'Other' program status ID */
DECLARE @OtherStatusId int
SELECT @OtherStatusId = programstatusid 
FROM dbo.programstatus 
WHERE status = 'Other'

/* Create a temp table and store the program list */
DECLARE @Programs TABLE(RowID int not null identity(1,1) primary key,
                        ProgramName nvarchar(255),
                        ParentProgramName  nvarchar(255),
                        Owner_OfficeSymbol  nvarchar(128))

INSERT INTO @Programs (ProgramName,ParentProgramName,Owner_OfficeSymbol) 
VALUES 
-- 'ECA/A/E'
--Program
('Fulbright Program',NULL,'ECA/A/E')

--Program/Subprogram
,('Fulbright Scholar Program','Fulbright Program','ECA/A/E')
,('Fulbright Student Program','Fulbright Program','ECA/A/E')
,('Study of the U.S. Institutes',NULL,'ECA/A/E')
,('Young African Leaders Initiative',NULL,'ECA/A/E')
,('Young Southeast Asian Leaders Initiative',NULL,'ECA/A/E') 

--Program/Subprogram
,('Fulbright Scholar Program from Partner Countries','Fulbright Scholar Program','ECA/A/E')
,('Fulbright U.S. Scholar Program','Fulbright Scholar Program','ECA/A/E')
,('Foreign Student Program from Partner Countries','Fulbright Student Program','ECA/A/E')
,('Fulbright U.S. Student Program','Fulbright Student Program','ECA/A/E')
,('Study of the U.S. Institute for Scholars','Study of the U.S. Institutes','ECA/A/E')
,('Study of the U.S. Institute for Student Leaders','Study of the U.S. Institutes','ECA/A/E')
,('Mandela Washington Fellowship','Young African Leaders Initiative','ECA/A/E')
,('YSEALI Academic Fellowships','Young Southeast Asian Leaders Initiative','ECA/A/E')
,('Afghan Junior Faculty Development',NULL,'ECA/A/E')
,('Fulbright Economics Teaching Program',NULL,'ECA/A/E')
,('International Center for Middle Eastern-Western Dialogue (Hollings Center)',NULL,'ECA/A/E')
,('Tibetan Scholarship Program',NULL,'ECA/A/E')
,('Undergraduate Exchange Program (UGRAD)',NULL,'ECA/A/E')
,('US-South Pacific Scholarship Program',NULL,'ECA/A/E')
,('US-Timor Leste Scholarship Program',NULL,'ECA/A/E')
,('Council of American Overseas Research Centers (CAORC)',NULL,'ECA/A/E')

--Subprogram
,('Fulbright ASEAN Visiting Scholar','Fulbright Scholar Program from Partner Countries','ECA/A/E')
,('Fulbright International Education Administrators Program','Fulbright Scholar Program from Partner Countries','ECA/A/E')
,('Fulbright Lecturer','Fulbright Scholar Program from Partner Countries','ECA/A/E')
,('Fulbright Lecturer/Researcher','Fulbright Scholar Program from Partner Countries','ECA/A/E')
,('Fulbright Scholar-in-Residence','Fulbright Scholar Program from Partner Countries','ECA/A/E')
,('Fulbright Scholar Researcher','Fulbright Scholar Program from Partner Countries','ECA/A/E')
,('Korean Secondary School Educator Program','Fulbright Scholar Program from Partner Countries','ECA/A/E')
,('NEA Fulbright Junior Faculty Development Program','Fulbright Scholar Program from Partner Countries','ECA/A/E')
,('Fulbright Arctic Initiative','Fulbright Scholar Program','ECA/A/E')
,('Fulbright NEXUS','Fulbright Scholar Program','ECA/A/E')
,('Fulbright-Fogarty Scholar','Fulbright U.S. Scholar Program','ECA/A/E')
,('Fulbright International Education Administrators Program','Fulbright U.S. Scholar Program','ECA/A/E')
,('Fulbright Lecturer','Fulbright U.S. Scholar Program','ECA/A/E')
,('Fulbright Lecturer/Researcher','Fulbright U.S. Scholar Program','ECA/A/E')
,('Fulbright Scholar Researcher','Fulbright U.S. Scholar Program','ECA/A/E')
,('Fulbright Specialist','Fulbright U.S. Scholar Program','ECA/A/E')
,('German Studies Seminar','Fulbright U.S. Scholar Program','ECA/A/E')
,('Fulbright Faculty Development Program Moldova','Foreign Student Program from Partner Countries','ECA/A/E')
,('Fulbright Faculty Development Program Ukraine','Foreign Student Program from Partner Countries','ECA/A/E')
,('Fulbright Foreign Language Teaching Assistant','Foreign Student Program from Partner Countries','ECA/A/E')
,('Fulbright Science and Technology Award','Foreign Student Program from Partner Countries','ECA/A/E')
,('Israeli Outreach Program','Foreign Student Program from Partner Countries','ECA/A/E')
,('Presidential Scholar','Foreign Student Program from Partner Countries','ECA/A/E')
,('Fulbright Young American Journalists Program','Fulbright U.S. Student Program','ECA/A/E')
,('Fulbright English Teaching Assistant','Fulbright U.S. Student Program','ECA/A/E')
,('Fulbright-Fogarty Student','Fulbright U.S. Student Program','ECA/A/E')
,('Fulbright mtvU Fellowship Program','Fulbright U.S. Student Program','ECA/A/E')
,('Fulbright-National Geographic Digital Storytelling Fellowship','Fulbright U.S. Student Program','ECA/A/E')
,('Fulbright Student Researcher','Fulbright U.S. Student Program','ECA/A/E')
,('J William Fulbright - Hillary Rodham Clinton Fellowship','Fulbright U.S. Student Program','ECA/A/E')
,('Contemporary American Literature','Study of the U.S. Institute for Scholars','ECA/A/E')
,('Secondary Educators (Administrators)','Study of the U.S. Institute for Scholars','ECA/A/E')
,('Secondary Educators (Teachers)','Study of the U.S. Institute for Scholars','ECA/A/E')
,('Journalism and New Media','Study of the U.S. Institute for Scholars','ECA/A/E')
,('Religious Pluralism in the United States','Study of the U.S. Institute for Scholars','ECA/A/E')
,('U.S. Culture and Society','Study of the U.S. Institute for Scholars','ECA/A/E')
,('U.S. Foreign Policy','Study of the U.S. Institute for Scholars','ECA/A/E')
,('U.S. Political Thought','Study of the U.S. Institute for Scholars','ECA/A/E')
,('Civic Engagement','Study of the U.S. Institute for Student Leaders','ECA/A/E')
,('Global Environmental Issues','Study of the U.S. Institute for Student Leaders','ECA/A/E')
,('Journalism and New Media','Study of the U.S. Institute for Student Leaders','ECA/A/E')
,('Local, State, and Federal Public Policymaking for Pakistani Student Leaders','Study of the U.S. Institute for Student Leaders','ECA/A/E')
,('Religious Pluralism in the United States','Study of the U.S. Institute for Student Leaders','ECA/A/E')
,('Social Entrepreneurship','Study of the U.S. Institute for Student Leaders','ECA/A/E')
,('U.S. History and Government','Study of the U.S. Institute for Student Leaders','ECA/A/E')
,('Women''s Leadership','Study of the U.S. Institute for Student Leaders','ECA/A/E')
,('Environmental Stewardship','Study of the U.S. Institute for Student Leaders','ECA/A/E')
,('Business and Entrepreneurship','Mandela Washington Fellowship','ECA/A/E')
,('Civic Leadership','Mandela Washington Fellowship','ECA/A/E')
,('Public Management','Mandela Washington Fellowship','ECA/A/E')
,('Environmental Issues','YSEALI Academic Fellowships','ECA/A/E')
,('Civic Engagement','YSEALI Academic Fellowships','ECA/A/E')
,('Social Entrepreneurship and Economic Development','YSEALI Academic Fellowships','ECA/A/E')
,('Master of Public Policy (MPP)','Fulbright Economics Teaching Program','ECA/A/E')
,('Vietnam Executive Leadership Training (VELT)','Fulbright Economics Teaching Program','ECA/A/E')
,('Challenging Extremist Ideology, Propaganda and Messaging: Building the Counter-narrative','International Center for Middle Eastern-Western Dialogue (Hollings Center)','ECA/A/E')
,('Protecting Cultural Heritage in Conflict and Preparing for Post-Conflict','International Center for Middle Eastern-Western Dialogue (Hollings Center)','ECA/A/E')
,('Corporate Social Responsibility in Islam','International Center for Middle Eastern-Western Dialogue (Hollings Center)','ECA/A/E')
,('High and Dry: Addressing the Middle East Water Challenge','International Center for Middle Eastern-Western Dialogue (Hollings Center)','ECA/A/E')
,('UGRAD Serbia and Montenegro','Undergraduate Exchange Program (UGRAD)','ECA/A/E')
,('UGRAD Tunisia','Undergraduate Exchange Program (UGRAD)','ECA/A/E')
,('UGRAD Eurasia and Central Asia','Undergraduate Exchange Program (UGRAD)','ECA/A/E')
,('UGRAD Pakistan','Undergraduate Exchange Program (UGRAD)','ECA/A/E')
,('UGRAD Near East and Sub-Saharan Africa','Undergraduate Exchange Program (UGRAD)','ECA/A/E')
,('UGRAD Western Hemisphere','Undergraduate Exchange Program (UGRAD)','ECA/A/E')
,('UGRAD East Asia and Pacific' ,'Undergraduate Exchange Program (UGRAD)','ECA/A/E')
,('Global Undergraduate Exchange Program (UGRAD)','Undergraduate Exchange Program (UGRAD)','ECA/A/E')

-- 'ECA/EC'
--Program
,('Summer Work Travel',NULL,'ECA/EC')
,('Intern',NULL,'ECA/EC')
,('Trainee',NULL,'ECA/EC')
,('Specialist',NULL,'ECA/EC')
,('Camp Counselor',NULL,'ECA/EC')
,('Au Pair',NULL,'ECA/EC')
,('Short Term Scholar',NULL,'ECA/EC')
,('Research Scholar',NULL,'ECA/EC')
,('College and University Student',NULL,'ECA/EC')
,('High School Student',NULL,'ECA/EC')
,('Physician',NULL,'ECA/EC')
,('Professor',NULL,'ECA/EC')

--Program/Subprogram
,('General','Summer Work Travel','ECA/EC')
,('Australia/New Zealand Pilot','Summer Work Travel','ECA/EC')
,('General','Intern','ECA/EC')
,('Intern Work Travel (Ireland)','Intern','ECA/EC')
,('Intern Work Travel (Korea West)','Intern','ECA/EC')
,('Intern Work Travel (Mexico)','Intern','ECA/EC')
,('EduCare','Au Pair','ECA/EC')
,('General','Au Pair','ECA/EC')

-- ECA/A/S
--Program
,('Fulbright Program',NULL,'ECA/A/S')

--Program/Subprogram
,('Critical Language Scholarship (CLS) Program',NULL,'ECA/A/S')
,('Gilman Program',NULL,'ECA/A/S')
,('U.S. Study Abroad',NULL,'ECA/A/S')
,('Hubert H. Humphrey Fellowship Program','Fulbright Program','ECA/A/S')
,('Community College Initiative Program',NULL,'ECA/A/S')
,('Community College Administrator Program',NULL,'ECA/A/S')
,('Thomas Jefferson Scholarship Program',NULL,'ECA/A/S')
,('Fubright Distinguished Awards in Teaching','Fulbright Program','ECA/A/S' )
,('Teaching Excellence and Achievement Program',NULL,'ECA/A/S')
,('International Leaders in Education Program',NULL,'ECA/A/S')
,('Teachers for Global Classrooms Program',NULL,'ECA/A/S')
,('Teachers of Critical Languages Program',NULL,'ECA/A/S')
,('EducationUSA',NULL,'ECA/A/S')

--Subprogram
,('Capacity-Building Program for U.S. Undergraduate Study Abroad','U.S. Study Abroad','ECA/A/S')
,('Tunisia Community College Scholarship Program','Thomas Jefferson Scholarship Program','ECA/A/S')
,('Opportunity Funds','EducationUSA','ECA/A/S')
,('Competitive College Club','EducationUSA','ECA/A/S')
,('United States Achievers Program','EducationUSA','ECA/A/S')
,('Cohort-Advising Program (Other)','EducationUSA','ECA/A/S')
,('Leadership Institutes','EducationUSA','ECA/A/S')
,('EducationUSA Academy','EducationUSA','ECA/A/S')
,('Open Doors Survey','EducationUSA','ECA/A/S')
,('Global EducationUSA Services','EducationUSA','ECA/A/S')
,('Advising Operations in Post-Soviet Countries','EducationUSA','ECA/A/S')
,('Advising Operations in the Middle East and North Africa','EducationUSA','ECA/A/S')
,('EducationUSA Interactive','EducationUSA','ECA/A/S')
,('EducationUSA Fairs','EducationUSA','ECA/A/S')
,('EducationUSA Recycling Program','EducationUSA','ECA/A/S')

-- ECA/A/L
--Program
,('Access',NULL,'ECA/A/L')
,('English Language Fellows/Specialists',NULL,'ECA/A/L')
,('E-Teacher',NULL,'ECA/A/L')
,('English Teaching Forum',NULL,'ECA/A/L')
,('American English Website',NULL,'ECA/A/L')
,('American English Social Media',NULL,'ECA/A/L')
,('Print Materials',NULL,'ECA/A/L')

--Program/Subprogram
,('Learner Scholarships','Access','ECA/A/L')
,('Teacher Training','Access','ECA/A/L')
,('U.S.-Based Exchanges','Access','ECA/A/L')
,('Content for Learners','Access','ECA/A/L')
,('Alumni Programming','Access','ECA/A/L')
,('EL Fellows','English Language Fellows/Specialists','ECA/A/L')
,('EL Specialists','English Language Fellows/Specialists','ECA/A/L')
,('Webinars','English Language Fellows/Specialists','ECA/A/L')
,('Virtual Specialists','English Language Fellows/Specialists','ECA/A/L')
,('Scholarships','E-Teacher','ECA/A/L')
,('Open Educational Resources','E-Teacher','ECA/A/L')
,('Print','English Teaching Forum','ECA/A/L')
,('Digital','English Teaching Forum','ECA/A/L')
,('Facebook','American English Social Media','ECA/A/L')

-- ECA/PE/C/PF
--Program
,('Professional Fellows',NULL,'ECA/PE/C/PF')
,('Professional Fellows - YSEALI',NULL,'ECA/PE/C/PF')
,('Professional Fellows On Demand',NULL,'ECA/PE/C/PF')
,('Community Solutions',NULL,'ECA/PE/C/PF')
,('Professional Fellows Congress',NULL,'ECA/PE/C/PF')
,('TechWomen',NULL,'ECA/PE/C/PF')
,('Pakistan Journalism Program',NULL,'ECA/PE/C/PF')
,('Fortune/U.S. Department of State Global Women''s Mentoring Partnership',NULL,'ECA/PE/C/PF')
,('Goldman Sachs 10,000 Women - U.S. Department of State Entrepreneurship Program',NULL,'ECA/PE/C/PF')
,('Business Leadership Program for Young Russians',NULL,'ECA/PE/C/PF')
,('Digital Communications Network (=Russian Periphery Digital Communicators Network',NULL,'ECA/PE/C/PF')
,('Japan-US Friendship Commission (CULCON)',NULL,'ECA/PE/C/PF')
,('Mike Mansfield Fellowship Program',NULL,'ECA/PE/C/PF')
,('National Youth Science Camp',NULL,'ECA/PE/C/PF')
,('U.S. Congress-Korea National Assembly Youth Exchange',NULL,'ECA/PE/C/PF')
,('Traditional Public-Private Partnerships: American Council of Young Political Leaders (ACYPL)',NULL,'ECA/PE/C/PF')
,('Traditional Public-Private Partnerships: ACILS',NULL,'ECA/PE/C/PF')
,('Traditional Public-Private Partnerships: Partners of the Americas',NULL,'ECA/PE/C/PF')
,('Traditional Public-Private Partnerships: Sister Cities International',NULL,'ECA/PE/C/PF')
,('Traditional Public-Private Partnerships: Institute for Representative Government',NULL,'ECA/PE/C/PF')

--Program/Subprogram
,('Professional Fellows Program FY13','Professional Fellows','ECA/PE/C/PF')
,('Professional Fellows - YSEALI FY15','Professional Fellows - YSEALI','ECA/PE/C/PF')
,('Professional Fellows  On-Demand FY12','Professional Fellows On Demand','ECA/PE/C/PF') 
,('Community Solutions FY13','Community Solutions','ECA/PE/C/PF')
,('Professional Fellows Congress FY13','Professional Fellows Congress','ECA/PE/C/PF')
,('TechWomen FY13','TechWomen','ECA/PE/C/PF')
,('Pakistan Journalism Program FY13','Pakistan Journalism Program','ECA/PE/C/PF')
,('Japan-US Friendship Commission','Japan-US Friendship Commission (CULCON)','ECA/PE/C/PF')
,('Mike Mansfied Fellowship Program FY13','Mike Mansfield Fellowship Program','ECA/PE/C/PF')
,('National Youth Science Camp FY13','National Youth Science Camp','ECA/PE/C/PF')
,('U.S. Congress-Korea National Assembly Youth Exchange FY13','U.S. Congress-Korea National Assembly Youth Exchange','ECA/PE/C/PF')
,('TPPP: The American Council of Young Political Leaders FY13','Traditional Public-Private Partnerships: American Council of Young Political Leaders (ACYPL)','ECA/PE/C/PF')
,('TPPP: ACILS FY13','Traditional Public-Private Partnerships: ACILS','ECA/PE/C/PF')
,('TPPP: Partners of the Americas FY13','Traditional Public-Private Partnerships: Partners of the Americas','ECA/PE/C/PF')
,('TPPP: Sister Cities International','Traditional Public-Private Partnerships: Sister Cities International','ECA/PE/C/PF')
,('TPPP: Institute for Representative Government (IRG) FY13','Traditional Public-Private Partnerships: Institute for Representative Government','ECA/PE/C/PF')
 
-- ECA/PE/C/PY
--Program
,('FLEX',NULL,'ECA/PE/C/PY')
,('A-SMYLE',NULL,'ECA/PE/C/PY')
,('YES',NULL,'ECA/PE/C/PY')
,('YES Abroad',NULL,'ECA/PE/C/PY')
,('CBYX',NULL,'ECA/PE/C/PY')
,('English Language Workshops for Alumni',NULL,'ECA/PE/C/PY')
,('Tech Girls',NULL,'ECA/PE/C/PY')
,('National Security Language Initiative for Youth (NSLI-Y)',NULL,'ECA/PE/C/PY')
,('Short Term Youth Leadership Programs',NULL,'ECA/PE/C/PY')
,('Youth Leadership On Demand',NULL,'ECA/PE/C/PY')
,('American Youth Leadership Program',NULL,'ECA/PE/C/PY')
,('Youth Ambassadors Program',NULL,'ECA/PE/C/PY')
,('German-American Partnership Program (GAPP)',NULL,'ECA/PE/C/PY')

--Program/Subprogram
,('CBYX - High School Program','CBYX','ECA/PE/C/PY')
,('CBYX - Young Professionals Program','CBYX','ECA/PE/C/PY')
,('CBYX - Vocational Program','CBYX','ECA/PE/C/PY')
,('General','Short Term Youth Leadership Programs','ECA/PE/C/PY')
,('On Demand','Youth Leadership On Demand','ECA/PE/C/PY')
,('AYLP','American Youth Leadership Program','ECA/PE/C/PY')
,('YAP','Youth Ambassadors Program','ECA/PE/C/PY')

--Program/Subprogram
,('FLEX FY13','FLEX','ECA/PE/C/PY')
,('A-SMYLE FY 13','A-SMYLE','ECA/PE/C/PY')
,('YES FY13','YES','ECA/PE/C/PY')
,('CBYX - High School Program FY13','CBYX - High School Program','ECA/PE/C/PY')
,('CBYX - Young Professionals Program FY13','CBYX - Young Professionals Program','ECA/PE/C/PY')
,('CBYX - Vocational Program FY13','CBYX - Vocational Program','ECA/PE/C/PY')
,('Workshop FY13','English Language Workshops for Alumni','ECA/PE/C/PY')
,('TechGirls FY13','Tech Girls','ECA/PE/C/PY')
,('NSLI-Y FY13','National Security Language Initiative for Youth (NSLI-Y)','ECA/PE/C/PY')
,('On Demand FY13','On Demand','ECA/PE/C/PY')
,('GAPP FY13','German-American Partnership Program (GAPP)','ECA/PE/C/PY')

-- ECA/PE/C/CU
--Program
,('Arts Envoy',NULL,'ECA/PE/C/CU')
,('American Film Showcase',NULL,'ECA/PE/C/CU')
,('American Music Abroad',NULL,'ECA/PE/C/CU')
,('DanceMotion USA',NULL,'ECA/PE/C/CU')
,('Next Level',NULL,'ECA/PE/C/CU') 
,('American Arts Incubator',NULL,'ECA/PE/C/CU')
,('Community Engagement through Mural Arts',NULL,'ECA/PE/C/CU')
,('One Beat',NULL,'ECA/PE/C/CU') 
,('Center Stage',NULL,'ECA/PE/C/CU')
,('International Writing Program',NULL,'ECA/PE/C/CU')
,('Museums Connect',NULL,'ECA/PE/C/CU')
,('U.S. Art Biennale',NULL,'ECA/PE/C/CU')
,('U.S. Architecture Biennale',NULL,'ECA/PE/C/CU')
,('Biennale - U.S. Pavilion Support',NULL,'ECA/PE/C/CU')

--Program/Subprogram
,('American Film Showcase FY13','American Film Showcase','ECA/PE/C/CU')
,('American Music Abroad FY13','American Music Abroad','ECA/PE/C/CU')
,('DanceMotion USA FY13','DanceMotion USA','ECA/PE/C/CU')
,('Next Level FY13','Next Level','ECA/PE/C/CU')
,('American Arts Incubator FY13','American Arts Incubator','ECA/PE/C/CU')
,('Community Engagement through Mural Arts FY13','Community Engagement through Mural Arts','ECA/PE/C/CU')
,('One Beat FY13','One Beat','ECA/PE/C/CU')
,('Center Stage FY12','Center Stage','ECA/PE/C/CU')
,('International Writing Program FY13','International Writing Program','ECA/PE/C/CU')
,('Museums Connect FY13','Museums Connect','ECA/PE/C/CU')
,('U.S. Art Biennale FY12','U.S. Art Biennale','ECA/PE/C/CU')
,('U.S. Architecture Biennale FY13','U.S. Architecture Biennale','ECA/PE/C/CU')
,('Biennale - U.S. Pavilion Support FY13','Biennale - U.S. Pavilion Support','ECA/PE/C/CU')

-- ECA/PE/C/SU
--Program
,('International Sports Programming Initiative',NULL,'ECA/PE/C/SU')
,('Global Sports Mentoring Program',NULL,'ECA/PE/C/SU')
,('Empowering Women & Girls through Sports Initiative',NULL,'ECA/PE/C/SU')
,('Sports for Community',NULL,'ECA/PE/C/SU')
,('Sports Visitor',NULL,'ECA/PE/C/SU')
,('Sports Envoys',NULL,'ECA/PE/C/SU') 

--Program/Subprogram
,('National Ability Center','International Sports Programming Initiative','ECA/PE/C/SU')
,('Empowering Women and Girls Through Sports FY15','Global Sports Mentoring Program','ECA/PE/C/SU')
,('Sport for Community FY15','Global Sports Mentoring Program','ECA/PE/C/SU')
,('Visitors','Empowering Women & Girls through Sports Initiative','ECA/PE/C/SU')
,('Envoys','Empowering Women & Girls through Sports Initiative','ECA/PE/C/SU')
,('Global Sports Mentoring FY13','Empowering Women & Girls through Sports Initiative','ECA/PE/C/SU')
,('Sport for Community FY14','Sports for Community','ECA/PE/C/SU')
,('Sports Visitors FY13','Sports Visitor','ECA/PE/C/SU')
,('Sports Envoys FY13','Sports Envoys','ECA/PE/C/SU')


/* ******************** */
/*  PROCESS STARTS HERE */
/* ******************** */

/* Process the rows - update existing owner_organizationid - add new ones */
declare @i int
select @i = min(RowID) from @Programs
declare @max int
select @max = max(RowID) from @Programs

declare @ProgramName nvarchar(255)
declare @ParentProgramName  nvarchar(255)
declare @ParentProgramId  int
declare @Owner_Organizationid  int
declare @Owner_OfficeSymbol  nvarchar(128)
declare @sqlstring  nvarchar(4000)
declare @existingprogramid  int
DECLARE @OfficeId int

/* Set the Program status to 'Other' for all existing ACTIVE programs in the included offices - this will be changed to Active with update or insert */
UPDATE dbo.program
SET ProgramStatusId = @OtherStatusId
WHERE owner_organizationid IN (SELECT organizationid 
                                 FROM dbo.Organization 
                                WHERE officesymbol IN (SELECT DISTINCT Owner_OfficeSymbol 
                                                         FROM @Programs)) 
AND programstatusid = @ActiveStatusId

/* Loop through all the rows in the temp table */
while @i <= @max begin

    /* Read the row and Get the OrganizationId for the office */
    SELECT @ProgramName = P.ProgramName,
           @ParentProgramName = P.ParentProgramName,
           @Owner_OfficeSymbol = P.Owner_OfficeSymbol, 
           @OfficeId = O.OrganizationId
      FROM @Programs P
      LEFT JOIN dbo.organization O 
        ON (O.OfficeSymbol = P.Owner_OfficeSymbol)
     WHERE RowID = @i 

    /* Find the correct Parent program - takes care of duplicates in hierarchy */
    SELECT @ParentProgramId = ProgramId
      FROM dbo.Program
     WHERE UPPER(Name) = UPPER(@ParentProgramName) 
       AND Owner_OrganizationId = @OfficeId 
       AND ProgramStatusId = @ActiveStatusId
     ORDER BY ParentProgram_ProgramId DESC


    /* Get the existing ProgramId */
    SET @existingprogramid = NULL
    /* Need to see if program exists */
    IF @ParentProgramId IS NULL
      SELECT @existingProgramId = programid 
        FROM dbo.Program 
       WHERE UPPER(Name) = UPPER(@ProgramName) AND ParentProgram_ProgramId IS NULL AND owner_organizationid = @OfficeId
    ELSE
      SELECT @existingProgramId = programid 
        FROM dbo.Program 
       WHERE UPPER(name) = UPPER(@ProgramName) AND ParentProgram_ProgramId = @ParentProgramId AND owner_organizationid = @OfficeId

    IF @existingprogramid IS NOT NULL
      /* Update the existing program - if active assign null enddate if enddate was previously generated */
      UPDATE dbo.program 
      SET Name = @ProgramName,
          Description = CASE
                           WHEN UPPER(Description) = UPPER(@ProgramName) THEN @ProgramName
                           ELSE Description
                        END,
          ProgramStatusId = CASE 
                               WHEN ProgramStatusId = @OtherStatusId THEN @ActiveStatusId
                               WHEN CAST(enddate AS DATE) <= CONVERT (date, GETDATE()) THEN @CompletedStatusId
                               ELSE ProgramStatusId
                            END,
          EndDate = CASE 
                      WHEN CAST(enddate AS Date) = '2016-01-01' THEN NULL
		      WHEN CAST(enddate AS Date) < CAST(startdate AS Date) THEN NULL
                      ELSE enddate
                    END,
          Owner_OrganizationId = @OfficeId,
          ParentProgram_ProgramId = @ParentProgramId,
	  History_RevisedOn = CAST(N'2015-09-24T00:00:00.0000000-00:00' AS DateTimeOffset),
	  History_RevisedBy = 1
      WHERE programid = @existingprogramid
    ELSE
      /* Add the new program as an Active Program */
      INSERT 
      INTO dbo.program
          (ProgramStatusId,Name,Description,StartDate,EndDate,
           History_CreatedBy,History_CreatedOn,History_RevisedBy,History_RevisedOn,
           ParentProgram_ProgramId,Owner_OrganizationId) 
      VALUES (@ActiveStatusId,@ProgramName,@ProgramName,CAST(N'2015-01-01T00:00:00.0000000-00:00' AS DateTimeOffset),
              NULL,1,CAST(N'2015-09-24T00:00:00.0000000-00:00' AS DateTimeOffset),
              1,CAST(N'2015-09-24T00:00:00.0000000-00:00' AS DateTimeOffset),@ParentProgramId,@OfficeId)

    set @i = @i + 1
end

/* Should be done here */
GO