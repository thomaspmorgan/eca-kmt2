CREATE TABLE [dbo].[ItineraryAppointmentType]
(
	[AppointmentTypeId] INT IDENTITY (1, 1) NOT NULL, 
    [AppointmentTypeName] NVARCHAR(50) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_dbo.ItineraryAppointmentType] PRIMARY KEY ([AppointmentTypeId])
)
