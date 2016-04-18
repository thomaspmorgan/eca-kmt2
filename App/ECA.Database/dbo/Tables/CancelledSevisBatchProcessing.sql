CREATE TABLE [dbo].[CancelledSevisBatchProcessing]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[BatchId] NVARCHAR(14) NOT NULL,
	[SevisUsername] NVARCHAR(10) NOT NULL, 
    [SevisOrgId] NVARCHAR(15) NOT NULL, 
    [SubmitDate] DATETIMEOFFSET NULL, 
    [RetrieveDate] DATETIMEOFFSET NULL, 
    [SendXml] XML NULL, 
    [TransactionLogXml] XML NULL, 
    [UploadDispositionCode] NCHAR(5) NULL, 
    [ProcessDispositionCode] NCHAR(5) NULL, 
    [DownloadDispositionCode] NCHAR(5) NULL,
	[UploadTries] INT NOT NULL DEFAULT 0, 
	[DownloadTries] INT NOT NULL DEFAULT 0,
    [LastUploadTry] DATETIMEOFFSET NULL, 
    [LastDownloadTry] DATETIMEOFFSET NULL,
	[CancelledOn] DATETIMEOFFSET NOT NULL,
	[Reason] NVARCHAR(MAX) NOT NULL
)

GO
