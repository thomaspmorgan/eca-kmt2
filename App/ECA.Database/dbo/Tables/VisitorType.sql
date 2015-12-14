CREATE TABLE [dbo].[VisitorType]
(
	[VisitorTypeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [VisitorTypeName] NVARCHAR(20) NOT NULL,
	[History_CreatedBy]   INT                NOT NULL,
    [History_CreatedOn]   DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]   INT                NOT NULL,
    [History_RevisedOn]   DATETIMEOFFSET (7) NOT NULL,
)
