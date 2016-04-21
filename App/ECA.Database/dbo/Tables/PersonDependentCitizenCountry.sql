CREATE TABLE [dbo].[PersonDependentCitizenCountry] (
    [DependentId]   INT NOT NULL,
    [LocationId] INT NOT NULL, 
    [IsPrimary] BIT NOT NULL DEFAULT (0), 
    CONSTRAINT [PK_PersonDependentCitizenCountry] PRIMARY KEY ([DependentId], [LocationId]), 
    CONSTRAINT [FK_dbo.PersonDependentCitizenCountry_DependentId] FOREIGN KEY ([DependentId]) REFERENCES [PersonDependent]([DependentId]), 
    CONSTRAINT [FK_dbo.PersonDependentCitizenCountry_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Location]([LocationId]),
);



GO

CREATE INDEX [IX_DependentId] ON [dbo].[PersonDependentCitizenCountry] ([DependentId])

GO

CREATE INDEX [IX_LocationId] ON [dbo].[PersonDependentCitizenCountry] ([LocationId])
