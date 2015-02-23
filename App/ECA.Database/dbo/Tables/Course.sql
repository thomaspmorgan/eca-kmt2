CREATE TABLE [dbo].[Course] (
    [Id]                            INT            IDENTITY (1, 1) NOT NULL,
    [Name]                          NVARCHAR (MAX) NULL,
    [ItineraryStop_ItineraryStopId] INT            NULL,
    CONSTRAINT [PK_dbo.Course] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Course_dbo.ItineraryStop_ItineraryStop_ItineraryStopId] FOREIGN KEY ([ItineraryStop_ItineraryStopId]) REFERENCES [dbo].[ItineraryStop] ([ItineraryStopId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ItineraryStop_ItineraryStopId]
    ON [dbo].[Course]([ItineraryStop_ItineraryStopId] ASC);

