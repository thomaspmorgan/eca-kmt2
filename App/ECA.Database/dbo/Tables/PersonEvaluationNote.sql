CREATE TABLE [dbo].[PersonEvaluationNote]
(
	[EvaluationNoteId] INT NOT NULL IDENTITY(1,1), 
    [PersonId] INT NOT NULL, 
    [EvaluationNote] NVARCHAR(MAX) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
	CONSTRAINT [PK_dbo.EvaluationNoteId] PRIMARY KEY CLUSTERED ([EvaluationNoteId] ASC),
    CONSTRAINT [FK_PersonEvaluationNote_ToPerson] FOREIGN KEY ([PersonId]) REFERENCES [Person]([PersonId])
)

GO 

CREATE INDEX [IX_PersonEvaluationNote_PersonId] ON [dbo].[PersonEvaluationNote] ([PersonId])
