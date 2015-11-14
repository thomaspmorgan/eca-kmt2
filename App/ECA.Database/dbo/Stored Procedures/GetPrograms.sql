
-- =============================================
-- Author:		Doug Krehbel
-- Create date: 5/5/2015
-- Description:	Get all ECA programs recursively and ordered - updated from previous using cte and recursion
-- =============================================
CREATE PROCEDURE [dbo].[GetPrograms]

AS
BEGIN


with cte as
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
		ProgramStatus.Status,
		[path] +'-'+ cast(row_number()over(partition by t.parentProgram_ProgramId order by t.name) as varchar(max)),
		programLevel+1,
		sortOrder + row_number()over(partition by t.parentProgram_ProgramId order by t.name) / power(10.0,programlevel+1) 
	from
		cte
		Join Organization as Org on Owner_OrganizationId = Org.OrganizationId
		join program t on cte.ProgramId = t.parentProgram_ProgramId
		Join ProgramStatus On cte.ProgramStatusId = ProgramStatus.ProgramStatusId
)
   
select * from cte order by sortOrder

END
