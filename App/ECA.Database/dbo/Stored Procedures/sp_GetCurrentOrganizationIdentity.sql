CREATE PROCEDURE sp_GetCurrentOrganizationIdentity
AS
SELECT IDENT_CURRENT('dbo.Organization')
