/* Moves all objects from CAM schema to cam schema so that data compare can be done */

/* create temporary schema */
CREATE SCHEMA [cam_tmp]
GO

/* move all existing objects to temp schema */
ALTER SCHEMA cam_tmp TRANSFER CAM.AccountStatus;
ALTER SCHEMA cam_tmp TRANSFER CAM.Application;
ALTER SCHEMA cam_tmp TRANSFER CAM.Permission;
ALTER SCHEMA cam_tmp TRANSFER CAM.PermissionAssignment;
ALTER SCHEMA cam_tmp TRANSFER CAM.Principal;
ALTER SCHEMA cam_tmp TRANSFER CAM.PrincipalRole;
ALTER SCHEMA cam_tmp TRANSFER CAM.PrincipalType;
ALTER SCHEMA cam_tmp TRANSFER CAM.Resource;
ALTER SCHEMA cam_tmp TRANSFER CAM.ResourceType;
ALTER SCHEMA cam_tmp TRANSFER CAM.Role;
ALTER SCHEMA cam_tmp TRANSFER CAM.RoleResourcePermission;
ALTER SCHEMA cam_tmp TRANSFER CAM.UserAccount;
GO

/* drop the uppercas CAM schema */
DROP SCHEMA [CAM]
GO

/* create the new schema */
CREATE SCHEMA [cam]
GO

/* move all the objects from the temp schema */
ALTER SCHEMA cam TRANSFER cam_tmp.AccountStatus;
ALTER SCHEMA cam TRANSFER cam_tmp.Application;
ALTER SCHEMA cam TRANSFER cam_tmp.Permission;
ALTER SCHEMA cam TRANSFER cam_tmp.PermissionAssignment;
ALTER SCHEMA cam TRANSFER cam_tmp.Principal;
ALTER SCHEMA cam TRANSFER cam_tmp.PrincipalRole;
ALTER SCHEMA cam TRANSFER cam_tmp.PrincipalType;
ALTER SCHEMA cam TRANSFER cam_tmp.Resource;
ALTER SCHEMA cam TRANSFER cam_tmp.ResourceType;
ALTER SCHEMA cam TRANSFER cam_tmp.Role;
ALTER SCHEMA cam TRANSFER cam_tmp.RoleResourcePermission;
ALTER SCHEMA cam TRANSFER cam_tmp.UserAccount;
GO

/* drop the temp schema */
DROP SCHEMA [cam_tmp]
GO