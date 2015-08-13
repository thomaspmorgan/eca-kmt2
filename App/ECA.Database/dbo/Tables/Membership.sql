CREATE TABLE [dbo].[Membership] (
    [MembershipId]    INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (255) NOT NULL,
    [Person_PersonId] INT            NOT NULL,
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_dbo.Membership] PRIMARY KEY CLUSTERED ([MembershipId] ASC),
    CONSTRAINT [FK_dbo.Membership_dbo.Person_Person_PersonId] FOREIGN KEY ([Person_PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Person_PersonId]
    ON [dbo].[Membership]([Person_PersonId] ASC);

