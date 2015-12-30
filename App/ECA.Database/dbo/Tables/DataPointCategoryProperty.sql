CREATE TABLE [dbo].[DataPointCategoryProperty]
(
	[DataPointCategoryPropertyId] INT NOT NULL IDENTITY (1, 1) PRIMARY KEY, 
    [DataPointCategoryId] INT NOT NULL, 
    [DataPointPropertyId] INT NOT NULL
    CONSTRAINT [FK_DataPointCategoryCategory_Category] FOREIGN KEY ([DataPointCategoryId]) REFERENCES [DataPointCategory]([DataPointCategoryId]),
    CONSTRAINT [FK_DataPointCategoryProperty_Property] FOREIGN KEY ([DataPointPropertyId]) REFERENCES [DataPointProperty]([DataPointPropertyId])
)
