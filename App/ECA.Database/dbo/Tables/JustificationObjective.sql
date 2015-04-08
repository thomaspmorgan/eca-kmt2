CREATE TABLE [dbo].[JustificationObjective]
(
	[ObjectiveId] INT NOT NULL IDENTITY (1,1), 
    [JustificationId] INT NOT NULL, 
    [ObjectiveName] NVARCHAR(50) NOT NULL, 
	[History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
	CONSTRAINT [PK_dbo.JustificationObjective] PRIMARY KEY CLUSTERED ([ObjectiveId] ASC), 
    CONSTRAINT [FK_JustificationObjective_ToJustification] FOREIGN KEY ([JustificationId]) REFERENCES [Justification]([JustificationId])
)
