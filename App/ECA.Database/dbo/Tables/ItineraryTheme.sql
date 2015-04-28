CREATE TABLE [dbo].[ItineraryTheme]
(
	[ItineraryThemeId] INT IDENTITY (1, 1) NOT NULL,	 
    [Theme] NCHAR(200) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_dbo.ItineraryTheme] PRIMARY KEY ([ItineraryThemeId])
)
