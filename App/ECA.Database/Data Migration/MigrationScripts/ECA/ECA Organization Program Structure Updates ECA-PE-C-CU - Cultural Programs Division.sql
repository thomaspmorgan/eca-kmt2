/* This script will update existing programs for a selected office or insert the ones that don't exist */

/* Office Of Citizen Exchanges - Cultural Programs Division ECA/PE/C/CU */

/* Get the OrganizationId for the office */
DECLARE @OfficeId int
SELECT @OfficeId = organizationid FROM organization WHERE officesymbol = 'ECA/PE/C/CU'

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

('Arts Envoy',NULL,@OfficeId)
,('American Film Showcase',NULL,@OfficeId)
,('American Music Abroad',NULL,@OfficeId)
,('DanceMotion USA',NULL,@OfficeId)
,('Next Level',NULL,@OfficeId) 
,('American Arts Incubator',NULL,@OfficeId)
,('Community Engagement through Mural Arts',NULL,@OfficeId)
,('One Beat',NULL,@OfficeId) 
,('Center Stage',NULL,@OfficeId)
,('International Writing Program',NULL,@OfficeId)
,('Museums Connect',NULL,@OfficeId)
,('U.S. Art Biennale',NULL,@OfficeId)
,('U.S. Architecture Biennale',NULL,@OfficeId)
,('Biennale - U.S. Pavilion Support',NULL,@OfficeId)



--Program/Subprogram
,('American Film Showcase FY13','American Film Showcase',@OfficeId)
,('American Music Abroad FY13','American Music Abroad',@OfficeId)
,('DanceMotion USA FY13','DanceMotion USA',@OfficeId)
,('Next Level FY13','Next Level',@OfficeId)
,('American Arts Incubator','American Arts Incubator',@OfficeId)
,('Community Engagement through Mural Arts','Community Engagement through Mural Arts',@OfficeId)
,('One Beat FY13','One Beat',@OfficeId)
,('Center Stage FY12','Center Stage',@OfficeId)
,('International Writing Program FY13','International Writing Program',@OfficeId)
,('Museums Connect FY13','Museums Connect',@OfficeId)
,('U.S. Art Biennale FY12','U.S. Art Biennale',@OfficeId)
,('U.S. Architecture Biennale FY13','U.S. Architecture Biennale',@OfficeId)
,('Biennale - U.S. Pavilion Support FY13','Biennale - U.S. Pavilion Support',@OfficeId)



--Program/Subprogram




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
GO






 

