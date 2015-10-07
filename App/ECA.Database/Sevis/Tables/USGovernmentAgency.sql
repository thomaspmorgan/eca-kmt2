CREATE TABLE [sevis].[USGovernmentAgency]
(
	[AgencyId] INT IDENTITY(1,1) NOT NULL, 
    [AgencyCode] NVARCHAR(10) NOT NULL, 
    [Description] NVARCHAR(250) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.USGovernmentAgency] PRIMARY KEY ([AgencyId]) 
)

GO



CREATE INDEX [IX_USGovernmentAgency_AgencyCode] ON [sevis].[USGovernmentAgency] ([AgencyCode])

GO
