CREATE TABLE [dbo].[Activity] (
    [ActivityId]                INT                IDENTITY (1, 1) NOT NULL,
    [Title]                  NVARCHAR (MAX)     NOT NULL,
    [ActivityTypeId]            INT                NOT NULL,
    [ActivityDate]              DATETIMEOFFSET (7) NOT NULL,
    [LocationId]             INT                NOT NULL,
    [Description]            NVARCHAR (MAX)     NULL,
    [TargetAudience]         NVARCHAR (MAX)     NULL,
    [EsimatedAudienceSize]   INT                NOT NULL,
    [EsimatedNumberOfAlumni] INT                NOT NULL,
    [History_CreatedBy]      INT                NOT NULL,
    [History_CreatedOn]      DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]      INT                NOT NULL,
    [History_RevisedOn]      DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.Activity] PRIMARY KEY CLUSTERED ([ActivityId] ASC),
    CONSTRAINT [FK_dbo.Activity_dbo.ActivityType_ActivityTypeId] FOREIGN KEY ([ActivityTypeId]) REFERENCES [dbo].[ActivityType] ([ActivityTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Activity_dbo.Location_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ActivityTypeId]
    ON [dbo].[Activity]([ActivityTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LocationId]
    ON [dbo].[Activity]([LocationId] ASC);

