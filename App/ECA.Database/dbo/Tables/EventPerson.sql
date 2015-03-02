CREATE TABLE [dbo].[EventPerson] (
    [Person_PersonId] INT NOT NULL,
    [Event_EventId]   INT NOT NULL,
    CONSTRAINT [PK_dbo.EventPerson] PRIMARY KEY CLUSTERED ([Event_EventId] ASC, [Person_PersonId] ASC),
    CONSTRAINT [FK_dbo.EventPerson_dbo.Person_Person_PersonId] FOREIGN KEY ([Person_PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.PersonEvent_dbo.Event_Event_EventId] FOREIGN KEY ([Event_EventId]) REFERENCES [dbo].[Event] ([EventId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Person_PersonId]
    ON [dbo].[EventPerson]([Person_PersonId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Event_EventId]
    ON [dbo].[EventPerson]([Event_EventId] ASC);

