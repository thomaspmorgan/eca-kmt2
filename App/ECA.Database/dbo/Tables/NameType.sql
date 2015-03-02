CREATE TABLE [dbo].[NameType] (
    [NameTypeId]        INT                IDENTITY (1, 1) NOT NULL,
    [NameTypeName]      NVARCHAR (20)      NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.NameType] PRIMARY KEY CLUSTERED ([NameTypeId] ASC)
);

