CREATE TABLE [dbo].[DefaultExchangeVisitorFunding]
(
	[ProjectId] INT NOT NULL PRIMARY KEY,
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
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
    CONSTRAINT [FK_DefaultExchangeVisitorFunding_ToProject] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project]([ProjectId]), 
    CONSTRAINT [FK_DefaultExchangeVisitorFunding_ToUSGovernmentAgency_1] FOREIGN KEY ([GovtAgency1Id]) REFERENCES [sevis].[USGovernmentAgency]([AgencyId]), 
    CONSTRAINT [FK_DefaultExchangeVisitorFunding_ToUSGovermentAgency_2] FOREIGN KEY ([GovtAgency2Id]) REFERENCES [sevis].[USGovernmentAgency]([AgencyId]), 
    CONSTRAINT [FK_DefaultExchangeVisitorFunding_ToIntlOrg1] FOREIGN KEY ([IntlOrg1Id]) REFERENCES [sevis].[InternationalOrganization]([OrganizationId]), 
    CONSTRAINT [FK_DefaultExchangeVisitorFunding_ToIntlOrg2] FOREIGN KEY ([IntlOrg2Id]) REFERENCES [sevis].[InternationalOrganization]([OrganizationId]) 
)
GO

CREATE INDEX [IX_DefaultExchangeVisitorFunding_USGovernmentAgency1] ON [dbo].[DefaultExchangeVisitorFunding] (GovtAgency1Id)

GO


CREATE INDEX [IX_DefaultExchangeVisitorFunding_USGovernmentAgency2] ON [dbo].[DefaultExchangeVisitorFunding] (GovtAgency2Id)

GO


CREATE INDEX [IX_DefaultExchangeVisitorFunding_IntlOrg1] ON [dbo].[DefaultExchangeVisitorFunding] (IntlOrg1Id)

GO

CREATE INDEX [IX_DefaultExchangeVisitorFunding_IntlOrg2] ON [dbo].[DefaultExchangeVisitorFunding] (IntlOrg2Id)

GO
