-- =============================================
-- Author:		Brandon Tucker
-- Create date: 1/13/2016
-- Description:	Get all organizations
-- =============================================
CREATE PROCEDURE [dbo].[GetOrganizations]
	-- Add the parameters for the stored procedure here
AS
BEGIN

With Offices As
(SELECT TopLevelOrg.[OrganizationId]
      ,TopLevelOrg.[OrganizationTypeId]
	  ,OrgType.OrganizationTypeName as [OrganizationType]
	  ,TopLevelOrg.[Name]
      ,[ParentOrganization_OrganizationId]
	  ,cast(row_number()over(partition by TopLevelOrg.ParentOrganization_OrganizationId order by TopLevelOrg.Name) as varchar(max)) as [Path]
	  ,CASE WHEN EXISTS (SELECT 1 FROM dbo.Organization AS o1 WHERE o1.ParentOrganization_OrganizationId = TopLevelOrg.OrganizationId) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS HasChildren
	  ,1 as OfficeLevel
	  ,row_number()over(partition by TopLevelOrg.ParentOrganization_OrganizationId order by TopLevelOrg.name) / power(10.0, 0) as SortOrder
  FROM [Organization] as TopLevelOrg
  JOIN OrganizationType as OrgType
  ON TopLevelOrg.OrganizationTypeId = orgType.OrganizationTypeId
  where TopLevelOrg.ParentOrganization_OrganizationId Is Null and TopLevelOrg.OrganizationTypeId Not In (1,2,3) and TopLevelOrg.Status = 'Active'

  union all

  SELECT Org.[OrganizationId]
      ,Org.[OrganizationTypeId]
	  ,OrgType.OrganizationTypeName as [OrganizationType]
	  ,Org.Name
      ,Org.[ParentOrganization_OrganizationId]
	  ,[Path] +'-'+ cast(row_number()over(partition by Org.ParentOrganization_OrganizationId order by Org.Name) as varchar(max))
	  ,CASE WHEN EXISTS (SELECT 1 FROM dbo.Organization AS o2 WHERE o2.ParentOrganization_OrganizationId = Org.OrganizationId) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS HasChildren
	  , OL.OfficeLevel + 1
	  ,sortOrder + row_number()over(partition by Org.ParentOrganization_OrganizationId order by Org.Name) / power(10.0,OfficeLevel) 
  FROM [Organization] as Org
  
  JOIN OrganizationType as OrgType
  ON Org.OrganizationTypeId = orgType.OrganizationTypeId

  inner join Offices as OL On Org.ParentOrganization_OrganizationId = OL.OrganizationId
  where Org.ParentOrganization_OrganizationId Is Not Null and Org.OrganizationTypeId Not In (1,2,3) and Org.Status = 'Active')

  select * from Offices order by OfficeLevel

End
GO