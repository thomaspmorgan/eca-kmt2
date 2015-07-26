CREATE TABLE [dbo].[ItineraryStopTheme]
(
	[ItineraryStopId] INT NOT NULL , 
    [ItineraryThemeId] INT NOT NULL, 
    CONSTRAINT [PK_dbo.ItineraryStopTheme] PRIMARY KEY ([ItineraryStopId] ASC, [ItineraryThemeId] ASC), 
    CONSTRAINT [FK_dbo.ItineraryStopTheme_dbo.ItineraryStop_ItineraryStopId] FOREIGN KEY ([ItineraryStopId]) REFERENCES [dbo].[ItineraryStop]([ItineraryStopId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_dbo.ItineraryStopTheme_dbo.ItineraryTheme_ItineraryThemeId] FOREIGN KEY ([ItineraryThemeId]) REFERENCES [dbo].[ItineraryTheme]([ItineraryThemeId]) ON DELETE CASCADE
)

GO

CREATE INDEX [IX_ItineraryStopId] ON [dbo].[ItineraryStopTheme] ([ItineraryStopId])

GO

CREATE INDEX [IX_ItineraryThemeId] ON [dbo].[ItineraryStopTheme] ([ItineraryThemeId])
