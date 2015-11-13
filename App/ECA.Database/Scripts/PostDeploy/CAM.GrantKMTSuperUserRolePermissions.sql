DECLARE @officeResourceTypeName VARCHAR(25) = 'office';
DECLARE @programResourceTypeName VARCHAR(25) = 'program';
DECLARE @projectResourceTypeName VARCHAR(25) = 'project';

DECLARE @officeResourceTypeId INT;
DECLARE @programResourceTypeId INT;
DECLARE @projectResourceTypeId INT;

SELECT @officeResourceTypeId = ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @officeResourceTypeName
SELECT @programResourceTypeId = ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @programResourceTypeName
SELECT @projectResourceTypeId = ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @projectResourceTypeName


--Grant KMT Super User Role access to all projects, programs and offices and allow search and grant administrator permission
DECLARE @kmtSuperUserRoleName VARCHAR(25) = 'KMT Super User';
DECLARE @kmtSuperUserRoleId INT;
SELECT @kmtSuperUserRoleId = RoleId FROM cam.Role WHERE RoleName = @kmtSuperUserRoleName

DECLARE @kmtSearchPermissionName VARCHAR(25) = 'Search'
DECLARE @kmtSearchPermissionId INT;
SELECT @kmtSearchPermissionId = PermissionId FROM cam.Permission WHERE PermissionName = @kmtSearchPermissionName

DECLARE @kmtAdministratorPermissionName VARCHAR(25) = 'Administrator'
DECLARE @kmtAdministratorPermissionId INT;
SELECT @kmtAdministratorPermissionId = PermissionId FROM cam.Permission WHERE PermissionName = @kmtAdministratorPermissionName

DECLARE @kmtApplicationResourceId INT = 1;
DECLARE @systemUserId INT = 1;

DELETE FROM cam.RoleResourcePermission WHERE RoleId = @kmtSuperUserRoleId

INSERT INTO cam.RoleResourcePermission(RoleId, ResourceId, PermissionId, AssignedOn, AssignedBy)
SELECT
@kmtSuperUserRoleId as RoleId,
@kmtApplicationResourceId as ResourceId,
@kmtSearchPermissionId as PermissionId,
SYSDATETIMEOFFSET() as AssignedOn,
@systemUserId as AssignedBy

WHERE NOT EXISTS(
	SELECT 1
	FROM cam.RoleResourcePermission rrp
	WHERE rrp.ResourceId = @kmtApplicationResourceId
	AND rrp.PermissionId = @kmtSearchPermissionId
	AND rrp.RoleId = @kmtSuperUserRoleId
);
PRINT 'Inserted ' + CAST(@@RowCount AS VARCHAR(10)) + ' search kmt permission to kmt super user role.';


INSERT INTO cam.RoleResourcePermission(RoleId, ResourceId, PermissionId, AssignedOn, AssignedBy)
SELECT
@kmtSuperUserRoleId as RoleId,
@kmtApplicationResourceId as ResourceId,
@kmtAdministratorPermissionId as PermissionId,
SYSDATETIMEOFFSET() as AssignedOn,
@systemUserId as AssignedBy

WHERE NOT EXISTS(
	SELECT 1
	FROM cam.RoleResourcePermission rrp
	WHERE rrp.ResourceId = @kmtApplicationResourceId
	AND rrp.PermissionId = @kmtAdministratorPermissionId
	AND rrp.RoleId = @kmtSuperUserRoleId
);
PRINT 'Inserted ' + CAST(@@RowCount AS VARCHAR(10)) + ' administrator permission to kmt super user role.';


INSERT INTO cam.RoleResourcePermission(RoleId, ResourceId, PermissionId, AssignedOn, AssignedBy)
SELECT DISTINCT
@kmtSuperUserRoleId as RoleId,
r.ParentResourceId as ResourceId,
p.PermissionId as PermissionId,
SYSDATETIMEOFFSET() as AssignedOn,
@systemUserId as AssignedBy

FROM cam.Resource r, cam.Permission p
WHERE r.ResourceTypeId = @projectResourceTypeId
AND r.ParentResourceId IS NOT NULL
AND p.ResourceTypeId = @projectResourceTypeId
AND NOT EXISTS(
	SELECT 1
	FROM cam.RoleResourcePermission rrp
	WHERE rrp.ResourceId = r.ParentResourceId
	AND rrp.PermissionId = p.PermissionId
	AND rrp.RoleId = @kmtSuperUserRoleId
);
PRINT 'Inserted ' + CAST(@@RowCount AS VARCHAR(10)) + ' project permissions for the kmt super user role.';

INSERT INTO cam.RoleResourcePermission(RoleId, ResourceId, PermissionId, AssignedOn, AssignedBy)
SELECT DISTINCT
@kmtSuperUserRoleId as RoleId,
r.ParentResourceId as ResourceId,
p.PermissionId as PermissionId,
SYSDATETIMEOFFSET() as AssignedOn,
@systemUserId as AssignedBy

FROM cam.Resource r, cam.Permission p
WHERE r.ResourceTypeId = @programResourceTypeId
AND r.ParentResourceId IS NOT NULL
AND p.ResourceTypeId = @programResourceTypeId
AND NOT EXISTS(
	SELECT *
	FROM cam.RoleResourcePermission rrp
	WHERE rrp.ResourceId = r.ParentResourceId
	AND rrp.PermissionId = p.PermissionId
	AND rrp.RoleId = @kmtSuperUserRoleId
);
PRINT 'Inserted ' + CAST(@@RowCount AS VARCHAR(10)) + ' program permissions for the kmt super user role.';


INSERT INTO cam.RoleResourcePermission(RoleId, ResourceId, PermissionId, AssignedOn, AssignedBy)
SELECT DISTINCT
@kmtSuperUserRoleId as RoleId,
r.ResourceId as ResourceId,
p.PermissionId as PermissionId,
SYSDATETIMEOFFSET() as AssignedOn,
@systemUserId as AssignedBy

FROM cam.Resource r, cam.Permission p
WHERE r.ResourceTypeId = @officeResourceTypeId
AND p.ResourceTypeId = @officeResourceTypeId
AND NOT EXISTS(
	SELECT *
	FROM cam.RoleResourcePermission rrp
	WHERE rrp.ResourceId = r.ResourceId
	AND rrp.PermissionId = p.PermissionId
	AND rrp.RoleId = @kmtSuperUserRoleId
);
PRINT 'Inserted ' + CAST(@@RowCount AS VARCHAR(10)) + ' office permissions for the kmt super user role.';

GO