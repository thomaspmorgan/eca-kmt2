CREATE TABLE [dbo].[CitizenCountry] (
    [PersonId]   INT NOT NULL,
    [LocationId] INT NOT NULL,
    CONSTRAINT [PK_dbo.CitizenCountry] PRIMARY KEY CLUSTERED ([PersonId] ASC, [LocationId] ASC),
    CONSTRAINT [FK_dbo.CitizenCountry_dbo.Location_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.CitizenCountry_dbo.Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_PersonId]
    ON [dbo].[CitizenCountry]([PersonId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LocationId]
    ON [dbo].[CitizenCountry]([LocationId] ASC);

