CREATE TABLE [sevis].[InternationalOrganization]
(
	[OrganizationId] INT IDENTITY(1,1) NOT NULL, 
    [OrganizationCode] NVARCHAR(10) NOT NULL, 
    [Description] NVARCHAR(250) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.InternationalOrganization] PRIMARY KEY ([OrganizationId]) 
)

GO



CREATE INDEX [IX_InternationalOrganization_OrganizationCode] ON [sevis].[InternationalOrganization] ([OrganizationCode])

GO
