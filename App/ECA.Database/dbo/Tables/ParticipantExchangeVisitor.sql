CREATE TABLE [dbo].[ParticipantExchangeVisitor]
(
	[ParticipantId] INT NOT NULL PRIMARY KEY, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    [FieldOfStudyId] INT NULL, 
    [StudyProject] NVARCHAR(250) NULL, 
    [ProgramCategoryId] INT NULL, 
    [PositionId] INT NULL, 
    [StartDate] DATETIMEOFFSET NULL, 
    [EndDate] DATETIMEOFFSET NULL, 
    [FundingSponsor] DECIMAL(12, 2) NULL, 
    [FundingPersonal] DECIMAL(12, 2) NULL, 
    [FundingVisGovt] DECIMAL(12, 2) NULL, 
    [FundingVisBNC] DECIMAL(12, 2) NULL, 
    [FundingGovtAgency1] DECIMAL(12, 2) NULL, 
	[GovtAgency1Id] INT NULL,
	[GovtAgency1OtherName] NVARCHAR(60) NULL,
    [FundingGovtAgency2] DECIMAL(12, 2) NULL,
	[GovtAgency2Id] INT NULL,
	[GovtAgency2OtherName] NVARCHAR(60) NULL,
    [FundingIntlOrg1] DECIMAL(12, 2) NULL, 
	[IntlOrg1Id] INT NULL,
	[IntlOrg1OtherName] NVARCHAR(60) NULL,
    [FundingIntlOrg2] DECIMAL(12, 2) NULL,
	[IntlOrg2Id] INT NULL,
	[IntlOrg2OtherName] NVARCHAR(60) NULL,
    [FundingOther] DECIMAL(12, 2) NULL,
	[OtherName] NVARCHAR(60) NULL, 
    [FundingTotal] DECIMAL(12, 2) NULL,
	CONSTRAINT [FK_ParticipantExchangeVisitor_ToParticipantPerson] FOREIGN KEY ([ParticipantId]) REFERENCES [ParticipantPerson]([ParticipantId]), 
    CONSTRAINT [FK_ParticipantExchangeVisitor_ToFieldOfStudy] FOREIGN KEY ([FieldOfStudyId]) REFERENCES [sevis].[FieldOfStudy]([FieldOfStudyId]), 
    CONSTRAINT [FK_ParticipantExchangeVisitor_ToPosition] FOREIGN KEY ([PositionId]) REFERENCES [sevis].[Position]([PositionId]), 
    CONSTRAINT [FK_ParticipantExchangeVisitor_ToParticipant] FOREIGN KEY ([ParticipantId]) REFERENCES [Participant]([ParticipantId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_ParticipantExchangeVisitor_ToProgramCategory] FOREIGN KEY ([ProgramCategoryId]) REFERENCES [sevis].[ProgramCategory]([ProgramCategoryId]), 
    CONSTRAINT [FK_ParticipantExchangeVisitor_ToUSGovernmentAgency_1] FOREIGN KEY ([GovtAgency1Id]) REFERENCES [sevis].[USGovernmentAgency]([AgencyId]), 
    CONSTRAINT [FK_ParticipantExchangeVisitor_ToUSGovermentAgency_2] FOREIGN KEY ([GovtAgency2Id]) REFERENCES [sevis].[USGovernmentAgency]([AgencyId]), 
    CONSTRAINT [FK_ParticipantExchangeVisitor_ToIntlOrg1] FOREIGN KEY ([IntlOrg1Id]) REFERENCES [sevis].[InternationalOrganization]([OrganizationId]), 
    CONSTRAINT [FK_ParticipantExchangeVisitor_ToIntlOrg2] FOREIGN KEY ([IntlOrg2Id]) REFERENCES [sevis].[InternationalOrganization]([OrganizationId]) 
)

GO

CREATE INDEX [IX_ParticipantExchangeVisitor_SevisId] ON [dbo].[ParticipantPerson] ([SevisId])

GO

CREATE INDEX [IX_ParticipantExchangeVisitor_FieldOfStudyCode] ON [dbo].[ParticipantPerson] ([FieldOfStudyId])

GO

CREATE INDEX [IX_ParticipantExchangeVisitor_PositionCode] ON [dbo].[ParticipantPerson] ([PositionId])

GO


CREATE INDEX [IX_ParticipantExchangeVisitor_ProgramCategoryCode] ON [dbo].[ParticipantPerson] ([ProgramCategoryId])

GO