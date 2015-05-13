CREATE TABLE [dbo].[ItineraryStopPerson] (
    [ItineraryStopId] INT NOT NULL,
    [PersonId]   INT NOT NULL,
    CONSTRAINT [PK_dbo.ItineraryStopPerson] PRIMARY KEY CLUSTERED ([ItineraryStopId] ASC, [PersonId] ASC),
    CONSTRAINT [FK_dbo.ItineraryStopPerson_dbo.ItineraryStop_ItineraryStopId] FOREIGN KEY ([ItineraryStopId]) REFERENCES [dbo].[ItineraryStop] ([ItineraryStopId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ItineraryStopPerson_dbo.Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ItineraryStopId]
    ON [dbo].[ItineraryStopPerson]([ItineraryStopId] ASC);


GO


CREATE NONCLUSTERED INDEX [IX_PersonId] ON [dbo].[ItineraryStopPerson] ([PersonId] ASC)
