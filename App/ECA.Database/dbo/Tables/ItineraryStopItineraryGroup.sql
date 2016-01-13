CREATE TABLE [dbo].[ItineraryStopItineraryGroup]
(
	[ItineraryGroupId] INT NOT NULL,
	[ItineraryStopId] INT NOT NULL,

	CONSTRAINT [PK_dbo.ItineraryStopItineraryGroup] PRIMARY KEY CLUSTERED ([ItineraryGroupId] ASC, [ItineraryStopId] ASC),
	CONSTRAINT [FK_dbo.ItineraryStopItineraryGroup_dbo.ItineraryGroup_ItineraryGroupId] FOREIGN KEY ([ItineraryGroupId]) REFERENCES [dbo].[ItineraryGroup]([ItineraryGroupId]),
	CONSTRAINT [FK_dbo.ItineraryStopItineraryGroup_dbo.ItineraryStop_ItineraryStopId] FOREIGN KEY ([ItineraryStopId]) REFERENCES [dbo].[ItineraryStop]([ItineraryStopId])
)
