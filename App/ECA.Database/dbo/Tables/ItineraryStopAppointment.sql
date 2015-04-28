CREATE TABLE [dbo].[ItineraryStopAppointment]
(
	[ItineraryStopId] INT NOT NULL , 
    [AppointmentId] INT NOT NULL, 
    CONSTRAINT [FK_dbo.ItineraryStopAppointment_dbo.ItineraryStop_ItineraryStopId] FOREIGN KEY ([ItineraryStopId]) REFERENCES [dbo].[ItineraryStop]([ItineraryStopId]), 
    CONSTRAINT [FK_dbo.ItineraryStopAppointment_dbo.ItineraryAppointment_AppointmentId] FOREIGN KEY ([AppointmentId]) REFERENCES [dbo].[ItineraryAppointment]([AppointmentId]), 
    CONSTRAINT [PK_dbo.ItineraryStopAppointment] PRIMARY KEY ([ItineraryStopId] ASC, [AppointmentId] ASC)
)

GO

CREATE INDEX [IX_ItineraryStopId] ON [dbo].[ItineraryStopAppointment] ([ItineraryStopId])

GO

CREATE INDEX [IX_AppointmentId] ON [dbo].[ItineraryStopAppointment] ([AppointmentId])
