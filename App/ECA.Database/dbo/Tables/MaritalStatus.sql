CREATE TABLE [dbo].[MaritalStatus]
(
	[MaritalStatusId] INT NOT NULL IDENTITY (1,1) , 
    [Status] NCHAR(1) NOT NULL, 
    [Description] NCHAR(20) NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL,
	 CONSTRAINT [PK_dbo.NaritalStatus] PRIMARY KEY CLUSTERED ([MaritalStatusId] ASC)
)
