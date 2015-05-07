CREATE FUNCTION [dbo].[NumberOfChildPrograms] 
(
	@programId int
)
RETURNS int
AS
BEGIN

	DECLARE @numChildren int

	set @numChildren = (Select count(*) from program where 
		parentProgram_ProgramId = @programId and programstatusID = 1);

	Return @numChildren

	END

	GO

