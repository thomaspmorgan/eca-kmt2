CREATE TABLE [dbo].[PhoneNumber] (
    [PhoneNumberId]     INT            IDENTITY (1, 1) NOT NULL,
    [Number]            NVARCHAR (50) NOT NULL,
    [PhoneNumberTypeId] INT            NOT NULL,
    [Contact_ContactId] INT            NULL,
    [Person_PersonId]   INT            NULL,
	[History_CreatedBy] INT NOT NULL DEFAULT 1, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL DEFAULT sysdatetimeoffset(), 
    [History_RevisedBy] INT NOT NULL DEFAULT 1, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL DEFAULT sysdatetimeoffset(), 
    [IsPrimary] BIT NULL DEFAULT 0, 
    CONSTRAINT [PK_dbo.PhoneNumber] PRIMARY KEY CLUSTERED ([PhoneNumberId] ASC),
    CONSTRAINT [FK_dbo.PhoneNumber_dbo.Contact_Contact_ContactId] FOREIGN KEY ([Contact_ContactId]) REFERENCES [dbo].[Contact] ([ContactId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.PhoneNumber_dbo.Person_Person_PersonId] FOREIGN KEY ([Person_PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE
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

