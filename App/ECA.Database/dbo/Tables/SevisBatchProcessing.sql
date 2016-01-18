CREATE TABLE [dbo].[SevisBatchProcessing]
(
	[BatchId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SubmitDate] DATETIMEOFFSET NOT NULL, 
    [RetrieveDate] DATETIMEOFFSET NULL, 
    [SendXml] XML NOT NULL, 
    [TransactionLogXml] XML NULL, 
    [UploadDispositionCode] NCHAR(5) NULL, 
    [ProcessDispositionCode] NCHAR(5) NULL, 
    [DownloadDispositionCode] NCHAR(5) NULL
)
