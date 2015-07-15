CREATE TABLE [dbo].[ActivityPerson] (
    [Person_PersonId] INT NOT NULL,
    [Activity_ActivityId]   INT NOT NULL,
    CONSTRAINT [PK_dbo.ActivityPerson] PRIMARY KEY CLUSTERED ([Activity_ActivityId] ASC, [Person_PersonId] ASC),
    CONSTRAINT [FK_dbo.ActivityPerson_dbo.Person_Person_PersonId] FOREIGN KEY ([Person_PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.PersonActivity_dbo.Activity_Activity_ActivityId] FOREIGN KEY ([Activity_ActivityId]) REFERENCES [dbo].[Activity] ([ActivityId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Person_PersonId]
    ON [dbo].[ActivityPerson]([Person_PersonId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Activity_ActivityId]
    ON [dbo].[ActivityPerson]([Activity_ActivityId] ASC);

