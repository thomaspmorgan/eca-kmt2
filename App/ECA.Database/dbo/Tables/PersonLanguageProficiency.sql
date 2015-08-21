CREATE TABLE [dbo].[PersonLanguageProficiency] (
    [LanguageId] INT NOT NULL,
    [PersonId]                           INT NOT NULL,
    [NativeLanguageInd] CHAR NOT NULL DEFAULT 'N', 
    [SpeakingProficiency] INT NULL DEFAULT 0, 
    [ReadingProficiency] INT NULL DEFAULT 0, 
    [ComprehensionProficiency] INT NULL DEFAULT 0, 
    [History_CreatedBy] INT NOT NULL DEFAULT 1, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL DEFAULT sysdatetimeoffset(), 
    [History_RevisedBy] INT NOT NULL DEFAULT 1, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL DEFAULT sysdatetimeoffset(), 
    CONSTRAINT [PK_dbo.PersonLanguageProficiency] PRIMARY KEY CLUSTERED ([LanguageId] ASC, [PersonId] ASC),
    CONSTRAINT [FK_dbo.PersonLanguageProficiency_dbo.Language_LanguageId] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language] ([LanguageId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.PersonLanguageProficiency_dbo.Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE 
);


GO
CREATE NONCLUSTERED INDEX [IX_Language_LanguageId]
    ON [dbo].[PersonLanguageProficiency]([LanguageId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Person_PersonId]
    ON [dbo].[PersonLanguageProficiency]([PersonId] ASC);

