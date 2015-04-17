CREATE TABLE [dbo].[LocationType] (
    [LocationTypeId]    INT                IDENTITY (1, 1) NOT NULL,
    [LocationTypeName]  NVARCHAR (50)     NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.LocationType] PRIMARY KEY CLUSTERED ([LocationTypeId] ASC)
);

