CREATE TABLE [dbo].[Address] (
    [AddressId]         INT                IDENTITY (1, 1) NOT NULL,
    [AddressTypeId]     INT                NOT NULL,
    [LocationId]        INT                NOT NULL,
	[IsPrimary]			BIT				   NOT NULL DEFAULT(0),
    [PersonId]          INT                NULL,
    [OrganizationId]    INT                NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.Address] PRIMARY KEY CLUSTERED ([AddressId] ASC),
    CONSTRAINT [FK_dbo.Address_dbo.AddressType_AddressTypeId] FOREIGN KEY ([AddressTypeId]) REFERENCES [dbo].[AddressType] ([AddressTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Address_dbo.Location_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Address_dbo.Organization_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Address_dbo.Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AddressTypeId]
    ON [dbo].[Address]([AddressTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LocationId]
    ON [dbo].[Address]([LocationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PersonId]
    ON [dbo].[Address]([PersonId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OrganizationId]
    ON [dbo].[Address]([OrganizationId] ASC);

