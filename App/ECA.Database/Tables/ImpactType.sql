CREATE TABLE [dbo].[ImpactType] (
    [ImpactTypeId]      INT                IDENTITY (1, 1) NOT NULL,
    [Impact]            NVARCHAR (MAX)     NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    [Impact_ImpactId]   INT                NULL,
    CONSTRAINT [PK_dbo.ImpactType] PRIMARY KEY CLUSTERED ([ImpactTypeId] ASC),
    CONSTRAINT [FK_dbo.ImpactType_dbo.Impact_Impact_ImpactId] FOREIGN KEY ([Impact_ImpactId]) REFERENCES [dbo].[Impact] ([ImpactId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Impact_ImpactId]
    ON [dbo].[ImpactType]([Impact_ImpactId] ASC);

