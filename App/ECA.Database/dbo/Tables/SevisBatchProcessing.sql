CREATE TABLE [dbo].[SevisBatchProcessing]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[BatchId] NVARCHAR(14) NOT NULL,
    [SubmitDate] DATETIMEOFFSET NULL, 
    [RetrieveDate] DATETIMEOFFSET NULL, 
    [SendXml] XML NULL, 
    [TransactionLogXml] XML NULL, 
    [UploadDispositionCode] NCHAR(5) NULL, 
    [ProcessDispositionCode] NCHAR(5) NULL, 
    [DownloadDispositionCode] NCHAR(5) NULL, 
    CONSTRAINT [CK_SevisBatchProcessing_BatchId] UNIQUE (BatchId)
)

GO

CREATE INDEX [IX_BatchId] ON [dbo].[SevisBatchProcessing] ([BatchId])
