CREATE TABLE [dbo].[ItineraryAppointmentComment]
(
	[ItineraryAppointmentCommentId] INT IDENTITY (1, 1) NOT NULL, 
    [AppointmentId] INT NOT NULL, 
    [ItineraryCommentTypeId] INT NOT NULL, 
    [Comment] NVARCHAR(500) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_dbo.ItineraryAppointmentComment] PRIMARY KEY ([ItineraryAppointmentCommentId]), 
    CONSTRAINT [FK_dbo.ItineraryAppointmentComment_dbo.ItineraryAppointment] FOREIGN KEY ([AppointmentId]) REFERENCES [dbo].[ItineraryAppointment]([AppointmentId]), 
    CONSTRAINT [FK_dbo.ItineraryAppointmentComment_dbo.ItineraryCommentType] FOREIGN KEY ([ItineraryCommentTypeId]) REFERENCES [dbo].[ItineraryCommentType]([ItineraryCommentTypeId])
)

GO

CREATE INDEX [IX_AppointmentId] ON [dbo].[ItineraryAppointmentComment] ([AppointmentId])

GO

CREATE INDEX [IX_ItineraryCommentTypeId] ON [dbo].[ItineraryAppointmentComment] ([ItineraryCommentTypeId])
