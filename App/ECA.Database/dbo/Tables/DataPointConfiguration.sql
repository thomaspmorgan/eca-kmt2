CREATE TABLE [dbo].[DataPointConfiguration]
(
	[DataPointConfigurationId] INT NOT NULL IDENTITY (1, 1) PRIMARY KEY, 
    [OfficeId] INT NULL, 
    [ProgramId] INT NULL, 
    [ProjectId] INT NULL, 
    [Category] VARCHAR(50) NOT NULL, 
    [Property] VARCHAR(50) NOT NULL, 
    [IsHidden] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [FK_DataPointConfiguration_Office] FOREIGN KEY ([OfficeId]) REFERENCES [Organization]([OrganizationId]), 
    CONSTRAINT [FK_DataPointConfiguration_Program] FOREIGN KEY ([ProgramId]) REFERENCES [Program]([ProgramId]), 
    CONSTRAINT [FK_DataPointConfiguration_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([ProjectId])
)
