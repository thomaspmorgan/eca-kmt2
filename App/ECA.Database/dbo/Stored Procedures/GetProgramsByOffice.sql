
-- =============================================
-- Author:		Tom Morgan
-- Create date: 3/13/2015
-- Description:	Get all ECA programs recursively and ordered
-- =============================================
CREATE PROCEDURE [dbo].[GetProgramsByOffice]
	-- Add the parameters for the stored procedure here
	@OfficeId int
AS

Begin

With Programs As
(SELECT TopLevelProgram.[ProgramId]
,TopLevelProgram.[Name]
,TopLevelProgram.[Description]
      ,TopLevelProgram.[ParentProgram_ProgramId]
	  ,TopLevelProgram.[Owner_OrganizationId]
	  ,Org.Name As OrgName
	  ,Org.OfficeSymbol as OfficeSymbol
	  ,1 As ProgramLevel
  FROM [Program] as TopLevelProgram
  Join Organization As Org On TopLevelProgram.Owner_OrganizationId = Org.OrganizationId
  where TopLevelProgram.Owner_OrganizationId = @OfficeId and TopLevelProgram.ProgramStatusId = 1
  
  union all

  SELECT Prog.[ProgramId]
	,Prog.[Name]
	,Prog.[Description]
      ,Prog.[ParentProgram_ProgramId]
	  	,Prog.[Owner_OrganizationId]
		,Org.Name As OrgName
		,Org.OfficeSymbol as OfficeSymbol
	  , PL.ProgramLevel + 1
  FROM [Program] as Prog
    Join Organization as Org on Prog.Owner_OrganizationId = Org.OrganizationId
  inner join Programs as PL On Prog.ParentProgram_ProgramId = PL.ProgramId

  where Prog.Owner_OrganizationId != @OfficeId and Prog.ProgramStatusId = 1)

  select * from Programs order by Name, ProgramLevel

End