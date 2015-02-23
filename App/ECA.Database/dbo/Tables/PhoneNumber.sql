CREATE TABLE [dbo].[PhoneNumber] (
    [PhoneNumberId]     INT            IDENTITY (1, 1) NOT NULL,
    [Number]            NVARCHAR (MAX) NULL,
    [PhoneNumberTypeId] INT            NOT NULL,
    [Contact_ContactId] INT            NULL,
    [Person_PersonId]   INT            NULL,
    CONSTRAINT [PK_dbo.PhoneNumber] PRIMARY KEY CLUSTERED ([PhoneNumberId] ASC),
    CONSTRAINT [FK_dbo.PhoneNumber_dbo.Contact_Contact_ContactId] FOREIGN KEY ([Contact_ContactId]) REFERENCES [dbo].[Contact] ([ContactId]),
    CONSTRAINT [FK_dbo.PhoneNumber_dbo.Person_Person_PersonId] FOREIGN KEY ([Person_PersonId]) REFERENCES [dbo].[Person] ([PersonId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Contact_ContactId]
    ON [dbo].[PhoneNumber]([Contact_ContactId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Person_PersonId]
    ON [dbo].[PhoneNumber]([Person_PersonId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PhoneNumberTypeId]
    ON [dbo].[PhoneNumber]([PhoneNumberTypeId] ASC);

