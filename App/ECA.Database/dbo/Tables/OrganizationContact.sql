CREATE TABLE [dbo].[OrganizationContact] (
    [OrganizationId] INT NOT NULL,
    [ContactId]      INT NOT NULL,
    CONSTRAINT [PK_dbo.OrganizationContact] PRIMARY KEY CLUSTERED ([OrganizationId] ASC, [ContactId] ASC),
    CONSTRAINT [FK_dbo.OrganizationContact_dbo.Contact_Contact_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact] ([ContactId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.OrganizationContact_dbo.Organization_Organization_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_OrganizationId]
    ON [dbo].[OrganizationContact]([OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ContactId]
    ON [dbo].[OrganizationContact]([ContactId] ASC);

