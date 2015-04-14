CREATE TABLE [dbo].[Objective]
(
	[ObjectiveId] INT NOT NULL IDENTITY (1,1), 
    [JustificationId] INT NOT NULL, 
    [ObjectiveName] NVARCHAR(255) NOT NULL, 
	[History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
	CONSTRAINT [PK_dbo.Objective] PRIMARY KEY CLUSTERED ([ObjectiveId] ASC), 
    CONSTRAINT [FK_Objective_ToJustification] FOREIGN KEY ([JustificationId]) REFERENCES [Justification]([JustificationId])
)
