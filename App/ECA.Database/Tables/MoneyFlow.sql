CREATE TABLE [dbo].[MoneyFlow] (
    [MoneyFlowId]               INT                IDENTITY (1, 1) NOT NULL,
    [Value]                     REAL               NOT NULL,
    [RecipientAccommodationId]  INT                NOT NULL,
    [History_CreatedBy]         INT                NOT NULL,
    [History_CreatedOn]         DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]         INT                NOT NULL,
    [History_RevisedOn]         DATETIMEOFFSET (7) NOT NULL,
    [Parent_MoneyFlowId]        INT                NULL,
    [RecipientOrganizationId]   INT                NULL,
    [SourceOrganizationId]      INT                NULL,
    [MoneyFlowStatusId]         INT                DEFAULT ((0)) NOT NULL,
    [MoneyFlowTypeId]           INT                DEFAULT ((0)) NOT NULL,
    [SourceTypeId]              INT                DEFAULT ((0)) NOT NULL,
    [RecipientTypeId]           INT                DEFAULT ((0)) NOT NULL,
    [SourceProgramId]           INT                NULL,
    [RecipientProgramId]        INT                NULL,
    [SourceProjectId]           INT                NULL,
    [RecipientProjectId]        INT                NULL,
    [SourceParticipantId]       INT                NULL,
    [RecipientParticipantId]    INT                NULL,
    [SourceItineraryStopId]     INT                NULL,
    [RecipientItineraryStopId]  INT                NULL,
    [RecipientTransportationId] INT                NULL,
    [Description]               NVARCHAR (255)     NULL,
    CONSTRAINT [PK_dbo.MoneyFlow] PRIMARY KEY CLUSTERED ([MoneyFlowId] ASC),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.Accommodation_AccommodationId] FOREIGN KEY ([RecipientAccommodationId]) REFERENCES [dbo].[Accommodation] ([AccommodationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.ItineraryStop_RecipientItineraryStopId] FOREIGN KEY ([RecipientItineraryStopId]) REFERENCES [dbo].[ItineraryStop] ([ItineraryStopId]),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.ItineraryStop_SourceItineraryStopId] FOREIGN KEY ([SourceItineraryStopId]) REFERENCES [dbo].[ItineraryStop] ([ItineraryStopId]),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.MoneyFlow_Parent_MoneyFlowId] FOREIGN KEY ([Parent_MoneyFlowId]) REFERENCES [dbo].[MoneyFlow] ([MoneyFlowId]),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.MoneyFlowSourceRecipientType_RecipientTypeId] FOREIGN KEY ([RecipientTypeId]) REFERENCES [dbo].[MoneyFlowSourceRecipientType] ([MoneyFlowSourceRecipientTypeId]),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.MoneyFlowSourceRecipientType_SourceTypeId] FOREIGN KEY ([SourceTypeId]) REFERENCES [dbo].[MoneyFlowSourceRecipientType] ([MoneyFlowSourceRecipientTypeId]),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.MoneyFlowStatus_MoneyFlowStatus_MoneyFlowStatusId] FOREIGN KEY ([MoneyFlowStatusId]) REFERENCES [dbo].[MoneyFlowStatus] ([MoneyFlowStatusId]),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.MoneyFlowType_MoneyFlowType_MoneyFlowTypeId] FOREIGN KEY ([MoneyFlowTypeId]) REFERENCES [dbo].[MoneyFlowType] ([MoneyFlowTypeId]),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.Organization_Recipient_OrganizationId] FOREIGN KEY ([RecipientOrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.Organization_Source_OrganizationId] FOREIGN KEY ([SourceOrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.Participant_RecipientParticipantId] FOREIGN KEY ([RecipientParticipantId]) REFERENCES [dbo].[Participant] ([ParticipantId]),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.Participant_SourceParticipantId] FOREIGN KEY ([SourceParticipantId]) REFERENCES [dbo].[Participant] ([ParticipantId]),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.Program_RecipientProgramId] FOREIGN KEY ([RecipientProgramId]) REFERENCES [dbo].[Program] ([ProgramId]),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.Program_SourceProgramId] FOREIGN KEY ([SourceProgramId]) REFERENCES [dbo].[Program] ([ProgramId]),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.Project_RecipientProjectId] FOREIGN KEY ([RecipientProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.Project_SourceProjectId] FOREIGN KEY ([SourceProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_dbo.MoneyFlow_dbo.Transportation_RecipientTransportationId] FOREIGN KEY ([RecipientTransportationId]) REFERENCES [dbo].[Transportation] ([TransportationId])
);


GO
CREATE NONCLUSTERED INDEX [IX_SourceOrganizationId]
    ON [dbo].[MoneyFlow]([SourceOrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RecipientOrganizationId]
    ON [dbo].[MoneyFlow]([RecipientOrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SourceTypeId]
    ON [dbo].[MoneyFlow]([SourceTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RecipientTypeId]
    ON [dbo].[MoneyFlow]([RecipientTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RecipientAccommodationId]
    ON [dbo].[MoneyFlow]([RecipientAccommodationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Parent_MoneyFlowId]
    ON [dbo].[MoneyFlow]([Parent_MoneyFlowId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SourceProgramId]
    ON [dbo].[MoneyFlow]([SourceProgramId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RecipientProgramId]
    ON [dbo].[MoneyFlow]([RecipientProgramId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MoneyFlowStatusId]
    ON [dbo].[MoneyFlow]([MoneyFlowStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MoneyFlowTypeId]
    ON [dbo].[MoneyFlow]([MoneyFlowTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SourceProjectId]
    ON [dbo].[MoneyFlow]([SourceProjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RecipientProjectId]
    ON [dbo].[MoneyFlow]([RecipientProjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SourceParticipantId]
    ON [dbo].[MoneyFlow]([SourceParticipantId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RecipientParticipantId]
    ON [dbo].[MoneyFlow]([RecipientParticipantId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SourceItineraryStopId]
    ON [dbo].[MoneyFlow]([SourceItineraryStopId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RecipientItineraryStopId]
    ON [dbo].[MoneyFlow]([RecipientItineraryStopId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RecipientTransportationId]
    ON [dbo].[MoneyFlow]([RecipientTransportationId] ASC);

