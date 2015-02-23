CREATE TABLE [dbo].[Location] (
    [LocationId]         INT                IDENTITY (1, 1) NOT NULL,
    [LocationTypeId]     INT                NOT NULL,
    [Latitude]           REAL               NULL,
    [Longitude]          REAL               NULL,
    [Street1]            NVARCHAR (MAX)     NULL,
    [Street2]            NVARCHAR (MAX)     NULL,
    [Street3]            NVARCHAR (MAX)     NULL,
    [City]               NVARCHAR (MAX)     NULL,
    [Division]           NVARCHAR (MAX)     NULL,
    [PostalCode]         NVARCHAR (MAX)     NULL,
    [LocationName]       NVARCHAR (MAX)     NULL,
    [LocationIso]        NVARCHAR (MAX)     NULL,
    [History_CreatedBy]  INT                NOT NULL,
    [History_CreatedOn]  DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]  INT                NOT NULL,
    [History_RevisedOn]  DATETIMEOFFSET (7) NOT NULL,
    [Region_LocationId]  INT                NULL,
    [Country_LocationId] INT                NULL,
    CONSTRAINT [PK_dbo.Location] PRIMARY KEY CLUSTERED ([LocationId] ASC),
    CONSTRAINT [FK_dbo.Location_dbo.Location_Country_LocationId] FOREIGN KEY ([Country_LocationId]) REFERENCES [dbo].[Location] ([LocationId]),
    CONSTRAINT [FK_dbo.Location_dbo.Location_Region_LocationId] FOREIGN KEY ([Region_LocationId]) REFERENCES [dbo].[Location] ([LocationId]),
    CONSTRAINT [FK_dbo.Location_dbo.LocationType_LocationTypeId] FOREIGN KEY ([LocationTypeId]) REFERENCES [dbo].[LocationType] ([LocationTypeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_LocationTypeId]
    ON [dbo].[Location]([LocationTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Region_LocationId]
    ON [dbo].[Location]([Region_LocationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Country_LocationId]
    ON [dbo].[Location]([Country_LocationId] ASC);

