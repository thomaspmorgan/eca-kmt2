CREATE TABLE [sevis].[EmploymentTime]
(
	[EmploymentTimeId] INT IDENTITY(1,1) NOT NULL, 
    [EmploymentTimeCode] CHAR(2) NOT NULL, 
    [Description] NVARCHAR(50) NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.EmploymentTime] PRIMARY KEY ([EmploymentTimeId])
)

GO

CREATE INDEX [IX_EmploymentTimeCode] ON [sevis].[EmploymentTime] ([EmploymentTimeCode])
