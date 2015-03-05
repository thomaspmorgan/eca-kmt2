Declare @Programname nvarchar(max),@officename nvarchar(max),@officesymbol nvarchar(max),@organizationid int
Declare @cursorProgram CURSOR

/* Define the cursor */
set @cursorProgram = CURSOR FOR
SELECT ps.name,ps.officename,ps.officesymbol,o.OrganizationId 
  FROM program_staging ps
  JOIN eca_dev.eca_dev.dbo.organization o
  ON (o.Name = ps.officesymbol)

/* Open the cursor */
OPEN @cursorProgram

/* Fetch the first project */
FETCH NEXT FROM @cursorProgram INTO @Programname, @officename, @officesymbol, @organizationid

/* Loop thru all projects (staging) - should match DB */
WHILE @@FETCH_STATUS = 0
BEGIN

/* update the parentprogramid on project */
UPDATE eca_dev.eca_dev.dbo.program SET owner_organizationid = @organizationid WHERE name = @Programname

/* Fetch the next project */
FETCH NEXT FROM @cursorProgram INTO @Programname, @officename, @officesymbol, @organizationid
END

/* Cleanup */
CLOSE @cursorProgram
DEALLOCATE @cursorProgram
GO


/* update the unknowns */
select p.name,p.ParentProgram_ProgramId,p.Owner_OrganizationId,
       o.Name,o.Description,o.organizationid,
	   pr.name ,pr.ParentProgram_ProgramId,pr.Owner_OrganizationId
from eca_dev.eca_dev.dbo.program p
left outer join eca_dev.eca_dev.dbo.organization o
on (o.organizationid = p.owner_organizationid)
left outer join eca_dev.eca_dev.dbo.program pr
on (pr.programid = p.parentprogram_programid)
where p.ParentProgram_ProgramId is not null
and p.Owner_OrganizationId <> pr.Owner_OrganizationId
and p.Owner_OrganizationId = 1

