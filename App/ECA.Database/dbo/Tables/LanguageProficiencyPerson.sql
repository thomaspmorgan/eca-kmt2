﻿CREATE TABLE [dbo].[LanguageProficiencyPerson] (
    [LanguageProficiency_LanguageProficiencyId] INT NOT NULL,
    [Person_PersonId]                           INT NOT NULL,
    [NativeLanguageInd] CHAR NOT NULL DEFAULT 'N', 
    [SpeakingProficiency] INT NULL DEFAULT 0, 
    [ReadingProficiency] INT NULL DEFAULT 0, 
    [ComprehensionProficiency] INT NULL DEFAULT 0, 
    CONSTRAINT [PK_dbo.LanguageProficiencyPerson] PRIMARY KEY CLUSTERED ([LanguageProficiency_LanguageProficiencyId] ASC, [Person_PersonId] ASC, [NativeLanguageInd] ASC),
    CONSTRAINT [FK_dbo.LanguageProficiencyPerson_dbo.LanguageProficiency_LanguageProficiency_LanguageProficiencyId] FOREIGN KEY ([LanguageProficiency_LanguageProficiencyId]) REFERENCES [dbo].[LanguageProficiency] ([LanguageProficiencyId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.LanguageProficiencyPerson_dbo.Person_Person_PersonId] FOREIGN KEY ([Person_PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE 
);


GO
CREATE NONCLUSTERED INDEX [IX_LanguageProficiency_LanguageProficiencyId]
    ON [dbo].[LanguageProficiencyPerson]([LanguageProficiency_LanguageProficiencyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Person_PersonId]
    ON [dbo].[LanguageProficiencyPerson]([Person_PersonId] ASC);

