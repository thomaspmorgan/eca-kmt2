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
:r .\StaticData\dbo.BirthCountryReason.staticdata.sql
:r .\StaticData\dbo.DependentType.staticdata.sql
:r .\StaticData\dbo.DataPointCategory.staticdata.sql
:r .\StaticData\dbo.DataPointProperty.staticdata.sql
:r .\StaticData\dbo.DataPointCategoryProperty.staticdata.sql
:r .\StaticData\dbo.DependentType.staticdata.sql
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
:r .\StaticData\dbo.DependentType.staticdata.sql
:r .\StaticData\dbo.PhoneNumberType.staticdata.sql
:r .\StaticData\dbo.ProgramType.staticdata.sql
:r .\StaticData\dbo.ProjectStatus.staticdata.sql
:r .\StaticData\dbo.ProgramStatus.staticdata.sql
:r .\StaticData\dbo.ProgramType.staticdata.sql
:r .\StaticData\dbo.ProjectType.staticdata.sql
:r .\StaticData\dbo.ProminentCategory.staticdata.sql
:r .\StaticData\dbo.SocialMediaType.staticdata.sql
:r .\StaticData\dbo.SevisCommStatus.staticdata.sql
:r .\StaticData\dbo.VisitorType.staticdata.sql

:r .\StaticData\sevis.BirthCountry.staticdata.sql
:r .\StaticData\sevis.CapGapExtensionType.staticdata.sql
:r .\StaticData\sevis.DependentCancellationReason.staticdata.sql
:r .\StaticData\sevis.DependentTermination.staticdata.sql
:r .\StaticData\sevis.DropBelowFullCourseReason.staticdata.sql
:r .\StaticData\sevis.EducationLevel.staticdata.sql
:r .\StaticData\sevis.EmploymentTime.staticdata.sql
:r .\StaticData\sevis.EndProgramReason.staticdata.sql
:r .\StaticData\sevis.ExchangeVisitorPosition.staticdata.sql
:r .\StaticData\sevis.ExchangeVisitorTerminationReason.staticdata.sql
:r .\StaticData\sevis.FieldOfStudy.staticdata.sql
:r .\StaticData\sevis.InternationalOrganization.staticdata.sql
:r .\StaticData\sevis.OccupationalCategory.staticdata.sql
:r .\StaticData\sevis.Position.staticdata.sql
:r .\StaticData\sevis.ProgramCategory.staticdata.sql
:r .\StaticData\sevis.ProgramSubject.staticdata.sql
:r .\StaticData\sevis.StudentCancellation.staticdata.sql
:r .\StaticData\sevis.StudentCreation.staticdata.sql
:r .\StaticData\sevis.StudentReprint.staticdata.sql
:r .\StaticData\sevis.StudentSecondaryMajorMinor.staticdata.sql
:r .\StaticData\sevis.StudentTermination.staticdata.sql
:r .\StaticData\sevis.USGovernmentAgency.staticdata.sql

:r .\CAM.InsertResources.sql
:r .\CAM.GrantKMTSuperUserRolePermissions.sql