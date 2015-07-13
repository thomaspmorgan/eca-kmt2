CREATE TABLE [dbo].[ActivityType] (
    [ActivityTypeId]       INT                IDENTITY (1, 1) NOT NULL,
    [ActivityTypeName]     NVARCHAR (MAX)     NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.ActivityType] PRIMARY KEY CLUSTERED ([ActivityTypeId] ASC)
);

