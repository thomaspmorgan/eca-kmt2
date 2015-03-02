CREATE TABLE [dbo].[PhoneNumberType] (
    [PhoneNumberTypeId]   INT                IDENTITY (1, 1) NOT NULL,
    [PhoneNumberTypeName] NVARCHAR (20)      NOT NULL,
    [History_CreatedBy]   INT                NOT NULL,
    [History_CreatedOn]   DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]   INT                NOT NULL,
    [History_RevisedOn]   DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.PhoneNumberType] PRIMARY KEY CLUSTERED ([PhoneNumberTypeId] ASC)
);

