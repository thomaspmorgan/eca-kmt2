CREATE TABLE [dbo].[Location] (
    [LocationId]         INT                IDENTITY (1, 1) NOT NULL,
    [LocationTypeId]     INT                NOT NULL,
    [Latitude]           REAL               NULL,
    [Longitude]          REAL               NULL,
    [Street1]            NVARCHAR (100)     NULL,
    [Street2]            NVARCHAR (100)     NULL,
    [Street3]            NVARCHAR (100)     NULL,
    [City]               NVARCHAR (200)     NULL,
    [Division]           NVARCHAR (100)     NULL,
    [PostalCode]         NVARCHAR (50)     NULL,
    [LocationName]       NVARCHAR (200)     NULL,
    [LocationIso]        NVARCHAR (50)     NULL,
    [History_CreatedBy]  INT                NOT NULL,
    [History_CreatedOn]  DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]  INT                NOT NULL,
    [History_RevisedOn]  DATETIMEOFFSET (7) NOT NULL,
    [Region_LocationId]  INT                NULL,
    [Country_LocationId] INT                NULL,
    [LocationISO-2] NVARCHAR(10) NULL, 
    [City_LocationId] INT NULL, 
    [Division_LocationId] INT NULL, 
    [IsDivisionUnknown] BIT NULL DEFAULT 0, 
    [IsActive] BIT NOT NULL , 
    [SEVISCountryCodeId] INT NULL, 
    CONSTRAINT [PK_dbo.Location] PRIMARY KEY CLUSTERED ([LocationId] ASC),
    CONSTRAINT [FK_dbo.Location_dbo.Location_Country_LocationId] FOREIGN KEY ([Country_LocationId]) REFERENCES [dbo].[Location] ([LocationId]),
    CONSTRAINT [FK_dbo.Location_dbo.Location_Region_LocationId] FOREIGN KEY ([Region_LocationId]) REFERENCES [dbo].[Location] ([LocationId]),
    CONSTRAINT [FK_dbo.Location_dbo.LocationType_LocationTypeId] FOREIGN KEY ([LocationTypeId]) REFERENCES [dbo].[LocationType] ([LocationTypeId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_dbo.Location_dbo.Location_City_LocationId] FOREIGN KEY ([City_LocationId]) REFERENCES [dbo].[Location]([LocationId]), 
    CONSTRAINT [FK_dbo.Location_dbo.Location_Division_LocationId] FOREIGN KEY ([Division_LocationId]) REFERENCES [dbo].[Location]([LocationId]), 
    CONSTRAINT [FK_dbo.Location_sevisBirthCountry_CountryCode] FOREIGN KEY ([SEVISCountryCodeId]) REFERENCES [Sevis].[BirthCountry]([BirthCountryId])
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


GO

CREATE NONCLUSTERED INDEX [IX_LocationName] 
	ON [dbo].[Location] ([LocationName] ASC)

GO

CREATE NONCLUSTERED INDEX [IX_City] ON [dbo].[Location] ([City])

GO

CREATE NONCLUSTERED INDEX [IX_City_LocationId] ON [dbo].[Location] ([City_LocationId])
