
-- =============================================
-- Author:		Doug Krehbel
-- Create date: 5/5/2015
-- Description:	Get all ECA programs recursively and ordered - updated from previous using cte and recursion
-- =============================================
CREATE PROCEDURE [dbo].[GetProgramsForUser]
(@userId int)
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
	prog.History_CreatedBy,
	Org.Name As OrgName,
	Org.OfficeSymbol as OfficeSymbol,
	dbo.NumberOfChildPrograms(prog.ProgramID) as NumChildren,
    cast(row_number()over(partition by prog.parentProgram_ProgramId order by prog.name) as varchar(max)) as [path],
    0 as programLevel,
    row_number()over(partition by prog.parentProgram_ProgramId order by prog.name) / power(10.0,0) as x
 
from program as prog
Join Organization as Org on Owner_OrganizationId = Org.OrganizationId
where ISNULL(parentProgram_ProgramId, 0) = 0
and (ProgramStatusId = 1 OR (ProgramStatusId = 4 AND prog.History_CreatedBy = @userId))

union all
select
    t.ProgramId,
    t.name,
	t.[description],
	t.[Owner_OrganizationId],
    t.parentProgram_ProgramId,
	t.programStatusId,
	t.History_CreatedBy,
	Org.Name As OrgName,
	Org.OfficeSymbol as OfficeSymbol,
	dbo.NumberOfChildPrograms(t.ProgramID) as NumChildren,
    [path] +'-'+ cast(row_number()over(partition by t.parentProgram_ProgramId order by t.name) as varchar(max)),
    programLevel+1,
    x + row_number()over(partition by t.parentProgram_ProgramId order by t.name) / power(10.0,programlevel+1)
 
from
    cte
Join Organization as Org on Owner_OrganizationId = Org.OrganizationId
join program t on cte.ProgramId = t.parentProgram_ProgramId
where t.ProgramStatusId = 1 OR (t.ProgramStatusId = 4 AND t.History_CreatedBy = @userId)

)
   
select * from cte order by x

END
