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
:r .\StaticData\dbo.ActorType.staticdata.sql
:r .\StaticData\dbo.AddressType.staticdata.sql
:r .\StaticData\dbo.ArtifactType.staticdata.sql
:r .\StaticData\dbo.EventType.staticdata.sql
:r .\StaticData\dbo.Focus.staticdata.sql
:r .\StaticData\dbo.Gender.staticdata.sql
:r .\StaticData\dbo.ImpactType.staticdata.sql
:r .\StaticData\dbo.LocationType.staticdata.sql
:r .\StaticData\dbo.NameType.staticdata.sql
:r .\StaticData\dbo.ParticipantType.staticdata.sql
:r .\StaticData\dbo.PhoneNumberType.staticdata.sql
:r .\StaticData\dbo.ProgramType.staticdata.sql
:r .\StaticData\dbo.ProgramStatus.staticdata.sql
:r .\StaticData\dbo.ProjectStatus.staticdata.sql
:r .\StaticData\dbo.ProjectType.staticdata.sql
:r .\StaticData\dbo.SocialMediaType.staticdata.sql

