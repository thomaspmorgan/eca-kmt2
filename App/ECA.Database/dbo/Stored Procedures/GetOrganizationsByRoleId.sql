
CREATE PROCEDURE [dbo].[GetOrganizationsByRoleId]
	@OrganizationRoleId int = null
AS
BEGIN

WITH Organization_CTE AS
(SELECT ParentOrg.OrganizationId,
		ParentOrg.OrganizationTypeId,
		OrgType.OrganizationTypeName as OrganizationType,
		ParentOrg.Name,
		ParentOrg.ParentOrganization_OrganizationId,
		CAST (ROW_NUMBER() OVER (PARTITION BY ParentOrg.ParentOrganization_OrganizationId ORDER BY ParentOrg.Name) AS varchar(max)) AS Path,
		0 as OrganizationLevel,
		ROW_NUMBER() OVER (PARTITION BY ParentOrg.ParentOrganization_OrganizationId ORDER BY ParentOrg.Name) / power(10.0, 0) as SortOrder,
		ParentOrg.Status,
		ParentOrg.Description
 FROM Organization AS ParentOrg
 JOIN OrganizationType AS OrgType
 ON ParentOrg.OrganizationTypeId = OrgType.OrganizationTypeId
 WHERE ParentOrg.ParentOrganization_OrganizationId IS NULL AND ParentOrg.OrganizationTypeId NOT IN (1,2,3) AND ParentOrg.Status = 'Active'

 UNION ALL

 SELECT ChildOrg.OrganizationId,
        ChildOrg.OrganizationTypeId,
		OrgType.OrganizationTypeName as OrganizationType,
		ChildOrg.Name,
		ChildOrg.ParentOrganization_OrganizationId,
		[Path] +'-'+ CAST (ROW_NUMBER() OVER (PARTITION BY ChildOrg.ParentOrganization_OrganizationId ORDER BY ChildOrg.Name) AS varchar(max)),
		OrgCTE.OrganizationLevel + 1,
		SortOrder + ROW_NUMBER() OVER (PARTITION BY ChildOrg.ParentOrganization_OrganizationId ORDER BY ChildOrg.Name) / power(10.0, OrganizationLevel),
		ChildOrg.Status,
		ChildOrg.Description
 FROM Organization as ChildOrg
 JOIN OrganizationType as OrgType
 ON ChildOrg.OrganizationTypeId = OrgType.OrganizationTypeId
 INNER JOIN Organization_CTE AS OrgCTE ON ChildOrg.ParentOrganization_OrganizationId = OrgCTE.OrganizationId
 WHERE ChildOrg.ParentOrganization_OrganizationId IS NOT NULL AND ChildOrg.OrganizationTypeId Not In (1,2,3) and ChildOrg.Status = 'Active'

 )

SELECT Organization_CTE.*,
	(SELECT count(childOrg.OrganizationId) 
	  FROM Organization childOrg 
	  WHERE Organization_CTE.OrganizationId = childOrg.ParentOrganization_OrganizationId 
		  and childOrg.OrganizationTypeId NOT IN (1,2,3) 
		  and childOrg.Status = 'Active') as NumberOfChildren
FROM Organization_CTE 
WHERE OrganizationId IN (SELECT OrganizationId from OrganizationOrganizationRole WHERE OrganizationRoleId = @OrganizationRoleId) 
ORDER BY OrganizationLevel

END
GO