/* This script updates the Owner_OrganizationID and the ParentProgram_ProgramID on existing Programs */
USE ECA_Dev_Test
GO

Declare @ProgramName nvarchar(max)
Declare @OfficeName nvarchar(max)
Declare @OfficeSymbol nvarchar(max)
Declare @OrganizationId  int
Declare @OrganizationName  nvarchar(max)
Declare @ProgramId  int
Declare @Owner_OrganizationId  int
Declare @ParentProgram_ProgramId  int
Declare @ParentProgramId   int
Declare @ParentProgramName  nvarchar(max)
Declare @OfficeNameid int
Declare @cursorProgram CURSOR

/* Define the cursor */
SET @cursorProgram = CURSOR FOR
SELECT p.programid,p.owner_organizationid,p.parentprogram_programid,
       fp.[Program Name],fp.[Office Name],fp.[Office Symbol],
       o.organizationid,o.name,
       p1.programid,p1.name
FROM ECA_Dev.ECA_Dev.dbo.program p
JOIN ECA_Data_Migration.dbo.FullProgramStaging fp ON (p.name = fp.[Program Name])
JOIN ECA_Dev.ECA_Dev.dbo.organization o ON (o.name = fp.[Office Symbol])
LEFT JOIN ECA_Dev.ECA_Dev.dbo.program p1 ON (p1.name = fp.[Parent Programs])

/* Open the cursor */
OPEN @cursorProgram

/* Fetch the first program */
FETCH NEXT FROM @cursorProgram INTO @ProgramId,@Owner_OrganizationId,@ParentProgram_ProgramId,
                @ProgramName,@OfficeName,@OfficeSymbol,
                @OrganizationId,@OrganizationName,
                @ParentProgramId,@ParentProgramName

/* Loop thru all programs (staging) - should match DB */
WHILE @@FETCH_STATUS = 0
BEGIN

/* update the parentprogramid and owner organizationid on program */
UPDATE eca_dev.eca_dev.dbo.program 
   SET Owner_OrganizationId = @OrganizationId,
       ParentProgram_ProgramId = @ParentProgramId
 WHERE ProgramId = @ProgramId

/* Fetch the next program */
FETCH NEXT FROM @cursorProgram INTO @ProgramId,@Owner_OrganizationId,@ParentProgram_ProgramId,
                @ProgramName,@OfficeName,@OfficeSymbol,
                @OrganizationId,@OrganizationName,
                @ParentProgramId,@ParentProgramName

END

/* Cleanup */
CLOSE @cursorProgram
DEALLOCATE @cursorProgram
GO






/*
select [Program Name],[Office Name],[Office Symbol],p.name,p.programid,p.owner_organizationid,o2.name ,o.organizationid orgid2,o.name orgname2,o.organizationtypeid,o.description 
from FullProgramStaging fp
left join eca_dev_test.dbo.organization o on (o.name = fp.[Office Symbol])
left join eca_dev_test.dbo.program p on (p.name = fp.[Program Name])
left join eca_dev_test.dbo.organization o2 on (o2.organizationid = p.owner_organizationid)
where fp.[Program Name] is not null
/*