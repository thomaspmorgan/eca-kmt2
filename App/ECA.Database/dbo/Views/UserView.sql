CREATE VIEW [dbo].[UserView]
	AS SELECT PrincipalId, LastName, FirstName, DisplayName, EmailAddress, SevisUsername FROM [cam].[UserAccount]

