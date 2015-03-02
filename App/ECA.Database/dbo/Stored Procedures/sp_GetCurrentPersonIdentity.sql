CREATE PROCEDURE sp_GetCurrentPersonIdentity
AS
RETURN IDENT_CURRENT('dbo.Person')
