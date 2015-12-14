CREATE TABLE [dbo].[Project] (
    [ProjectId]                       INT                IDENTITY (1, 1) NOT NULL,
    [Name]                            NVARCHAR (500)     NOT NULL,
    [Description]                     NVARCHAR (3000)     NOT NULL,
    [ProjectTypeId]                   INT                NOT NULL,
	[FocusArea]                       NVARCHAR (100)     NULL,
    [StartDate]                       DATETIMEOFFSET (7) NOT NULL,
    [EndDate]                         DATETIMEOFFSET (7) NULL,
    [Language]                        NVARCHAR (100)     NULL,
    [AudienceReach]                   INT                NOT NULL,
    [ActivityId]                         INT                NULL,
    [History_CreatedBy]               INT                NOT NULL,
    [History_CreatedOn]               DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]               INT                NOT NULL,
    [History_RevisedOn]               DATETIMEOFFSET (7) NOT NULL,
    [NominationSource_OrganizationId] INT                NULL,
    [ProgramId]                       INT                NOT NULL,
    [ProjectStatusId]                 INT                DEFAULT ((0)) NOT NULL,
	[RowVersion] TIMESTAMP NOT NULL, 
    [ProjectNumberIVLP] NVARCHAR(100) NULL, 
    [VisitorTypeId] INT NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_dbo.Project] PRIMARY KEY CLUSTERED ([ProjectId] ASC),
    CONSTRAINT [FK_dbo.Project_dbo.Activity_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activity] ([ActivityId]),
    CONSTRAINT [FK_dbo.Project_dbo.Organization_NominationSource_OrganizationId] FOREIGN KEY ([NominationSource_OrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_dbo.Project_dbo.ProgramId] FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[Program] ([ProgramId]), 
    CONSTRAINT [FK_Project_ToVisitorType] FOREIGN KEY ([VisitorTypeId]) REFERENCES [dbo].[VisitorType] ([VisitorTypeId]) 
);


GO
CREATE NONCLUSTERED INDEX [IX_ActivityId]
    ON [dbo].[Project]([ActivityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NominationSource_OrganizationId]
    ON [dbo].[Project]([NominationSource_OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProgramId]
    ON [dbo].[Project]([ProgramId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectStatusId]
    ON [dbo].[Project]([ProjectStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectTypeId]
    ON [dbo].[Project]([ProjectTypeId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_ProjectNumberIVLP]
    ON [dbo].[Project]([ProjectNumberIVLP]) WHERE [ProjectNumberIVLP] IS NOT NULL;

