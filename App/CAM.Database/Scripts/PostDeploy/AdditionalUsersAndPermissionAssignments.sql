
DECLARE @kmtApplicationId INT;
DECLARE @systemAdminPermissionId INT;
DECLARE @kmtSuperUserRoleId INT;

SELECT @kmtApplicationId = ResourceId FROM cam.Application WHERE ApplicationName = 'KMT';
SELECT @systemAdminPermissionId = PermissionId FROM cam.Permission WHERE PermissionName = 'Administrator';
SELECT @kmtSuperUserRoleId = RoleId FROM cam.Role WHERE RoleName = 'KMT Super User';

DECLARE @systemUserAdGuid UNIQUEIDENTIFIER = convert(uniqueidentifier, 'E6F49140-877B-4819-9E92-1427AF1F06AB');
DECLARE @thomasMorganAdGuid UNIQUEIDENTIFIER  = convert(uniqueidentifier, '8111908E-EFF7-4DE2-AEA2-F7BA54107008');
DECLARE @brianGibowskiAdGuid UNIQUEIDENTIFIER  = convert(uniqueidentifier, 'D56E313C-187E-462F-9171-0A6287F909FA');
DECLARE @brandonTuckerAdGuid UNIQUEIDENTIFIER  = convert(uniqueidentifier, '8735C8F8-949B-4342-A5AE-EE08CF99403F');

DECLARE @systemUserId INT = 0;
DECLARE @thomasMorganUserId INT = 0;
DECLARE @brianGibowskiUserId INT = 0;
DECLARE @brandonTuckerUserId INT = 0;


IF NOT EXISTS(SELECT * FROM cam.UserAccount WHERE AdGuid = @systemUserAdGuid)
BEGIN
	INSERT INTO cam.Principal(PrincipalTypeId) VALUES (1);
	SET @systemUserId = @@IDENTITY;
	PRINT 'Inserting system user with id ' + cast(@@IDENTITY as varchar(100));
	INSERT INTO cam.UserAccount(PrincipalId, AdGuid, CreatedOn, RevisedOn, RevisedBy, CreatedBy, AccountStatusId, LastName, FirstName, DisplayName, EmailAddress)
	VALUES (@systemUserId, @systemUserAdGuid, sysdatetimeoffset(), sysdatetimeoffset(), 1, 1, 1, 'System', 'NFN', 'System', NULL)

END
ELSE
	SELECT @systemUserId = PrincipalId FROM cam.UserAccount WHERE AdGuid = @systemUserAdGuid;


IF NOT EXISTS(SELECT * FROM cam.UserAccount WHERE AdGuid = @thomasMorganAdGuid)
BEGIN
	INSERT INTO cam.Principal(PrincipalTypeId) VALUES (1);
	SET @thomasMorganUserId = @@IDENTITY;
	PRINT 'Inserting thomas morgan user with id ' + cast(@@IDENTITY as varchar(100));
	INSERT INTO cam.UserAccount(PrincipalId, AdGuid, CreatedOn, RevisedOn, RevisedBy, CreatedBy, AccountStatusId, LastName, FirstName, DisplayName, EmailAddress)
	VALUES (@thomasMorganUserId, @thomasMorganAdGuid, sysdatetimeoffset(), sysdatetimeoffset(), 1, 1, 1, 'Morgan', 'Thomas', 'Thomas Morgan', 'thomas.morgan@buchanan-edwards.com')

END
ELSE
	SELECT @thomasMorganUserId = PrincipalId FROM cam.UserAccount WHERE AdGuid = @thomasMorganAdGuid;



IF NOT EXISTS(SELECT * FROM cam.UserAccount WHERE AdGuid = @brianGibowskiAdGuid)
BEGIN
	INSERT INTO cam.Principal(PrincipalTypeId) VALUES (1);
	SET @brianGibowskiUserId = @@IDENTITY;
	PRINT 'Inserting brian gibowski user with id ' + cast(@@IDENTITY as varchar(100));
	INSERT INTO cam.UserAccount(PrincipalId, AdGuid, CreatedOn, RevisedOn, RevisedBy, CreatedBy, AccountStatusId, LastName, FirstName, DisplayName, EmailAddress)
	VALUES (@brianGibowskiUserId, @brianGibowskiAdGuid, sysdatetimeoffset(), sysdatetimeoffset(), 1, 1, 1, 'Gibowski', 'Brian', 'Brian Gibowski', 'brian.gibowski@buchanan-edwards.com')

END
ELSE
	SELECT @brianGibowskiUserId = PrincipalId FROM cam.UserAccount WHERE AdGuid = @brianGibowskiAdGuid;


IF NOT EXISTS(SELECT * FROM cam.UserAccount WHERE AdGuid = @brandonTuckerAdGuid)
BEGIN
	INSERT INTO cam.Principal(PrincipalTypeId) VALUES (1);
	SET @brandonTuckerUserId = @@IDENTITY;
	PRINT 'Inserting brandon tucker user with id ' + cast(@@IDENTITY as varchar(100));
	INSERT INTO cam.UserAccount(PrincipalId, AdGuid, CreatedOn, RevisedOn, RevisedBy, CreatedBy, AccountStatusId, LastName, FirstName, DisplayName, EmailAddress)
	VALUES (@brandonTuckerUserId, @brandonTuckerAdGuid, sysdatetimeoffset(), sysdatetimeoffset(), 1, 1, 1, 'Tucker', 'Brandon', 'Brandon Tucker', 'brandon.tucker@buchanan-edwards.com')

END
ELSE
	SELECT @brandonTuckerUserId = PrincipalId FROM cam.UserAccount WHERE AdGuid = @brandonTuckerAdGuid;



MERGE INTO cam.[PermissionAssignment] AS Target
USING (VALUES

	(@thomasMorganUserId, @kmtApplicationId, @systemAdminPermissionId, sysdatetimeoffset(), @systemUserId, 1),
	(@brianGibowskiUserId, @kmtApplicationId, @systemAdminPermissionId, sysdatetimeoffset(), @systemUserId, 1),
	(@brandonTuckerUserId, @kmtApplicationId, @systemAdminPermissionId, sysdatetimeoffset(), @systemUserId, 1)
)
AS Source(PrincipalId, ResourceId, PermissionId, AssignedOn, AssignedBy, IsAllowed)
ON Target.PrincipalId = Source.PrincipalId AND Target.ResourceId = @kmtApplicationId AND Target.PermissionId = @systemAdminPermissionId
--set is allowed to tru
WHEN MATCHED THEN
UPDATE SET IsAllowed = 1

--Insert new records
WHEN NOT MATCHED BY TARGET THEN
	INSERT (PrincipalId, ResourceId, PermissionId, AssignedOn, AssignedBy, IsAllowed)
	VALUES (PrincipalId, ResourceId, PermissionId, AssignedOn, AssignedBy, IsAllowed);


	
MERGE INTO cam.[PrincipalRole] AS Target
USING (VALUES

	(@thomasMorganUserId, @kmtSuperUserRoleId, @systemUserId, sysdatetimeoffset()),
	(@brianGibowskiUserId, @kmtSuperUserRoleId, @systemUserId, sysdatetimeoffset()),
	(@brandonTuckerUserId, @kmtSuperUserRoleId, @systemUserId, sysdatetimeoffset())
)
AS Source(PrincipalId, RoleId, AssignedBy, AssignedOn)
ON Target.PrincipalId = Source.PrincipalId AND Target.RoleId = Source.RoleId
--set is allowed to true
--WHEN MATCHED THEN
--Insert new records
WHEN NOT MATCHED BY TARGET THEN
	INSERT (PrincipalId, RoleId, AssignedBy, AssignedOn)
	VALUES (PrincipalId, RoleId, AssignedBy, AssignedOn);
