CREATE TABLE [dbo].[Category]
(
	[CategoryId] INT NOT NULL IDENTITY (1,1), 
    [FocusId] INT NOT NULL, 
    [CategoryName] NVARCHAR(50) NOT NULL, 
	[History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
	CONSTRAINT [PK_dbo.Category] PRIMARY KEY CLUSTERED ([CategoryId] ASC), 
    CONSTRAINT [FK_Category_ToFocus] FOREIGN KEY ([FocusId]) REFERENCES [Focus]([FocusId]) ON DELETE CASCADE 
)
