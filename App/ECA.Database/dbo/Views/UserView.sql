CREATE VIEW [dbo].[UserView]
	AS SELECT PrincipalId, LastName, FirstName, DisplayName, EmailAddress FROM [cam].[UserAccount]

