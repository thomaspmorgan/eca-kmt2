CREATE TABLE [dbo].[InterestSpecialization] (
    [InterestSpecializationId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]                     NVARCHAR (MAX) NOT NULL,
    [Person_PersonId]          INT            NULL,
    CONSTRAINT [PK_dbo.InterestSpecialization] PRIMARY KEY CLUSTERED ([InterestSpecializationId] ASC),
    CONSTRAINT [FK_dbo.InterestSpecialization_dbo.Person_Person_PersonId] FOREIGN KEY ([Person_PersonId]) REFERENCES [dbo].[Person] ([PersonId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Person_PersonId]
    ON [dbo].[InterestSpecialization]([Person_PersonId] ASC);

