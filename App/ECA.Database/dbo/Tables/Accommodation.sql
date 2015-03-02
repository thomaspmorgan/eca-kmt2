CREATE TABLE [dbo].[Accommodation] (
    [AccommodationId]     INT                IDENTITY (1, 1) NOT NULL,
    [CheckIn]             DATETIMEOFFSET (7) NOT NULL,
    [CheckOut]            DATETIMEOFFSET (7) NOT NULL,
    [RecordLocator]       NVARCHAR (MAX)     NULL,
    [History_CreatedBy]   INT                NOT NULL,
    [History_CreatedOn]   DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]   INT                NOT NULL,
    [History_RevisedOn]   DATETIMEOFFSET (7) NOT NULL,
    [Host_OrganizationId] INT                NOT NULL,
    CONSTRAINT [PK_dbo.Accommodation] PRIMARY KEY CLUSTERED ([AccommodationId] ASC),
    CONSTRAINT [FK_dbo.Accommodation_dbo.Organization_Host_OrganizationId] FOREIGN KEY ([Host_OrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Host_OrganizationId]
    ON [dbo].[Accommodation]([Host_OrganizationId] ASC);

