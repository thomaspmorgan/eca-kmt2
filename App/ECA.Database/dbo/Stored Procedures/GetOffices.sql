
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
	  ,OrgType.OrganizationTypeName as [OrganizationType]
	  ,TopLevelOffice.OfficeSymbol
	  ,TopLevelOffice.[Name]
	  ,TopLevelOffice.[Description]
      ,[ParentOrganization_OrganizationId]
	  ,cast(row_number()over(partition by TopLevelOffice.ParentOrganization_OrganizationId order by TopLevelOffice.Name) as varchar(max)) as [Path]
	  ,1 as OfficeLevel
	  ,row_number()over(partition by TopLevelOffice.ParentOrganization_OrganizationId order by TopLevelOffice.name) / power(10.0, 0) as SortOrder
  FROM [Organization] as TopLevelOffice
  JOIN OrganizationType as OrgType
  ON TopLevelOffice.OrganizationTypeId = orgType.OrganizationTypeId
  where TopLevelOffice.ParentOrganization_OrganizationId Is Null and TopLevelOffice.OrganizationTypeId In (1,2,3) and TopLevelOffice.Status = 'Active'

  union all

  SELECT Org.[OrganizationId]
      ,Org.[OrganizationTypeId]
	  ,OrgType.OrganizationTypeName as [OrganizationType]
	  ,Org.OfficeSymbol
	  ,Org.Name
      ,Org.Description
      ,Org.[ParentOrganization_OrganizationId]
	  ,[Path] +'-'+ cast(row_number()over(partition by Org.ParentOrganization_OrganizationId order by Org.Name) as varchar(max))
	  , OL.OfficeLevel + 1
	  ,sortOrder + row_number()over(partition by Org.ParentOrganization_OrganizationId order by Org.Name) / power(10.0,OfficeLevel) 
  FROM [Organization] as Org
  
  JOIN OrganizationType as OrgType
  ON Org.OrganizationTypeId = orgType.OrganizationTypeId

  inner join Offices as OL On Org.ParentOrganization_OrganizationId = OL.OrganizationId
  where Org.ParentOrganization_OrganizationId Is Not Null and Org.OrganizationTypeId In (1,2,3) and Org.Status = 'Active')

  select office.*, 
	  (select count(childOrg.OrganizationId) 
	  from Organization childOrg 
	  where office.OrganizationId = childOrg.ParentOrganization_OrganizationId 
		  and childOrg.OrganizationTypeId In (1,2,3) 
		  and childOrg.Status = 'Active') as NumberOfChildren
  from Offices office
  order by OfficeSymbol, OfficeLevel

End
GO