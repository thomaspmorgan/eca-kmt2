CREATE TABLE [dbo].[Person] (
	[PersonId]            INT                IDENTITY (1, 1) NOT NULL,
	[FirstName]           NVARCHAR (50)     NULL,
	[LastName]           NVARCHAR (50)     NULL,
	[SecondLastName]           NVARCHAR (50)     NULL,
	[NamePrefix]           NVARCHAR (10)     NULL,
	[NameSuffix]           NVARCHAR (10)     NULL,
	[GivenName]           NVARCHAR (50)     NULL,
	[FamilyName]           NVARCHAR (50)     NULL,
	[MiddleName]           NVARCHAR (50)     NULL,
	[Patronym]           NVARCHAR (50)     NULL,
	[Alias]           NVARCHAR (50)     NULL,
    [GenderId]            INT                NOT NULL,
    [DateOfBirth]         DATETIMEOFFSET (7) NOT NULL,
    [Ethnicity]           NVARCHAR (MAX)     NULL,
    [PermissionToContact] BIT                NOT NULL,
    [EvaluationRetention] NVARCHAR (MAX)     NULL,
    [History_CreatedBy]   INT                NOT NULL,
    [History_CreatedOn]   DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]   INT                NOT NULL,
    [History_RevisedOn]   DATETIMEOFFSET (7) NOT NULL,
    [Location_LocationId] INT                NULL,
    [MedicalConditions]   NVARCHAR (MAX)     NULL,
    [Awards]              NVARCHAR (MAX)     NULL,
    
    [MaritalStatusId] INT NULL, 
    CONSTRAINT [PK_dbo.Person] PRIMARY KEY CLUSTERED ([PersonId] ASC),
    CONSTRAINT [FK_dbo.Person_dbo.Location_Location_LocationId] FOREIGN KEY ([Location_LocationId]) REFERENCES [dbo].[Location] ([LocationId]), 
    CONSTRAINT [FK_Person_ToGender] FOREIGN KEY ([GenderId]) REFERENCES [Gender]([GenderId]), 
    CONSTRAINT [FK_Person_ToTable] FOREIGN KEY ([MaritalStatusId]) REFERENCES [MaritalStatus]([MaritalStatusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_GenderId]
    ON [dbo].[Person]([GenderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Location_LocationId]
    ON [dbo].[Person]([Location_LocationId] ASC);

