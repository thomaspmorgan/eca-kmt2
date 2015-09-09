CREATE TABLE [dbo].[Website]
(
	[WebsiteId] INT NOT NULL PRIMARY KEY IDENTITY (1, 1), 
    [WebsiteValue] NVARCHAR(4000) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL
)
