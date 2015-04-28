CREATE TABLE [dbo].[ItineraryAppointmentStatus]
(
	[AppointmentStatusId] INT IDENTITY (1, 1) NOT NULL , 
    [Status] NVARCHAR(50) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_dbo.ItineraryAppointmentStatus] PRIMARY KEY ([AppointmentStatusId] ASC)
)
