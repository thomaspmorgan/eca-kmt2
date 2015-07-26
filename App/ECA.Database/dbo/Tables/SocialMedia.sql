CREATE TABLE [dbo].[SocialMedia] (
    [SocialMediaId]     INT                IDENTITY (1, 1) NOT NULL,
    [SocialMediaTypeId] INT                NOT NULL,
    [SocialMediaValue]  NVARCHAR (100)     NULL,
    [OrganizationId]    INT                NULL,
    [PersonId]          INT                NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.SocialMedia] PRIMARY KEY CLUSTERED ([SocialMediaId] ASC),
    CONSTRAINT [FK_dbo.SocialMedia_dbo.Organization_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.SocialMedia_dbo.Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.SocialMedia_dbo.SocialMediaType_SocialMediaTypeId] FOREIGN KEY ([SocialMediaTypeId]) REFERENCES [dbo].[SocialMediaType] ([SocialMediaTypeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_OrganizationId]
    ON [dbo].[SocialMedia]([OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PersonId]
    ON [dbo].[SocialMedia]([PersonId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SocialMediaTypeId]
    ON [dbo].[SocialMedia]([SocialMediaTypeId] ASC);

