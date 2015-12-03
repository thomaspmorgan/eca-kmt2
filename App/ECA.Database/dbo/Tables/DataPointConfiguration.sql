CREATE TABLE [dbo].[DataPointConfiguration]
(
	[DataPointConfigurationId] INT NOT NULL IDENTITY (1, 1) PRIMARY KEY, 
    [OfficeId] INT NULL, 
    [ProgramId] INT NULL, 
    [ProjectId] INT NULL, 
    [DataPointCategoryPropertyId] INT NOT NULL, 
    CONSTRAINT [FK_DataPointConfiguration_Office] FOREIGN KEY ([OfficeId]) REFERENCES [Organization]([OrganizationId]), 
    CONSTRAINT [FK_DataPointConfiguration_Program] FOREIGN KEY ([ProgramId]) REFERENCES [Program]([ProgramId]), 
    CONSTRAINT [FK_DataPointConfiguration_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([ProjectId]),
    CONSTRAINT [FK_DataPointConfiguration_CategoryProperty] FOREIGN KEY ([DataPointCategoryPropertyId]) REFERENCES [DataPointCategoryProperty]([DataPointCategoryPropertyId])
)
