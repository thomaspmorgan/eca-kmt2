CREATE TABLE [dbo].[Contact] (
    [ContactId]         INT                IDENTITY (1, 1) NOT NULL,
    [FullName]          NVARCHAR (100)     NOT NULL,
    [Position]          NVARCHAR (100)     NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.Contact] PRIMARY KEY CLUSTERED ([ContactId] ASC)
);

