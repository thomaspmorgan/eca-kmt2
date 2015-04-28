CREATE TABLE [dbo].[ItineraryCommentType]
(
	[ItineraryCommentTypeId] INT IDENTITY (1, 1) NOT NULL , 
    [CommentTypeName] NVARCHAR(50) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_dbo.ItineraryCommentType] PRIMARY KEY ([ItineraryCommentTypeId])
)
