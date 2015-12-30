CREATE TABLE [dbo].[Itinerary] (
    [ItineraryId]       INT                IDENTITY (1, 1) NOT NULL,
    [ItineraryStatusId] INT                NOT NULL,
    [StartDate]         DATETIMEOFFSET (7) NULL,
    [EndDate]           DATETIMEOFFSET (7) NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    [ProjectId] INT NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL,     
    [Arrival_LocationId] INT NULL, 
    [Departure_LocationId] INT NULL, 
    [IVLP_ItineraryId] NVARCHAR(32) NULL, 
    CONSTRAINT [PK_dbo.Itinerary] PRIMARY KEY CLUSTERED ([ItineraryId] ASC),
    CONSTRAINT [FK_dbo.Itinerary_dbo.ItineraryStatus_ItineraryStatusId] FOREIGN KEY ([ItineraryStatusId]) REFERENCES [dbo].[ItineraryStatus] ([ItineraryStatusId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_dbo.Itinerary_dbo.Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project]([ProjectId]), 
	CONSTRAINT [FK_dbo.Itinerary_dbo.Location_Arrival_LocationId] FOREIGN KEY ([Arrival_LocationId]) REFERENCES [dbo].[Location]([LocationId]),
	CONSTRAINT [FK_dbo.Itinerary_dbo.Location_Departure_LocationId] FOREIGN KEY ([Departure_LocationId]) REFERENCES [dbo].[Location]([LocationId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ItineraryStatusId]
    ON [dbo].[Itinerary]([ItineraryStatusId] ASC);


GO

CREATE INDEX [IX_ItineraryProjectId] ON [dbo].[Itinerary] ([ProjectId])
