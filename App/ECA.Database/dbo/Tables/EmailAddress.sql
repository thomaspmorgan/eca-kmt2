CREATE TABLE [dbo].[EmailAddress] (
    [EmailAddressId]    INT            IDENTITY (1, 1) NOT NULL,
	[EmailAddressTypeId] INT NULL,
    [Address]           NVARCHAR (MAX) NULL,
    [Contact_ContactId] INT            NULL,
    [Person_PersonId]   INT            NULL,
    CONSTRAINT [PK_dbo.EmailAddress] PRIMARY KEY CLUSTERED ([EmailAddressId] ASC),
    CONSTRAINT [FK_dbo.EmailAddress_dbo.Contact_Contact_ContactId] FOREIGN KEY ([Contact_ContactId]) REFERENCES [dbo].[Contact] ([ContactId]),
    CONSTRAINT [FK_dbo.EmailAddress_dbo.Person_Person_PersonId] FOREIGN KEY ([Person_PersonId]) REFERENCES [dbo].[Person] ([PersonId]), 
    CONSTRAINT [FK_EmailAddress_ToEmailAddressType] FOREIGN KEY ([EmailAddressTypeId]) REFERENCES [EmailAddressType]([EmailAddressTypeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Contact_ContactId]
    ON [dbo].[EmailAddress]([Contact_ContactId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Person_PersonId]
    ON [dbo].[EmailAddress]([Person_PersonId] ASC);

