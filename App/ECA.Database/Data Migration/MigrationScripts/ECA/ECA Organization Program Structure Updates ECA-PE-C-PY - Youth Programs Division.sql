/* This script will update existing programs for a selected office or insert the ones that don't exist */

/* Office Of Citizen Exchanges - Youth Programs Division ECA/PE/C/PY */

/* Get the OrganizationId for the office */
DECLARE @OfficeId int
SELECT @OfficeId = organizationid FROM organization WHERE officesymbol = 'ECA/PE/C/PY'

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
('FLEX',NULL,@OfficeId)
,('A-SMYLE',NULL,@OfficeId)
,('YES',NULL,@OfficeId)
,('YES Abroad',NULL,@OfficeId)
,('CBYX',NULL,@OfficeId)
,('English Language Workshops for Alumni',NULL,@OfficeId)
,('Tech Girls',NULL,@OfficeId)
,('National Security Language Initiative for Youth (NSLI-Y)',NULL,@OfficeId)
,('Short Term Youth Leadership Programs',NULL,@OfficeId)
,('Youth Leadership On Demand',NULL,@OfficeId)
,('American Youth Leadership Program',NULL,@OfficeId)
,('Youth Ambassadors Program',NULL,@OfficeId)
,('German-American Partnership Program (GAPP)',NULL,@OfficeId)


--Program/Subprogram
,('CBYX - High School program','CBYX',@OfficeId)
,('CBYX - Young Professionals program','CBYX',@OfficeId)
,('CBYX - Vocational program','CBYX',@OfficeId)
,('General','Short Term Youth Leadership Programs',@OfficeId)
,('On Demand','Youth Leadership On Demand',@OfficeId)
,('AYLP','American Youth Leadership Program',@OfficeId)
,('YAP','Youth Ambassadors Program',@OfficeId)


--Program/Subprogram
,('FLEX FY13','FLEX',@OfficeId)
,('A-SMYLE FY 13','A-SMYLE',@OfficeId)
,('YES FY13','YES',@OfficeId)
,('CBYX - High School program FY13','CBYX - High School program',@OfficeId)
,('CBYX - Young Professionals program FY13','CBYX - Young Professionals program',@OfficeId)
,('CBYX - Vocational program FY13','CBYX - Vocational program',@OfficeId)
,('Workshop FY13','English Language Workshops for Alumni',@OfficeId)
,('TechGirls FY13','Tech Girls',@OfficeId)
,('NSLI-Y FY13','National Security Language Initiative for Youth (NSLI-Y)',@OfficeId)
,('On Demand FY13','On Demand',@OfficeId)
,('GAPP FY13','German-American Partnership Program (GAPP)',@OfficeId)



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






 

