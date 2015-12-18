CREATE TABLE [dbo].[ItineraryGroup]
(
	[ItineraryGroupId]  INT NOT NULL PRIMARY KEY,
	[ItineraryId] INT NOT NULL,
	[Name] VARCHAR(255) NOT NULL,
	[History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,

	CONSTRAINT [FK_dbo.ItineraryGroup_dbo.Itinerary_ItineraryId] FOREIGN KEY ([ItineraryId]) REFERENCES [dbo].[Itinerary]([ItineraryId])
)
