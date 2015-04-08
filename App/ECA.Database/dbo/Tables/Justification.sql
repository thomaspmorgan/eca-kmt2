CREATE TABLE [dbo].[Justification]
(
	[JustificationId] INT NOT NULL IDENTITY (1,1), 
    [JustifcationName] NVARCHAR(50) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    [OfficeId] INT NOT NULL, 
	CONSTRAINT [PK_dbo.Justification] PRIMARY KEY CLUSTERED ([JustificationId] ASC), 
    CONSTRAINT [FK_Justification_ToOffice] FOREIGN KEY ([OfficeId]) REFERENCES [Organization]([OrganizationId])
)
