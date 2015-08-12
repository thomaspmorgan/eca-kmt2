CREATE TABLE [dbo].[ProminentCategory] (
    [ProminentCategoryId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]                NVARCHAR (255) NOT NULL,
	[History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.ProminentCategory] PRIMARY KEY CLUSTERED ([ProminentCategoryId] ASC)
);


GO
