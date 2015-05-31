CREATE TABLE [sevis].[BirthCountry]
(
	[BirthCountryId] INT IDENTITY(1,1) NOT NULL, 
    [CountryCode] CHAR(2) NOT NULL, 
    [CountryName] NVARCHAR(100) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.BirthCountry] PRIMARY KEY ([BirthCountryId])
)

GO

CREATE INDEX [IX_CountryCode] ON [sevis].[BirthCountry] ([CountryCode])
