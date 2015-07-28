CREATE TABLE [dbo].[ProfessionEducation] (
    [ProfessionEducationId]       INT                IDENTITY (1, 1) NOT NULL,
    [Title]                       NVARCHAR (120)     NOT NULL,
    [Role]                        NVARCHAR (120)     NULL,
    [OrganizationId]              INT                NULL,
    [DateFrom]                    DATETIMEOFFSET (7) NOT NULL,
    [DateTo]                      DATETIMEOFFSET (7) NULL,
    [History_CreatedBy]           INT                NOT NULL,
    [History_CreatedOn]           DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]           INT                NOT NULL,
    [History_RevisedOn]           DATETIMEOFFSET (7) NOT NULL,
    [PersonOfEducation_PersonId]  INT                NULL,
    [PersonOfProfession_PersonId] INT                NULL,
    CONSTRAINT [PK_dbo.ProfessionEducation] PRIMARY KEY CLUSTERED ([ProfessionEducationId] ASC),
    CONSTRAINT [FK_dbo.ProfessionEducation_dbo.Organization_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProfessionEducation_dbo.Person_PersonOfEducation_PersonId] FOREIGN KEY ([PersonOfEducation_PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProfessionEducation_dbo.Person_PersonOfProfession_PersonId] FOREIGN KEY ([PersonOfProfession_PersonId]) REFERENCES [dbo].[Person] ([PersonId])
);


GO
CREATE NONCLUSTERED INDEX [IX_OrganizationId]
    ON [dbo].[ProfessionEducation]([OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PersonOfEducation_PersonId]
    ON [dbo].[ProfessionEducation]([PersonOfEducation_PersonId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PersonOfProfession_PersonId]
    ON [dbo].[ProfessionEducation]([PersonOfProfession_PersonId] ASC);

