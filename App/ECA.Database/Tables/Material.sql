CREATE TABLE [dbo].[Material] (
    [Name]                          NVARCHAR (MAX) NULL,
    [ItineraryStop_ItineraryStopId] INT            NULL,
    [MaterialId]                    INT            IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_dbo.Material] PRIMARY KEY CLUSTERED ([MaterialId] ASC),
    CONSTRAINT [FK_dbo.Material_dbo.ItineraryStop_ItineraryStop_ItineraryStopId] FOREIGN KEY ([ItineraryStop_ItineraryStopId]) REFERENCES [dbo].[ItineraryStop] ([ItineraryStopId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ItineraryStop_ItineraryStopId]
    ON [dbo].[Material]([ItineraryStop_ItineraryStopId] ASC);

