CREATE TABLE [dbo].[Language] (
    [LanguageId] INT                IDENTITY (1, 1) NOT NULL,
    [LanguageName]          NVARCHAR (100)     NOT NULL,
    [History_CreatedBy]     INT                NOT NULL,
    [History_CreatedOn]     DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]     INT                NOT NULL,
    [History_RevisedOn]     DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.Language] PRIMARY KEY CLUSTERED ([LanguageId] ASC)
);

