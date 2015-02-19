CREATE TABLE [dbo].[ItineraryStopAccommodation] (
    [ItineraryStop_ItineraryStopId] INT NOT NULL,
    [Accommodation_AccommodationId] INT NOT NULL,
    CONSTRAINT [PK_dbo.ItineraryStopAccommodation] PRIMARY KEY CLUSTERED ([ItineraryStop_ItineraryStopId] ASC, [Accommodation_AccommodationId] ASC),
    CONSTRAINT [FK_dbo.ItineraryStopAccommodation_dbo.Accommodation_Accommodation_AccommodationId] FOREIGN KEY ([Accommodation_AccommodationId]) REFERENCES [dbo].[Accommodation] ([AccommodationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ItineraryStopAccommodation_dbo.ItineraryStop_ItineraryStop_ItineraryStopId] FOREIGN KEY ([ItineraryStop_ItineraryStopId]) REFERENCES [dbo].[ItineraryStop] ([ItineraryStopId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ItineraryStop_ItineraryStopId]
    ON [dbo].[ItineraryStopAccommodation]([ItineraryStop_ItineraryStopId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Accommodation_AccommodationId]
    ON [dbo].[ItineraryStopAccommodation]([Accommodation_AccommodationId] ASC);

