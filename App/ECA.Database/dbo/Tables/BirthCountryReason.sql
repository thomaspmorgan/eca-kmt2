CREATE TABLE [dbo].[BirthCountryReason](
	[BirthCountryReasonId] [int] IDENTITY(1,1) NOT NULL,
	[BirthReasonCode] [nvarchar](2) NOT NULL,
	[Description] [nvarchar](40) NOT NULL,
	[History_CreatedBy] [int] NOT NULL,
	[History_CreatedOn] [datetimeoffset](7) NOT NULL,
	[History_RevisedBy] [int] NOT NULL,
	[History_RevisedOn] [datetimeoffset](7) NOT NULL,
    CONSTRAINT [PK_BirthCountryReason] PRIMARY KEY ([BirthCountryReasonId])
)
