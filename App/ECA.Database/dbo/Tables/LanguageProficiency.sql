CREATE TABLE [dbo].[LanguageProficiency] (
    [LanguageProficiencyId] INT                IDENTITY (1, 1) NOT NULL,
    [LanguageName]          NVARCHAR (120)     NOT NULL,
    [History_CreatedBy]     INT                NOT NULL,
    [History_CreatedOn]     DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]     INT                NOT NULL,
    [History_RevisedOn]     DATETIMEOFFSET (7) NOT NULL,
    [alpha2] CHAR(2) NOT NULL, 
    [alpha3] CHAR(3) NOT NULL, 
    CONSTRAINT [PK_dbo.LanguageProficiency] PRIMARY KEY CLUSTERED ([LanguageProficiencyId] ASC)
);

