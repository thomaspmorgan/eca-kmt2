CREATE TABLE [dbo].[Itinerary] (
    [ItineraryId]       INT                IDENTITY (1, 1) NOT NULL,
    [ItineraryStatus]   INT                NOT NULL,
    [StartDate]         DATETIMEOFFSET (7) NOT NULL,
    [EndDate]           DATETIMEOFFSET (7) NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.Itinerary] PRIMARY KEY CLUSTERED ([ItineraryId] ASC)
);

