CREATE TABLE [dbo].[PersonDependentCitizenCountry] (
    [DependentId]   INT NOT NULL,
    [LocationId] INT NOT NULL, 
    CONSTRAINT [PK_PersonDependentCitizenCountry] PRIMARY KEY ([DependentId], [LocationId]), 
    CONSTRAINT [FK_PersonDependentCitizenCountry_DependentId] FOREIGN KEY ([DependentId]) REFERENCES [PersonDependent]([DependentId]), 
    CONSTRAINT [FK_PersonDependentCitizenCountry_Locationid] FOREIGN KEY ([LocationId]) REFERENCES [Location]([LocationId]),
);


