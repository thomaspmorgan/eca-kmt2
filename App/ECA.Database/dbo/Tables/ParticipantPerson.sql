﻿CREATE TABLE [dbo].[ParticipantPerson]
(
	[ParticipantId] INT NOT NULL PRIMARY KEY, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    [SevisId] VARCHAR(15) NULL, 
    [FieldOfStudyId] INT NULL, 
    [StudyProject] NVARCHAR(250) NULL, 
    [ProgramCategoryId] INT NULL, 
    [PositionId] INT NULL, 
    [HostInstitutionId] INT NULL, 
    [HomeInstitutionId] INT NULL, 
    [IsSentToSevisViaRTI] BIT NOT NULL DEFAULT 0, 
    [IsValidatedViaRTI] BIT NOT NULL DEFAULT 0, 
    [IsCancelled] BIT NOT NULL DEFAULT 0, 
    [IsDS2019Printed] BIT NOT NULL DEFAULT 0, 
    [IsNeedsUpdate] BIT NOT NULL DEFAULT 0, 
    [IsDS2019SentToTraveler] BIT NOT NULL DEFAULT 0, 
    [StartDate] DATETIMEOFFSET NULL, 
    [EndDate] DATETIMEOFFSET NULL, 
    [FundingSponsor] DECIMAL(12, 2) NULL, 
    [FundingPersonal] DECIMAL(12, 2) NULL, 
    [FundingVisGovt] DECIMAL(12, 2) NULL, 
    [FundingVisBNC] DECIMAL(12, 2) NULL, 
    [FundingGovtAgency1] DECIMAL(12, 2) NULL, 
    [FundingGovtAgency2] DECIMAL(12, 2) NULL, 
    [FundingIntlOrg1] DECIMAL(12, 2) NULL, 
    [FundingIntlOrg2] DECIMAL(12, 2) NULL, 
    [FundingOther] DECIMAL(12, 2) NULL, 
    [FundingTotal] DECIMAL(12, 2) NULL, 
    CONSTRAINT [FK_ParticipantPerson_HostInstitution_ToOrganization] FOREIGN KEY ([HostInstitutionId]) REFERENCES [Organization]([OrganizationId]), 
    CONSTRAINT [FK_ParticipantPerson_HomeInstitution_ToOrganization] FOREIGN KEY ([HomeInstitutionId]) REFERENCES [Organization]([OrganizationId]), 
    CONSTRAINT [FK_ParticipantPerson_ToFieldOfStudy] FOREIGN KEY ([FieldOfStudyId]) REFERENCES [sevis].[FieldOfStudy]([FieldOfStudyId]), 
    CONSTRAINT [FK_ParticipantPerson_ToPosition] FOREIGN KEY ([PositionId]) REFERENCES [sevis].[Position]([PositionId]), 
    CONSTRAINT [FK_ParticipantPerson_ToParticipant] FOREIGN KEY ([ParticipantId]) REFERENCES [Participant]([ParticipantId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_ParticipantPerson_ToProgramCategory] FOREIGN KEY ([ProgramCategoryId]) REFERENCES [sevis].[ProgramCategory]([ProgramCategoryId]) 
)

GO

CREATE INDEX [IX_ParticipantPerson_SevisId] ON [dbo].[ParticipantPerson] ([SevisId])

GO

CREATE INDEX [IX_ParticipantPerson_FieldOfStudyCode] ON [dbo].[ParticipantPerson] ([FieldOfStudyId])

GO

CREATE INDEX [IX_ParticipantPerson_PositionCode] ON [dbo].[ParticipantPerson] ([PositionId])

GO

CREATE INDEX [IX_ParticipantPerson_HostInstitution] ON [dbo].[ParticipantPerson] ([HostInstitutionId])

GO

CREATE INDEX [IX_ParticipantPerson_HomeInstitution] ON [dbo].[ParticipantPerson] ([HomeInstitutionId])

GO

CREATE INDEX [IX_ParticipantPerson_ProgramCategoryCode] ON [dbo].[ParticipantPerson] ([ProgramCategoryId])
