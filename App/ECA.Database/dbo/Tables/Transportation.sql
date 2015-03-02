CREATE TABLE [dbo].[Transportation] (
    [TransportationId]       INT                IDENTITY (1, 1) NOT NULL,
    [MethodId]               INT                NOT NULL,
    [CarriageId]             NVARCHAR (MAX)     NULL,
    [RecordLocator]          NVARCHAR (MAX)     NULL,
    [ItineraryStopId]        INT                NULL,
    [History_CreatedBy]      INT                NOT NULL,
    [History_CreatedOn]      DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]      INT                NOT NULL,
    [History_RevisedOn]      DATETIMEOFFSET (7) NOT NULL,
    [Carrier_OrganizationId] INT                NULL,
    CONSTRAINT [PK_dbo.Transportation] PRIMARY KEY CLUSTERED ([TransportationId] ASC),
    CONSTRAINT [FK_dbo.Transportation_dbo.ItineraryStop_ItineraryStopId] FOREIGN KEY ([ItineraryStopId]) REFERENCES [dbo].[ItineraryStop] ([ItineraryStopId]),
    CONSTRAINT [FK_dbo.Transportation_dbo.Method_MethodId] FOREIGN KEY ([MethodId]) REFERENCES [dbo].[Method] ([MethodId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Transportation_dbo.Organization_Carrier_OrganizationId] FOREIGN KEY ([Carrier_OrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ItineraryStopId]
    ON [dbo].[Transportation]([ItineraryStopId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Carrier_OrganizationId]
    ON [dbo].[Transportation]([Carrier_OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MethodId]
    ON [dbo].[Transportation]([MethodId] ASC);

