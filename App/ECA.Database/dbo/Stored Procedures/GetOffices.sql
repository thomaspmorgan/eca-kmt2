
-- =============================================
-- Author:		Tom Morgan
-- Create date: 3/13/2015
-- Description:	Get all ECA offices recursively and ordered
-- =============================================
CREATE PROCEDURE [dbo].[GetOffices]
	-- Add the parameters for the stored procedure here
AS
BEGIN

With Offices As
(SELECT TopLevelOffice.[OrganizationId]
      ,TopLevelOffice.[OrganizationTypeId]
	  ,TopLevelOffice.OfficeSymbol
	  ,TopLevelOffice.[Name]
	  ,TopLevelOffice.[Description]
      ,[ParentOrganization_OrganizationId],
	  1 as OfficeLevel
  FROM [Organization] as TopLevelOffice
  where TopLevelOffice.ParentOrganization_OrganizationId Is Null and TopLevelOffice.OrganizationTypeId In (1,2,3) and TopLevelOffice.Status = 'Active'

  union all

  SELECT Org.[OrganizationId]
      ,Org.[OrganizationTypeId]
	  ,Org.OfficeSymbol
	  ,Org.Name
      ,Org.Description
      ,Org.[ParentOrganization_OrganizationId]
	  , OL.OfficeLevel + 1
  FROM [Organization] as Org
  inner join Offices as OL On Org.ParentOrganization_OrganizationId = OL.OrganizationId
  where Org.ParentOrganization_OrganizationId Is Not Null and Org.OrganizationTypeId In (1,2,3) and Org.Status = 'Active')

  select * from Offices order by OfficeSymbol, OfficeLevel

End
GO