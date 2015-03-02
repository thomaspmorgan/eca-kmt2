CREATE TABLE [dbo].[ExternalId] (
    [ExternalIdId]      INT                IDENTITY (1, 1) NOT NULL,
    [ExternalIdValue]   NVARCHAR (MAX)     NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    [Person_PersonId]   INT                NULL,
    CONSTRAINT [PK_dbo.ExternalId] PRIMARY KEY CLUSTERED ([ExternalIdId] ASC),
    CONSTRAINT [FK_dbo.ExternalId_dbo.Person_Person_PersonId] FOREIGN KEY ([Person_PersonId]) REFERENCES [dbo].[Person] ([PersonId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Person_PersonId]
    ON [dbo].[ExternalId]([Person_PersonId] ASC);

