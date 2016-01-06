CREATE TABLE [dbo].[ItineraryStopItineraryGroup]
(
	[ItineraryGroupId] INT NOT NULL,
	[ItineraryStopId] INT NOT NULL,

	CONSTRAINT [PK_dbo.ItineraryGroupItineraryGroup] PRIMARY KEY CLUSTERED ([ItineraryGroupId] ASC, [ItineraryStopId] ASC),
	CONSTRAINT [FK_dbo.ItineraryGroupItineraryGroup_dbo.ItineraryGroup_ItineraryGroupId] FOREIGN KEY ([ItineraryGroupId]) REFERENCES [dbo].[ItineraryGroup]([ItineraryGroupId]),
	CONSTRAINT [FK_dbo.ItineraryGroupItineraryGroup_dbo.ItineraryStop_ItineraryStopId] FOREIGN KEY ([ItineraryStopId]) REFERENCES [dbo].[ItineraryStop]([ItineraryStopId])
)
