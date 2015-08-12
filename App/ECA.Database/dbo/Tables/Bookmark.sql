CREATE TABLE [dbo].[Bookmark]
(
	[BookmarkId] INT NOT NULL IDENTITY (1, 1) PRIMARY KEY, 
    [OfficeId] INT NULL, 
    [ProgramId] INT NULL, 
    [ProjectId] INT NULL, 
    [PersonId] INT NULL, 
    [OrganizationId] INT NULL, 
    [PrincipalId] INT NOT NULL, 
    [AddedOn] DATETIMEOFFSET NOT NULL DEFAULT getdate(), 
    [Automatic] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Bookmark_Office] FOREIGN KEY ([OfficeId]) REFERENCES [Organization]([OrganizationId]), 
    CONSTRAINT [FK_Bookmark_Program] FOREIGN KEY ([ProgramId]) REFERENCES [Program]([ProgramId]), 
    CONSTRAINT [FK_Bookmark_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([ProjectId]), 
    CONSTRAINT [FK_Bookmark_Person] FOREIGN KEY ([PersonId]) REFERENCES [Person]([PersonId]), 
    CONSTRAINT [FK_Bookmark_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Organization]([OrganizationId]), 
) 
