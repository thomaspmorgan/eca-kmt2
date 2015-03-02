CREATE TABLE [dbo].[Person] (
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
    [PersonId]            INT                IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_dbo.Person] PRIMARY KEY CLUSTERED ([PersonId] ASC),
    CONSTRAINT [FK_dbo.Person_dbo.Location_Location_LocationId] FOREIGN KEY ([Location_LocationId]) REFERENCES [dbo].[Location] ([LocationId])
);


GO
CREATE NONCLUSTERED INDEX [IX_GenderId]
    ON [dbo].[Person]([GenderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Location_LocationId]
    ON [dbo].[Person]([Location_LocationId] ASC);

