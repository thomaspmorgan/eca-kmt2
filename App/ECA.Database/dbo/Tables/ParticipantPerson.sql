CREATE TABLE [dbo].[ParticipantPerson]
(
	[ParticipantId] INT NOT NULL PRIMARY KEY, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    [SevisId] VARCHAR(15) NULL, 
    [ContactAgreement] BIT NOT NULL DEFAULT 0, 
    [FieldOfStudyId] INT NULL, 
    [StudyProject] NVARCHAR(250) NULL, 
    [ProgramSubjectId] INT NULL, 
    [PositionId] INT NULL, 
    [HostInstitutionId] INT NULL, 
    [HomeInstitutionId] INT NULL, 
    CONSTRAINT [FK_ParticipantPerson_HostInstitution_ToOrganization] FOREIGN KEY ([HostInstitutionId]) REFERENCES [Organization]([OrganizationId]), 
    CONSTRAINT [FK_ParticipantPerson_HomeInstitution_ToOrganization] FOREIGN KEY ([HomeInstitutionId]) REFERENCES [Organization]([OrganizationId]), 
    CONSTRAINT [FK_ParticipantPerson_ToFieldOfStudy] FOREIGN KEY ([FieldOfStudyId]) REFERENCES [sevis].[FieldOfStudy]([FieldOfStudyId]), 
    CONSTRAINT [FK_ParticipantPerson_ToPosition] FOREIGN KEY ([PositionId]) REFERENCES [sevis].[Position]([PositionId]), 
    CONSTRAINT [FK_ParticipantPerson_ToProgramSubject] FOREIGN KEY ([ProgramSubjectId]) REFERENCES [sevis].[ProgramSubject]([ProgramSubjectId]) 
)

GO

CREATE INDEX [IX_ParticipantPerson_SevisId] ON [dbo].[ParticipantPerson] ([SevisId])

GO

CREATE INDEX [IX_ParticipantPerson_FieldOfStudyCode] ON [dbo].[ParticipantPerson] ([FieldOfStudyId])

GO

CREATE INDEX [IX_ParticipantPerson_ProgramSubjectCode] ON [dbo].[ParticipantPerson] ([ProgramSubjectId])

GO

CREATE INDEX [IX_ParticipantPerson_PositionCode] ON [dbo].[ParticipantPerson] ([PositionId])

GO

CREATE INDEX [IX_ParticipantPerson_HostInstitution] ON [dbo].[ParticipantPerson] ([HostInstitutionId])

GO

CREATE INDEX [IX_ParticipantPerson_HomeInstitution] ON [dbo].[ParticipantPerson] ([HomeInstitutionId])
