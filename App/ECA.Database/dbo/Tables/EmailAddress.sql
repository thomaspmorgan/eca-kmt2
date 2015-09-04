CREATE TABLE [dbo].[EmailAddress] (
    [EmailAddressId]    INT            IDENTITY (1, 1) NOT NULL,
	[EmailAddressTypeId] INT NOT NULL,
    [Address]           NVARCHAR (100) NOT NULL,
    [Contact_ContactId] INT            NULL,
    [Person_PersonId]   INT            NULL,
    [History_CreatedBy] INT NOT NULL DEFAULT 1, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL DEFAULT sysdatetimeoffset(), 
    [History_RevisedBy] INT NOT NULL DEFAULT 1, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL DEFAULT sysdatetimeoffset(), 
    CONSTRAINT [PK_dbo.EmailAddress] PRIMARY KEY CLUSTERED ([EmailAddressId] ASC),
    CONSTRAINT [FK_dbo.EmailAddress_dbo.Contact_Contact_ContactId] FOREIGN KEY ([Contact_ContactId]) REFERENCES [dbo].[Contact] ([ContactId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.EmailAddress_dbo.Person_Person_PersonId] FOREIGN KEY ([Person_PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_EmailAddress_ToEmailAddressType] FOREIGN KEY ([EmailAddressTypeId]) REFERENCES [EmailAddressType]([EmailAddressTypeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Contact_ContactId]
    ON [dbo].[EmailAddress]([Contact_ContactId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Person_PersonId]
    ON [dbo].[EmailAddress]([Person_PersonId] ASC);

