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
:r .\StaticData\dbo.ActivityType.staticdata.sql
:r .\StaticData\dbo.ActorType.staticdata.sql
:r .\StaticData\dbo.AddressType.staticdata.sql
:r .\StaticData\dbo.ArtifactType.staticdata.sql
:r .\StaticData\dbo.EmailAddressType.staticdata.sql
:r .\StaticData\dbo.Focus.staticdata.sql
:r .\StaticData\dbo.Gender.staticdata.sql
:r .\StaticData\dbo.ImpactType.staticdata.sql
:r .\StaticData\dbo.ItineraryStatus.staticdata.sql
:r .\StaticData\dbo.LocationType.staticdata.sql
:r .\StaticData\dbo.MaritalStatus.staticdata.sql
:r .\StaticData\dbo.MoneyFlowSourceRecipientType.staticdata.sql
:r .\StaticData\dbo.MoneyFlowSourceRecipientTypeSettings.staticdata.sql
:r .\StaticData\dbo.MoneyFlowStatus.staticdata.sql
:r .\StaticData\dbo.MoneyFlowType.staticdata.sql
:r .\StaticData\dbo.OrganizationRole.staticdata.sql
:r .\StaticData\dbo.OrganizationType.staticdata.sql
:r .\StaticData\dbo.ParticipantStatus.staticdata.sql
:r .\StaticData\dbo.ParticipantType.staticdata.sql
:r .\StaticData\dbo.PhoneNumberType.staticdata.sql
:r .\StaticData\dbo.ProgramType.staticdata.sql
:r .\StaticData\dbo.ProjectStatus.staticdata.sql
:r .\StaticData\dbo.ProgramStatus.staticdata.sql
:r .\StaticData\dbo.ProgramType.staticdata.sql
:r .\StaticData\dbo.ProjectType.staticdata.sql
:r .\StaticData\dbo.ProminentCategory.staticdata.sql
:r .\StaticData\dbo.SocialMediaType.staticdata.sql
:r .\StaticData\dbo.SevisCommStatus.staticdata.sql

:r .\CAM.InsertResources.sql
:r .\CAM.GrantKMTSuperUserRolePermissions.sql