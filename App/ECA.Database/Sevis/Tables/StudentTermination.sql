﻿CREATE TABLE [sevis].[StudentTermination]
(
	[StudentTerminationId] INT IDENTITY(1,1) NOT NULL, 
    [TerminationCode] CHAR(2) NOT NULL, 
    [Description] NVARCHAR(100) NOT NULL, 
    [F_1_Ind] CHAR NOT NULL, 
    [M_1_Ind] CHAR NOT NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [PK_sevis.StudentTermination] PRIMARY KEY ([StudentTerminationId])
)

GO

CREATE INDEX [IX_TerminationCode] ON [sevis].[StudentTermination] ([TerminationCode])
