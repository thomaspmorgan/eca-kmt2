/* This script will update existing programs for a selected office or insert the ones that don't exist */

/* Academic Exchange Programs - ECA/A/E */

/* Get the OrganizationId for the office */
DECLARE @OfficeId int
SELECT @OfficeId = organizationid FROM organization WHERE officesymbol = 'ECA/A/E'

/* Get the 'Active' program status ID */
DECLARE @ActiveStatusId int
SELECT @ActiveStatusId = programstatusid FROM dbo.programstatus WHERE status = 'Active'

/* Get the 'Completed' program status ID */
DECLARE @CompletedStatusId int
SELECT @CompletedStatusId = programstatusid FROM dbo.programstatus WHERE status = 'Completed'

/* Get the 'Other' program status ID */
DECLARE @OtherStatusId int
SELECT @OtherStatusId = programstatusid FROM dbo.programstatus WHERE status = 'Other'

/* Set the Program status to 'Other' for all existing ACTIVE programs in this office - this will be changed to Active with update or insert */
UPDATE dbo.program
SET ProgramStatusId = @OtherStatusId
WHERE owner_organizationid = @OfficeId --AND programstatusid = @ActiveStatusId 

/* Create a temp table and store the program list */
DECLARE @Programs TABLE(RowID int not null identity(1,1) primary key,
                        ProgramName nvarchar(255),
                        ParentProgramName  nvarchar(255),
                        Owner_OrganizationId  int)

INSERT INTO @Programs (ProgramName,ParentProgramName,Owner_OrganizationId) 
VALUES 
--Program
('Fulbright Program',NULL,@OfficeId)

--Program/Subprogram
,('Fulbright Scholar Program','Fulbright Program',@OfficeId)
,('Fulbright Student Program','Fulbright Program',@OfficeId)
,('Study of the U.S. Institutes',NULL,@OfficeId)
,('Young African Leaders Initiative',NULL,@OfficeId)
,('Young Southeast Asian Leaders Initiative',NULL,@OfficeId) 

--Program/Subprogram
,('Fulbright Scholar Program from Partner Countries','Fulbright Scholar Program',@OfficeId)
,('Fulbright U.S. Scholar Program','Fulbright Scholar Program',@OfficeId)
,('Foreign Student Program from Partner Countries','Fulbright Student Program',@OfficeId)
,('Fulbright U.S. Student Program','Fulbright Student Program',@OfficeId)
,('Study of the U.S. Institute for Scholars','Study of the U.S. Institutes',@OfficeId)
,('Study of the U.S. Institute for Student Leaders','Study of the U.S. Institutes',@OfficeId)
,('Mandela Washington Fellowship','Young African Leaders Initiative',@OfficeId)
,('YSEALI Academic Fellowships','Young Southeast Asian Leaders Initiative',@OfficeId)
,('Afghan Junior Faculty Development',NULL,@OfficeId)
,('Fulbright Economics Teaching Program',NULL,@OfficeId)
,('International Center for Middle Eastern-Western Dialogue (Hollings Center)',NULL,@OfficeId)
,('Tibetan Scholarship Program',NULL,@OfficeId)
,('Undergraduate Exchange Program (UGRAD)',NULL,@OfficeId)
,('US-South Pacific Scholarship Program',NULL,@OfficeId)
,('US-Timor Leste Scholarship Program',NULL,@OfficeId)
,('Council of American Overseas Research Centers (CAORC)',NULL,@OfficeId)

--Subprogram
,('Fulbright ASEAN Visiting Scholar','Fulbright Scholar Program from Partner Countries',@OfficeId)
,('Fulbright International Education Administrators Program','Fulbright Scholar Program from Partner Countries',@OfficeId)
,('Fulbright Lecturer','Fulbright Scholar Program from Partner Countries',@OfficeId)
,('Fulbright Lecturer/Researcher','Fulbright Scholar Program from Partner Countries',@OfficeId)
,('Fulbright Scholar-in-Residence','Fulbright Scholar Program from Partner Countries',@OfficeId)
,('Fulbright Scholar Researcher','Fulbright Scholar Program from Partner Countries',@OfficeId)
,('Korean Secondary School Educator Program','Fulbright Scholar Program from Partner Countries',@OfficeId)
,('NEA Fulbright Junior Faculty Development Program','Fulbright Scholar Program from Partner Countries',@OfficeId)
,('Fulbright Arctic Initiative','Fulbright Scholar Program',@OfficeId)
,('Fulbright NEXUS','Fulbright Scholar Program',@OfficeId)
,('Fulbright-Fogarty Scholar','Fulbright U.S. Scholar Program',@OfficeId)
,('Fulbright International Education Administrators Program','Fulbright U.S. Scholar Program',@OfficeId)
,('Fulbright Lecturer','Fulbright U.S. Scholar Program',@OfficeId)
,('Fulbright Lecturer/Researcher','Fulbright U.S. Scholar Program',@OfficeId)
,('Fulbright Scholar Researcher','Fulbright U.S. Scholar Program',@OfficeId)
,('Fulbright Specialist','Fulbright U.S. Scholar Program',@OfficeId)
,('German Studies Seminar','Fulbright U.S. Scholar Program',@OfficeId)
,('Fulbright Faculty Development Program Moldova','Foreign Student Program from Partner Countries',@OfficeId)
,('Fulbright Faculty Development Program Ukraine','Foreign Student Program from Partner Countries',@OfficeId)
,('Fulbright Foreign Language Teaching Assistant','Foreign Student Program from Partner Countries',@OfficeId)
,('Fulbright Science and Technology Award','Foreign Student Program from Partner Countries',@OfficeId)
,('Israeli Outreach Program','Foreign Student Program from Partner Countries',@OfficeId)
,('Presidential Scholar','Foreign Student Program from Partner Countries',@OfficeId)
,('Fulbright Young American Journalists Program','Fulbright U.S. Student Program',@OfficeId)
,('Fulbright English Teaching Assistant','Fulbright U.S. Student Program',@OfficeId)
,('Fulbright-Fogarty Student','Fulbright U.S. Student Program',@OfficeId)
,('Fulbright mtvU Fellowship Program','Fulbright U.S. Student Program',@OfficeId)
,('Fulbright-National Geographic Digital Storytelling Fellowship','Fulbright U.S. Student Program',@OfficeId)
,('Fulbright Student Researcher','Fulbright U.S. Student Program',@OfficeId)
,('J William Fulbright - Hillary Rodham Clinton Fellowship','Fulbright U.S. Student Program',@OfficeId)
,('Contemporary American Literature','Study of the U.S. Institute for Scholars',@OfficeId)
,('Secondary Educators (Administrators)','Study of the U.S. Institute for Scholars',@OfficeId)
,('Secondary Educators (Teachers)','Study of the U.S. Institute for Scholars',@OfficeId)
,('Journalism and New Media','Study of the U.S. Institute for Scholars',@OfficeId)
,('Religious Pluralism in the United States','Study of the U.S. Institute for Scholars',@OfficeId)
,('U.S. Culture and Society','Study of the U.S. Institute for Scholars',@OfficeId)
,('U.S. Foreign Policy','Study of the U.S. Institute for Scholars',@OfficeId)
,('U.S. Political Thought','Study of the U.S. Institute for Scholars',@OfficeId)
,('Civic Engagement','Study of the U.S. Institute for Student Leaders',@OfficeId)
,('Global Environmental Issues','Study of the U.S. Institute for Student Leaders',@OfficeId)
,('Journalism and New Media','Study of the U.S. Institute for Student Leaders',@OfficeId)
,('Local, State, and Federal Public Policymaking for Pakistani Student Leaders','Study of the U.S. Institute for Student Leaders',@OfficeId)
,('Religious Pluralism in the United States','Study of the U.S. Institute for Student Leaders',@OfficeId)
,('Social Entrepreneurship','Study of the U.S. Institute for Student Leaders',@OfficeId)
,('U.S. History and Government','Study of the U.S. Institute for Student Leaders',@OfficeId)
,('Women''s Leadership','Study of the U.S. Institute for Student Leaders',@OfficeId)
,('Environmental Stewardship','Study of the U.S. Institute for Student Leaders',@OfficeId)
,('Business and Entrepreneurship','Mandela Washington Fellowship',@OfficeId)
,('Civic Leadership','Mandela Washington Fellowship',@OfficeId)
,('Public Management','Mandela Washington Fellowship',@OfficeId)
,('Environmental Issues','YSEALI Academic Fellowships',@OfficeId)
,('Civic Engagement','YSEALI Academic Fellowships',@OfficeId)
,('Social Entrepreneurship and Economic Development','YSEALI Academic Fellowships',@OfficeId)
,('Master of Public Policy (MPP)','Fulbright Economics Teaching Program',@OfficeId)
,('Vietnam Executive Leadership Training (VELT)','Fulbright Economics Teaching Program',@OfficeId)
,('Challenging Extremist Ideology, Propaganda and Messaging: Building the Counter-narrative','International Center for Middle Eastern-Western Dialogue (Hollings Center)',@OfficeId)
,('Protecting Cultural Heritage in Conflict and Preparing for Post-Conflict','International Center for Middle Eastern-Western Dialogue (Hollings Center)',@OfficeId)
,('Corporate Social Responsibility in Islam','International Center for Middle Eastern-Western Dialogue (Hollings Center)',@OfficeId)
,('High and Dry: Addressing the Middle East Water Challenge','International Center for Middle Eastern-Western Dialogue (Hollings Center)',@OfficeId)
,('UGRAD Serbia and Montenegro','Undergraduate Exchange Program (UGRAD)',@OfficeId)
,('UGRAD Tunisia','Undergraduate Exchange Program (UGRAD)',@OfficeId)
,('UGRAD Eurasia and Central Asia','Undergraduate Exchange Program (UGRAD)',@OfficeId)
,('UGRAD Pakistan','Undergraduate Exchange Program (UGRAD)',@OfficeId)
,('UGRAD Near East and Sub-Saharan Africa','Undergraduate Exchange Program (UGRAD)',@OfficeId)
,('UGRAD Western Hemisphere','Undergraduate Exchange Program (UGRAD)',@OfficeId)
,('UGRAD East Asia and Pacific' ,'Undergraduate Exchange Program (UGRAD)',@OfficeId)
,('Global Undergraduate Exchange Program (UGRAD)','Undergraduate Exchange Program (UGRAD)',@OfficeId)


/*  PROCESS STARTS HERE */

/* Process the rows - update existing owner_organizationid - add new ones */
declare @i int
select @i = min(RowID) from @Programs
declare @max int
select @max = max(RowID) from @Programs
declare @ProgramName nvarchar(255)
declare @ParentProgramName  nvarchar(255)
declare @ParentProgramId  int
declare @Owner_Organizationid  int
declare @sqlstring  nvarchar(4000)
declare @existingprogramid  int

while @i <= @max begin
    /* Read the row */
    SELECT @ProgramName = ProgramName,
           @ParentProgramName = ParentProgramName,
           @Owner_OrganizationId = Owner_OrganizationId 
    FROM @Programs 
    WHERE RowID = @i 

    /* Get the Parent Program if not NULL */
    SET @ParentProgramId = NULL
    IF @ParentProgramName IS NOT NULL
      SELECT @ParentProgramId = ProgramId 
        FROM dbo.Program 
       WHERE name = @ParentProgramName AND owner_organizationid = @Owner_OrganizationId

    /* Get the existing ProgramId */
    SET @existingprogramid = NULL
    /* Need to see if program exists */
    --IF @ParentProgramId IS NULL
    SELECT @existingProgramId = programid 
      FROM dbo.Program 
     WHERE name = @ProgramName 
       AND (ParentProgram_ProgramId IS NULL AND @ParentProgramId IS NULL OR ParentProgram_ProgramId = @ParentProgramId) 
       AND owner_organizationid = @Owner_OrganizationId
    --ELSE
    --  SELECT @existingProgramId = programid 
    --    FROM dbo.Program 
    --   WHERE name = @ProgramName AND ParentProgram_ProgramId = @ParentProgramId AND owner_organizationid = @Owner_OrganizationId

    IF @existingprogramid IS NOT NULL
      /* Update the existing program - if active assign null enddate if enddate was previously generated */
      UPDATE dbo.program 
      SET ProgramStatusId = @ActiveStatusId,
          Owner_OrganizationId = @Owner_OrganizationId,
          ParentProgram_ProgramId = @ParentProgramId,
	  History_RevisedOn = GETDATE(),
	  History_RevisedBy = 1
      WHERE programid = @existingprogramid
    ELSE      
     /* Add the new program as an Active Program */
      INSERT 
      INTO dbo.program
          (ProgramStatusId,Name,Description,StartDate,EndDate,
           History_CreatedBy,History_CreatedOn,History_RevisedBy,History_RevisedOn,ParentProgram_ProgramId,Owner_OrganizationId) 
      VALUES (@ActiveStatusId,@ProgramName,@ProgramName,CAST(N'2015-01-01T00:00:00.0000000-00:00' AS DateTimeOffset),
              NULL,1,GETDATE(),1,GETDATE(),@ParentProgramId,@OfficeId)

    set @i = @i + 1
end

/* Should be done here */
GO