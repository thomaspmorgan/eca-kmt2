CREATE TABLE [dbo].[Organization] (
    [OrganizationId]                    INT                IDENTITY (1, 1) NOT NULL,
    [OrganizationTypeId]                INT                NOT NULL,
	[OfficeSymbol]                      NVARCHAR (128)     NULL,
    [Description]                       NVARCHAR (3000)     NOT NULL,
    [Status]                            NVARCHAR (20)     NOT NULL,
    [Name]                              NVARCHAR (600)     NOT NULL,
    [Website]                           NVARCHAR (2000)     NULL,
    [History_CreatedBy]                 INT                NOT NULL,
    [History_CreatedOn]                 DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]                 INT                NOT NULL,
    [History_RevisedOn]                 DATETIMEOFFSET (7) NOT NULL,
    [ParentOrganization_OrganizationId] INT                NULL,
    CONSTRAINT [PK_dbo.Organization] PRIMARY KEY CLUSTERED ([OrganizationId] ASC),
    CONSTRAINT [FK_dbo.Organization_dbo.Organization_ParentOrganization_OrganizationId] FOREIGN KEY ([ParentOrganization_OrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_dbo.Organization_dbo.OrganizationType_OrganizationTypeId] FOREIGN KEY ([OrganizationTypeId]) REFERENCES [dbo].[OrganizationType] ([OrganizationTypeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_OrganizationTypeId]
    ON [dbo].[Organization]([OrganizationTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ParentOrganization_OrganizationId]
    ON [dbo].[Organization]([ParentOrganization_OrganizationId] ASC);

