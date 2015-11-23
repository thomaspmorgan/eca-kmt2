CREATE TABLE [dbo].[DataPointConfiguration]
(
	[DataPointConfigurationId] INT NOT NULL IDENTITY (1, 1) PRIMARY KEY, 
    [OfficeId] INT NULL, 
    [ProgramId] INT NULL, 
    [ProjectId] INT NULL, 
    [CategoryId] INT NOT NULL, 
    [PropertyId] INT NOT NULL, 
    [IsHidden] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [FK_DataPointConfiguration_Office] FOREIGN KEY ([OfficeId]) REFERENCES [Organization]([OrganizationId]), 
    CONSTRAINT [FK_DataPointConfiguration_Program] FOREIGN KEY ([ProgramId]) REFERENCES [Program]([ProgramId]), 
    CONSTRAINT [FK_DataPointConfiguration_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([ProjectId]),
    CONSTRAINT [FK_DataPointConfiguration_Category] FOREIGN KEY ([CategoryId]) REFERENCES [DataPointCategory]([DataPointCategoryId]),
    CONSTRAINT [FK_DataPointConfiguration_Property] FOREIGN KEY ([PropertyId]) REFERENCES [DataPointProperty]([DataPointPropertyId])
)
