CREATE TABLE [dbo].[Gender] (
    [GenderId]          INT                IDENTITY (1, 1) NOT NULL,
    [GenderName]        NVARCHAR (20)      NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.Gender] PRIMARY KEY CLUSTERED ([GenderId] ASC)
);

