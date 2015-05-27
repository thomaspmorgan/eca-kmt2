CREATE TABLE [dbo].[ParticipantStatus] (
    [ParticipantStatusId]           INT                IDENTITY (1, 1) NOT NULL,
    [Status]                        NVARCHAR(50)                NOT NULL,
    [History_CreatedBy]             INT                NOT NULL,
    [History_CreatedOn]             DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]             INT                NOT NULL,
    [History_RevisedOn]             DATETIMEOFFSET (7) NOT NULL,
    [StatusDate]                    DATETIMEOFFSET (7) DEFAULT ('0001-01-01T00:00:00.000+00:00') NULL,
    CONSTRAINT [PK_dbo.ParticipantStatus] PRIMARY KEY CLUSTERED ([ParticipantStatusId] ASC)
);


GO


CREATE INDEX [IX_ParticipantStatus_StatusDate] ON [dbo].[ParticipantStatus] ([StatusDate])
