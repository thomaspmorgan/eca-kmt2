-- =================================================
-- Create User as DBO template for Windows Azure SQL Database
-- =================================================
-- For login <login_name, sysname, login_name>, create a user in the database
CREATE USER KMT_User
	FOR LOGIN KMT_User
	WITH DEFAULT_SCHEMA = dbo
GO

-- Add user to the database owner role
EXEC sp_addrolemember N'db_datareader', N'KMT_User'
EXEC sp_addrolemember N'db_datawriter', N'KMT_User'

GO