CREATE TABLE [dbo].[Focus]
(
	[FocusId]            INT                IDENTITY (1, 1) NOT NULL,
    [FocusName]          NVARCHAR (255)     NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    [OfficeId] INT NULL, 
    CONSTRAINT [PK_dbo.Focus] PRIMARY KEY CLUSTERED ([FocusId] ASC), 
    CONSTRAINT [FK_Focus_ToOffice] FOREIGN KEY ([OfficeId]) REFERENCES [Organization]([OrganizationId])
)
