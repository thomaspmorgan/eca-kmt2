﻿CREATE TABLE [dbo].[SevisBatchProcessing]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[BatchId] UNIQUEIDENTIFIER NOT NULL,
    [SubmitDate] DATETIMEOFFSET NULL, 
    [RetrieveDate] DATETIMEOFFSET NULL, 
    [SendXml] XML NULL, 
    [TransactionLogXml] XML NULL, 
    [UploadDispositionCode] NCHAR(5) NULL, 
    [ProcessDispositionCode] NCHAR(5) NULL, 
    [DownloadDispositionCode] NCHAR(5) NULL
)

GO

CREATE INDEX [IX_BatchId] ON [dbo].[SevisBatchProcessing] ([BatchId])
