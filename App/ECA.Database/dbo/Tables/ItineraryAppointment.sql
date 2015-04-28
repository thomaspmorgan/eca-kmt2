CREATE TABLE [dbo].[ItineraryAppointment]
(
	[AppointmentId] INT IDENTITY (1, 1) NOT NULL, 
    [ProjectId] INT NOT NULL, 
    [StartDate] DATETIMEOFFSET NOT NULL, 
    [EndDate] DATETIMEOFFSET NOT NULL, 
    [AppointmentTypeId] INT NOT NULL, 
    [AppointmentStatusId] INT NOT NULL, 
    [TravelDescription] NVARCHAR(100) NOT NULL, 
    [DepartureTime] DATETIMEOFFSET NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [FK_dbo.ItineraryAppointment_dbo.Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project]([ProjectId]), 
    CONSTRAINT [PK_dbo.ItineraryAppointment] PRIMARY KEY ([AppointmentId]), 
    CONSTRAINT [FK_dbo.ItineraryAppointment_dbo.ItineraryAppointmentStatus] FOREIGN KEY ([AppointmentStatusId]) REFERENCES [dbo].[ItineraryAppointmentStatus]([AppointmentStatusId]), 
    CONSTRAINT [FK_dbo.ItineraryAppointment_dbo.ItineraryAppointmentType] FOREIGN KEY ([AppointmentTypeId]) REFERENCES [dbo].[ItineraryAppointmentType]([AppointmentTypeId]) 
)

GO


CREATE INDEX [IX_dbo.ItineraryAppointment_ProjectId] ON [dbo].[ItineraryAppointment] ([ProjectId])

GO

CREATE INDEX [IX_dbo.ItineraryAppointment_AppointmentTypeId] ON [dbo].[ItineraryAppointment] ([AppointmentTypeId])

GO

CREATE INDEX [IX_dbo.ItineraryAppointment_AppointmentStatusId] ON [dbo].[ItineraryAppointment] ([AppointmentStatusId])
