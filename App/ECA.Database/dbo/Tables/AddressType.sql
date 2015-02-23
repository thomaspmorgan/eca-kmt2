CREATE TABLE [dbo].[AddressType] (
    [AddressTypeId]     INT                IDENTITY (1, 1) NOT NULL,
    [AddressName]       NVARCHAR (MAX)     NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.AddressType] PRIMARY KEY CLUSTERED ([AddressTypeId] ASC)
);

