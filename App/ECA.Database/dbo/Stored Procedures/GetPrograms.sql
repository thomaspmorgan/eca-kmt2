
-- =============================================
-- Author:		Tom Morgan
-- Create date: 3/13/2015
-- Description:	Get all ECA programs recursively and ordered
-- =============================================
CREATE PROCEDURE [dbo].[GetPrograms]
	-- Add the parameters for the stored procedure here
AS
BEGIN

With Programs As
(SELECT TopLevelProgram.[ProgramId]
,TopLevelProgram.[Name]
,TopLevelProgram.[Description]
      ,TopLevelProgram.[ParentProgram_ProgramId]
	  ,TopLevelProgram.[Owner_OrganizationId]
	  ,Org.Name As OrgName
	  ,1 As ProgramLevel
  FROM [Program] as TopLevelProgram
  Join Organization As Org On TopLevelProgram.Owner_OrganizationId = Org.OrganizationId
  where TopLevelProgram.ParentProgram_ProgramId Is Null and TopLevelProgram.ProgramStatusId = 1
  
  union all

  SELECT Prog.[ProgramId]
	,Prog.[Name]
	,Prog.[Description]
      ,Prog.[ParentProgram_ProgramId]
	  	,Prog.[Owner_OrganizationId]
		,Org.Name As OrgName
	  , PL.ProgramLevel + 1
  FROM [Program] as Prog
    Join Organization as Org on Prog.Owner_OrganizationId = Org.OrganizationId
  inner join Programs as PL On Prog.ParentProgram_ProgramId = PL.ProgramId

  where Prog.ParentProgram_ProgramId Is Not Null and Prog.ProgramStatusId = 1)

  select * from Programs order by Name, ProgramLevel

End
GO