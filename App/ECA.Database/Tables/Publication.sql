CREATE TABLE [dbo].[Publication] (
    [PublicationId]     INT                IDENTITY (1, 1) NOT NULL,
    [PublicationName]   NVARCHAR (MAX)     NOT NULL,
    [Work]              NVARCHAR (MAX)     NOT NULL,
    [PublicationDate]   DATETIMEOFFSET (7) NOT NULL,
    [PersonId]          INT                NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.Publication] PRIMARY KEY CLUSTERED ([PublicationId] ASC),
    CONSTRAINT [FK_dbo.Publication_dbo.Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId])
);


GO
CREATE NONCLUSTERED INDEX [IX_PersonId]
    ON [dbo].[Publication]([PersonId] ASC);

