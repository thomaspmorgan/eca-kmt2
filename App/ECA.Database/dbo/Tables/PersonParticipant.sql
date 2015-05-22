CREATE TABLE [dbo].[ParticipantPerson]
(
	[ParticipantId] INT NOT NULL PRIMARY KEY, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    [SevisId] VARCHAR(15) NULL, 
    [ContactAgreement] BIT NOT NULL DEFAULT 0, 
    [FieldOfStudyCode] CHAR(7) NULL, 
    [StudyProject] NVARCHAR(250) NULL, 
    [ProgramSubjectCode] CHAR(7) NULL, 
    [PositionCode] CHAR(3) NULL, 
    [HostInstitutionId] INT NULL, 
    [HomeInstitutionId] INT NULL, 
    CONSTRAINT [FK_ParticipantPerson_HostInstitution_ToOrganization] FOREIGN KEY ([HostInstitutionId]) REFERENCES [Organization]([OrganizationId]), 
    CONSTRAINT [FK_ParticipantPerson_HomeInstitution_ToOrganization] FOREIGN KEY ([HomeInstitutionId]) REFERENCES [Organization]([OrganizationId]) 
)

GO

CREATE INDEX [IX_ParticipantPerson_SevisId] ON [dbo].[ParticipantPerson] ([SevisId])

GO

CREATE INDEX [IX_ParticipantPerson_FieldOfStudyCode] ON [dbo].[ParticipantPerson] ([FieldOfStudyCode])

GO

CREATE INDEX [IX_ParticipantPerson_ProgramSubjectCode] ON [dbo].[ParticipantPerson] ([ProgramSubjectCode])

GO

CREATE INDEX [IX_ParticipantPerson_PositionCode] ON [dbo].[ParticipantPerson] ([PositionCode])

GO

CREATE INDEX [IX_ParticipantPerson_HostInstitution] ON [dbo].[ParticipantPerson] ([HostInstitutionId])

GO

CREATE INDEX [IX_ParticipantPerson_HomeInstitution] ON [dbo].[ParticipantPerson] ([HomeInstitutionId])
