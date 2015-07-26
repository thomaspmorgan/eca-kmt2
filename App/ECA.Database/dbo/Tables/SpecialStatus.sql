CREATE TABLE [dbo].[SpecialStatus] (
    [SpecialStatusId] INT            IDENTITY (1, 1) NOT NULL,
    [Status]          NVARCHAR (MAX) NOT NULL,
    [Person_PersonId] INT            NULL,
    CONSTRAINT [PK_dbo.SpecialStatus] PRIMARY KEY CLUSTERED ([SpecialStatusId] ASC),
    CONSTRAINT [FK_dbo.SpecialStatus_dbo.Person_Person_PersonId] FOREIGN KEY ([Person_PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Person_PersonId]
    ON [dbo].[SpecialStatus]([Person_PersonId] ASC);

