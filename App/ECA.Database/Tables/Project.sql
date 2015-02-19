CREATE TABLE [dbo].[Project] (
    [ProjectId]                       INT                IDENTITY (1, 1) NOT NULL,
    [Name]                            NVARCHAR (MAX)     NOT NULL,
    [Description]                     NVARCHAR (MAX)     NOT NULL,
    [ProjectType]                     INT                NOT NULL,
    [FocusArea]                       NVARCHAR (MAX)     NULL,
    [StartDate]                       DATETIMEOFFSET (7) NOT NULL,
    [EndDate]                         DATETIMEOFFSET (7) NULL,
    [Language]                        NVARCHAR (MAX)     NULL,
    [AudienceReach]                   INT                NOT NULL,
    [EventId]                         INT                NULL,
    [History_CreatedBy]               INT                NOT NULL,
    [History_CreatedOn]               DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]               INT                NOT NULL,
    [History_RevisedOn]               DATETIMEOFFSET (7) NOT NULL,
    [NominationSource_OrganizationId] INT                NULL,
    [ProgramId]                       INT                NOT NULL,
    [ProjectStatusId]                 INT                DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.Project] PRIMARY KEY CLUSTERED ([ProjectId] ASC),
    CONSTRAINT [FK_dbo.Project_dbo.Event_EventId] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([EventId]),
    CONSTRAINT [FK_dbo.Project_dbo.Organization_NominationSource_OrganizationId] FOREIGN KEY ([NominationSource_OrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_dbo.Project_dbo.Program_ProgramId] FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[Program] ([ProgramId])
);


GO
CREATE NONCLUSTERED INDEX [IX_EventId]
    ON [dbo].[Project]([EventId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NominationSource_OrganizationId]
    ON [dbo].[Project]([NominationSource_OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProgramId]
    ON [dbo].[Project]([ProgramId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectStatusId]
    ON [dbo].[Project]([ProjectStatusId] ASC);

