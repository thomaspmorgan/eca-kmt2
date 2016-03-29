﻿CREATE TABLE [dbo].[PersonDependent](
	[DependentId] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL, 
    [SevisId] VARCHAR(15) NULL, 
	[DependentTypeId] [int] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[NameSuffix] [nvarchar](20) NULL,
	[PassportName] [nvarchar](100) NULL,
	[PreferredName] [nvarchar](100) NULL,
	[GenderId] [int] NOT NULL,
	[DateOfBirth] [datetime2](7) NULL,
	[PlaceOfBirthId] [int] NOT NULL,
	[PlaceOfResidenceId] [int] NOT NULL,
	[BirthCountryReason] [nvarchar](100) NULL,
	[IsTravellingWithParticipant] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[IsSevisDeleted] [bit] NULL,
	[History_CreatedBy] [int] NOT NULL,
	[History_CreatedOn] [datetimeoffset](7) NOT NULL,
	[History_RevisedBy] [int] NOT NULL,
	[History_RevisedOn] [datetimeoffset](7) NOT NULL,
    CONSTRAINT [PK_PersonDependent] PRIMARY KEY ([DependentId]), 
    CONSTRAINT [FK_PersonDependent_ToPerson] FOREIGN KEY ([PersonId]) REFERENCES [Person]([PersonId]),
	CONSTRAINT [FK_PersonDependent_ToGender] FOREIGN KEY ([GenderId]) REFERENCES [Gender]([GenderId]),
	CONSTRAINT [FK_PersonDependent_ToPlaceOfBirth] FOREIGN KEY ([PlaceOfBirthId]) REFERENCES [Location]([LocationId]),
	CONSTRAINT [FK_PersonDependent_ToPlaceOfResidence] FOREIGN KEY ([PlaceOfResidenceId]) REFERENCES [Location]([LocationId])
)

GO
