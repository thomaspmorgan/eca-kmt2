CREATE TABLE [dbo].[Organization] (
    [OrganizationId]                    INT                IDENTITY (1, 1) NOT NULL,
    [OrganizationTypeId]                INT                NOT NULL,
    [Description]                       NVARCHAR (MAX)     NOT NULL,
    [Status]                            NVARCHAR (MAX)     NOT NULL,
    [Name]                              NVARCHAR (MAX)     NOT NULL,
    [Website]                           NVARCHAR (MAX)     NULL,
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

