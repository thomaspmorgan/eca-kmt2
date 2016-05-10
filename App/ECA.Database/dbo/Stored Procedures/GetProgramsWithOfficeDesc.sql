
-- =============================================
-- Author:		Doug Krehbel
-- Create date: 5/5/2015
-- Description:	Get all ECA programs recursively and ordered - updated from previous using cte and recursion
-- =============================================
CREATE PROCEDURE [dbo].[GetProgramsWithOfficeDesc]

AS
BEGIN


with cteNew as
(
	select
		prog.ProgramID,
		prog.name,
		prog.[description],
		prog.[Owner_OrganizationId],
		prog.parentProgram_ProgramId,
		prog.programStatusId,
		prog.History_CreatedBy as CreatedByUserId,
		Org.Name As OrgName,
		Org.OfficeSymbol as OfficeSymbol,
		Org.Description as OfficeDescription,
		ProgramStatus.Status,
		cast(row_number()over(partition by prog.parentProgram_ProgramId order by prog.name) as varchar(max)) as [path],
		0 as programLevel,
		row_number()over(partition by prog.parentProgram_ProgramId order by prog.name) / power(10.0,0) as sortOrder 
	from program as prog
	Join Organization as Org on Owner_OrganizationId = Org.OrganizationId 
	Join ProgramStatus On prog.ProgramStatusId = ProgramStatus.ProgramStatusId
	where parentProgram_ProgramId is null

	union all

	select
		t.ProgramId,
		t.name,
		t.[description],
		t.[Owner_OrganizationId],
		t.parentProgram_ProgramId,
		t.programStatusId,
		t.History_CreatedBy as CreatedByUserId,
		Org.Name As OrgName,
		Org.OfficeSymbol as OfficeSymbol,
		Org.Description as OfficeDescription,
		ProgramStatus.Status,
		[path] +'-'+ cast(row_number()over(partition by t.parentProgram_ProgramId order by t.name) as varchar(max)),
		programLevel+1,
		sortOrder + row_number()over(partition by t.parentProgram_ProgramId order by t.name) / power(10.0,programlevel+1) 
	from
		cteNew
		Join Organization as Org on Owner_OrganizationId = Org.OrganizationId
		join program t on cteNew.ProgramId = t.parentProgram_ProgramId
		Join ProgramStatus On t.ProgramStatusId = ProgramStatus.ProgramStatusId
)
   
SELECT cteNew.*,
(
	SELECT COUNT(childProgram.ProgramId)
	FROM Program childProgram
	WHERE childProgram.ParentProgram_ProgramId = cteNew.ProgramId
) as NumberOfChildren

FROM cteNew 
ORDER BY SortOrder

END
