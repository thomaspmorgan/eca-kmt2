CREATE TABLE [dbo].[SevisCommStatus]
(
	[SevisCommStatusId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SevisCommStatusName] NVARCHAR(50) NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1
)
