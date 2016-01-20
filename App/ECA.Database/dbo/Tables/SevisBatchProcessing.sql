CREATE TABLE [dbo].[SevisBatchProcessing]
(
	[BatchId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SubmitDate] DATETIMEOFFSET NULL, 
    [RetrieveDate] DATETIMEOFFSET NULL, 
    [SendXml] XML NULL, 
    [TransactionLogXml] XML NULL, 
    [UploadDispositionCode] NCHAR(5) NULL, 
    [ProcessDispositionCode] NCHAR(5) NULL, 
    [DownloadDispositionCode] NCHAR(5) NULL
)
