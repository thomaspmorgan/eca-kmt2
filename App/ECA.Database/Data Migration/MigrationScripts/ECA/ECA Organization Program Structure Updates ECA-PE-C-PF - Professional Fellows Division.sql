/* This script will update existing programs for a selected office or insert the ones that don't exist */

/* Office Of Citizen Exchanges - Professional Fellows Division ECA/PE/C/PF */

/* Get the OrganizationId for the office */
DECLARE @OfficeId int
SELECT @OfficeId = organizationid FROM organization WHERE officesymbol = 'ECA/PE/C/PF'

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
('Professional Fellows',NULL,@OfficeId)
,('Professional Fellows - YSEALI',NULL,@OfficeId)
,('Professional Fellows On Demand',NULL,@OfficeId)
,('Community Solutions',NULL,@OfficeId)
,('Professional Fellows Congress',NULL,@OfficeId)
,('TechWomen',NULL,@OfficeId)
,('Pakistan Journalism Program',NULL,@OfficeId)
,('Fortune/U.S. Department of State Global Women''s Mentoring Partnership',NULL,@OfficeId)
,('Goldman Sachs 10,000 Women - U.S. Department of State Entrepreneurship Program',NULL,@OfficeId)
,('Business Leadership Program for Young Russians',NULL,@OfficeId)
,('Digital Communications Network (=Russian Periphery Digital Communicators Network',NULL,@OfficeId)
,('Japan-US Friendship Commission (CULCON)',NULL,@OfficeId)
,('Mike Mansfield Fellowship Program',NULL,@OfficeId)
,('National Youth Science Camp',NULL,@OfficeId)
,('U.S. Congress-Korea National Assembly Youth Exchange',NULL,@OfficeId)
,('Traditional Public Private Partnerships (TPPP)',NULL,@OfficeId)

--Program/Subprogram
,('TPPP: American Council of Young Political Leaders (ACYPL)','Traditional Public Private Partnerships (TPPP)',@OfficeId)
,('TPPP: ACILS','Traditional Public Private Partnerships (TPPP)',@OfficeId)
,('TPPP: Partners of the Americas','Traditional Public Private Partnerships (TPPP)',@OfficeId)
,('TPPP: Sister Cities International','Traditional Public Private Partnerships (TPPP)',@OfficeId)
,('TPPP: Institute for Representative Government','Traditional Public Private Partnerships (TPPP)',@OfficeId)
 


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
	  History_RevisedOn = GETDATE(),
	  History_RevisedBy = 1
      WHERE programid = @existingprogramid
    ELSE
      INSERT 
      INTO dbo.program
          (ProgramStatusId,Name,Description,StartDate,EndDate,
           History_CreatedBy,History_CreatedOn,History_RevisedBy,History_RevisedOn,
           ParentProgram_ProgramId,Owner_OrganizationId) 
      VALUES (@ActiveStatusId,@ProgramName,@ProgramName,CAST(N'2015-01-01T00:00:00.0000000-00:00' AS DateTimeOffset),
              NULL,1,GETDATE(),
              1,GETDATE(),@ParentProgramId,@OfficeId)

    set @i = @i + 1
end

/* Should be done here */
GO






 

