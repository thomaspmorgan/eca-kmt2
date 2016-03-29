CREATE TABLE [dbo].[ParticipantPerson]
(
	[ParticipantId] INT NOT NULL PRIMARY KEY, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    [SevisId] VARCHAR(15) NULL, 
    [HostInstitutionId] INT NULL, 
	[HostInstitutionAddressId] INT NULL, 
    [HomeInstitutionId] INT NULL, 
	[HomeInstitutionAddressId] INT NULL, 
    [IsSentToSevisViaRTI] BIT NOT NULL DEFAULT 0, 
    [IsValidatedViaRTI] BIT NOT NULL DEFAULT 0, 
    [IsCancelled] BIT NOT NULL DEFAULT 0, 
    [IsDS2019Printed] BIT NOT NULL DEFAULT 0, 
    [IsNeedsUpdate] BIT NOT NULL DEFAULT 0, 
    [IsDS2019SentToTraveler] BIT NOT NULL DEFAULT 0, 
    [StartDate] DATETIMEOFFSET NULL, 
    [EndDate] DATETIMEOFFSET NULL, 
    [SevisValidationResult] NVARCHAR(MAX) NULL, 
    [SevisBatchResult] NVARCHAR(MAX) NULL, 
    [DS2019FileUrl] NCHAR(8000) NULL, 
    CONSTRAINT [FK_ParticipantPerson_HostInstitution_ToOrganization] FOREIGN KEY ([HostInstitutionId]) REFERENCES [Organization]([OrganizationId]), 
    CONSTRAINT [FK_ParticipantPerson_HomeInstitution_ToOrganization] FOREIGN KEY ([HomeInstitutionId]) REFERENCES [Organization]([OrganizationId]), 
	CONSTRAINT [FK_ParticipantPerson_HomeInstitutionAddress_ToAddress] FOREIGN KEY ([HomeInstitutionAddressId]) REFERENCES [Address]([AddressId]), 
	CONSTRAINT [FK_ParticipantPerson_HostInstitutionAddress_ToAddress] FOREIGN KEY ([HostInstitutionAddressId]) REFERENCES [Address]([AddressId])
)

GO

CREATE INDEX [IX_ParticipantPerson_SevisId] ON [dbo].[ParticipantPerson] ([SevisId])

GO

CREATE INDEX [IX_ParticipantPerson_HostInstitution] ON [dbo].[ParticipantPerson] ([HostInstitutionId])

GO

CREATE INDEX [IX_ParticipantPerson_HomeInstitution] ON [dbo].[ParticipantPerson] ([HomeInstitutionId])

GO

