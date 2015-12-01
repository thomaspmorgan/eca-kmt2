/* This script will update existing programs for a selected office or insert the ones that don't exist */


/* Check which programs already exist */
/*  -- just for debug
SELECT * FROM program WHERE name IN 
(
'Fulbright Program'

,'Critical Language Scholarship (CLS) Program'
,'Gilman Program'
,'U.S. Study Abroad'
,'Hubert H. Humphrey Fellowship Program'
,'Community College Initiative Program'
,'Community College Administrator Program'
,'Thomas Jefferson Scholarship Program'
,'Fubright Distinguished Awards in Teaching' 
,'Teaching Excellence and Achievement Program'
,'International Leaders in Education Program'
,'Teachers for Global Classrooms Program'
,'Teachers of Critical Languages Program'
,'EducationUSA'

,'Capacity-Building Program for U.S. Undergraduate Study Abroad'
,'Tunisia Community College Scholarship Program'
,'Opportunity Funds'
,'Competitive College Club'
,'United States Achievers Program'
,'Cohort-Advising Program (Other)'
,'Leadership Institutes'
,'EducationUSA Academy'
,'Open Doors Survey'
,'Global EducationUSA Services'
,'Advising Operations in Post-Soviet Countries'
,'Advising Operations in the Middle East and North Africa'
,'EducationUSA Interactive'
,'EducationUSA Fairs'
,'EducationUSA Recycling Program'


)
*/

/* Get the OrganizationId for the office */
DECLARE @OfficeId int
SELECT @OfficeId = organizationid FROM organization WHERE officesymbol = 'ECA/A/S'

/* Get the 'Active' program status ID */
DECLARE @ActiveStatusId int
SELECT @ActiveStatusId = programstatusid FROM dbo.programstatus WHERE status = 'Active'

/* Get the 'Other' program status ID */
DECLARE @OtherStatusId int
SELECT @OtherStatusId = programstatusid FROM dbo.programstatus WHERE status = 'Other'

/* Set the Program status to 'Other' for all existing programs in this office - this will be changed to Active with update or insert */
UPDATE dbo.program
SET ProgramStatusId = @OtherStatusId
WHERE owner_organizationid = @OfficeId

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
,('Critical Language Scholarship (CLS) Program',NULL,@OfficeId)
,('Gilman Program',NULL,@OfficeId)
,('U.S. Study Abroad',NULL,@OfficeId)
,('Hubert H. Humphrey Fellowship Program','Fulbright Program',@OfficeId)
,('Community College Initiative Program',NULL,@OfficeId)
,('Community College Administrator Program',NULL,@OfficeId)
,('Thomas Jefferson Scholarship Program',NULL,@OfficeId)
,('Fubright Distinguished Awards in Teaching','Fulbright Program',@OfficeId )
,('Teaching Excellence and Achievement Program',NULL,@OfficeId)
,('International Leaders in Education Program',NULL,@OfficeId)
,('Teachers for Global Classrooms Program',NULL,@OfficeId)
,('Teachers of Critical Languages Program',NULL,@OfficeId)
,('EducationUSA',NULL,@OfficeId)

--Subprogram
,('Capacity-Building Program for U.S. Undergraduate Study Abroad','U.S. Study Abroad',@OfficeId)
,('Tunisia Community College Scholarship Program','Thomas Jefferson Scholarship Program',@OfficeId)
,('Opportunity Funds','EducationUSA',@OfficeId)
,('Competitive College Club','EducationUSA',@OfficeId)
,('United States Achievers Program','EducationUSA',@OfficeId)
,('Cohort-Advising Program (Other)','EducationUSA',@OfficeId)
,('Leadership Institutes','EducationUSA',@OfficeId)
,('EducationUSA Academy','EducationUSA',@OfficeId)
,('Open Doors Survey','EducationUSA',@OfficeId)
,('Global EducationUSA Services','EducationUSA',@OfficeId)
,('Advising Operations in Post-Soviet Countries','EducationUSA',@OfficeId)
,('Advising Operations in the Middle East and North Africa','EducationUSA',@OfficeId)
,('EducationUSA Interactive','EducationUSA',@OfficeId)
,('EducationUSA Fairs','EducationUSA',@OfficeId)
,('EducationUSA Recycling Program','EducationUSA',@OfficeId)

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
    IF @ParentProgramId IS NULL
      SELECT @existingProgramId = programid 
        FROM dbo.Program 
       WHERE name = @ProgramName AND ParentProgram_ProgramId IS NULL AND owner_organizationid = @Owner_OrganizationId
    ELSE
      SELECT @existingProgramId = programid 
        FROM dbo.Program 
       WHERE name = @ProgramName AND ParentProgram_ProgramId = @ParentProgramId AND owner_organizationid = @Owner_OrganizationId

    IF @existingprogramid IS NOT NULL
      UPDATE dbo.program 
      SET ProgramStatusId = @ActiveStatusId,
          Owner_OrganizationId = @Owner_OrganizationId,
          ParentProgram_ProgramId = @ParentProgramId,
	  History_RevisedOn = CAST(N'2015-09-17T00:00:00.0000000-00:00' AS DateTimeOffset),
	  History_RevisedBy = 1
      WHERE programid = @existingprogramid
    ELSE
      INSERT 
      INTO dbo.program
          (ProgramStatusId,Name,Description,StartDate,EndDate,
           History_CreatedBy,History_CreatedOn,History_RevisedBy,History_RevisedOn,
           ParentProgram_ProgramId,Owner_OrganizationId) 
      VALUES (@ActiveStatusId,@ProgramName,@ProgramName,CAST(N'2015-01-01T00:00:00.0000000-00:00' AS DateTimeOffset),
              NULL,1,CAST(N'2015-09-17T00:00:00.0000000-00:00' AS DateTimeOffset),
              1,CAST(N'2015-09-17T00:00:00.0000000-00:00' AS DateTimeOffset),@ParentProgramId,@OfficeId)

    set @i = @i + 1
end

/* Should be done here */
--GO






 

