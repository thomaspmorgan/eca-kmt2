/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
:r .\StaticData\CAM.AccountStatus.staticdata.sql
:r .\StaticData\CAM.ResourceType.staticdata.sql
:r .\StaticData\CAM.PrincipalType.staticdata.sql
:r .\StaticData\CAM.Resource.staticdata.sql
:r .\StaticData\CAM.Application.staticdata.sql
:r .\StaticData\CAM.Principal.staticdata.sql
:r .\StaticData\CAM.Permission.staticdata.sql
:r .\StaticData\CAM.Role.staticdata.sql

:r .\AdditionalUsersAndPermissionAssignments.sql
:r .\AdditionalSevisAccounts.sql

