CREATE TABLE [dbo].[ItineraryStop] (
    [ItineraryStopId]        INT                IDENTITY (1, 1) NOT NULL,
    [ItineraryStatusId]      INT                NOT NULL,
    [DateArrive]             DATETIMEOFFSET (7) NOT NULL,
    [DateLeave]              DATETIMEOFFSET (7) NOT NULL,
    [ItineraryId]            INT                NOT NULL,
    [History_CreatedBy]      INT                NOT NULL,
    [History_CreatedOn]      DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]      INT                NOT NULL,
    [History_RevisedOn]      DATETIMEOFFSET (7) NOT NULL,
    [Destination_LocationId] INT                NULL,
    [Origin_LocationId]      INT                NULL,
    CONSTRAINT [PK_dbo.ItineraryStop] PRIMARY KEY CLUSTERED ([ItineraryStopId] ASC),
    CONSTRAINT [FK_dbo.ItineraryStop_dbo.Itinerary_ItineraryId] FOREIGN KEY ([ItineraryId]) REFERENCES [dbo].[Itinerary] ([ItineraryId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ItineraryStop_dbo.Location_Destination_LocationId] FOREIGN KEY ([Destination_LocationId]) REFERENCES [dbo].[Location] ([LocationId]),
    CONSTRAINT [FK_dbo.ItineraryStop_dbo.Location_Origin_LocationId] FOREIGN KEY ([Origin_LocationId]) REFERENCES [dbo].[Location] ([LocationId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ItineraryId]
    ON [dbo].[ItineraryStop]([ItineraryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Destination_LocationId]
    ON [dbo].[ItineraryStop]([Destination_LocationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Origin_LocationId]
    ON [dbo].[ItineraryStop]([Origin_LocationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ItineraryStatusId]
    ON [dbo].[ItineraryStop]([ItineraryStatusId] ASC);

